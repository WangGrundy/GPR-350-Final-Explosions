using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface Octree
{

    /// <summary>
    /// Inserts a particle into the octree, descending its children as needed.
    /// </summary>
    /// <param name="particle"></param>
    public void Insert(Particle3D particle);

    /// <summary>
    /// Does all necessary collision detection tests.
    /// </summary>
    public void ResolveCollisions();

    /// <summary>
    /// Removes all objects from the Octree.
    /// </summary>
    public void Clear();

    /// <summary>
    /// Creates a new Octree, properly creating children.
    /// </summary>
    /// <param name="pos">The position of this Octree</param>
    /// <param name="halfWidth">The width of this Octree node, from the center to one edge (only needs to be used to calculate children's positions)</param>
    /// <param name="depth">The number of levels beneath this one to create (i.e., depth = 1 means create one node with 8 children. depth = 0 means create only this node. depth = 2 means create one node with 8 children, each of which are Octree's with depth 1.</param>
    /// <returns>The newly created Octree</returns>
    public static Octree Create(Vector3 pos, float halfWidth = 5f, uint depth = 5) //may need to change the depth. Mess around and see which depth is the best
    {
        float newWidth = halfWidth / 2;

        if (depth == 0)
        {
            //create only this node
            //Object goes here

            return new OctreeObjects();
        }
        //create one node with 8 children

        OctreeNode node = new OctreeNode();
        node.position = pos;
        node.children = new Octree[8];
        int i;
        for (i = 0; i < 8; i++)
        {
            // Recursively call Create to initialize the Octree
            Vector3 newPos = pos;
            //the new position will be different for each node

            //assigns the proper math for each index
            switch (i)
            {
                case 0:
                    newPos.x = pos.x - newWidth;
                    newPos.y = pos.y - newWidth;
                    newPos.z = pos.z - newWidth;
                    break;
                case 1:
                    newPos.x = pos.x + newWidth;
                    newPos.y = pos.y - newWidth;
                    newPos.z = pos.z - newWidth;
                    break;
                case 2:
                    newPos.x = pos.x - newWidth;
                    newPos.y = pos.y + newWidth;
                    newPos.z = pos.z - newWidth;
                    break;
                case 3:
                    newPos.x = pos.x + newWidth;
                    newPos.y = pos.y + newWidth;
                    newPos.z = pos.z - newWidth;
                    break;
                case 4:
                    newPos.x = pos.x - newWidth;
                    newPos.y = pos.y - newWidth;
                    newPos.z = pos.z + newWidth;
                    break;
                case 5:
                    newPos.x = pos.x + newWidth;
                    newPos.y = pos.y - newWidth;
                    newPos.z = pos.z + newWidth;
                    break;
                case 6:
                    newPos.x = pos.x - newWidth;
                    newPos.y = pos.y + newWidth;
                    newPos.z = pos.z + newWidth;
                    break;
                case 7:
                    newPos.x = pos.x + newWidth;
                    newPos.y = pos.y + newWidth;
                    newPos.z = pos.z + newWidth;
                    break;
            }

            node.children[i] = Octree.Create(newPos, halfWidth / 2, depth - 1);
        }
        return node;
    }
}

/// <summary>
/// An octree that holds 8 children, all of which are Octree's.
/// </summary>
public class OctreeNode : Octree //represents one of the nodes on the tree that have children
{
    public Vector3 position;
    public Octree[] children;

    // TODO: YOUR CODE HERE

    //NOTE: used for passing the test on line 94 of movementTest.cs
    public OctreeNode()
    {
        children = new Octree[8];
        position = Vector3.zero;
    }

    /// <summary>
    /// Inserts the given particle into the appropriate children. The particle
    /// may need to be inserted into more than one child.
    /// </summary>
    /// <param name="sphere">The bounding sphere of the particle to insert.</param>
    public void Insert(Particle3D sphere)
    {
        //Determine where the sphere center would fit in the octree
        int index = 0;
        if (position.x < sphere.center.x) index += 1;
        if (position.y < sphere.center.y) index += 2;
        if (position.z < sphere.center.z) index += 4;

        //NOTE: Used for ensuring that the sphere is not inserted into the same index more than once
        HashSet<int> spheres = new HashSet<int>();

        //Detect in multiple zones
        Vector3 d = position - sphere.transform.position;

        //booleans used for comparisons  
        bool xCross = false, yCross = false, zCross = false;
        bool xNeg = true, yNeg = true, zNeg = true;

        int axis = 0; //used for a scenario where the sphere crosses all 3 axis

        //gets the initial index of where the center of the sphere falls under
        //checks x axis overlap
        if (Mathf.Abs(d.x) < sphere.radius)
        {
            //index +- 1;
            xCross = true;
            axis += 1;
        }

        //checks y axis overlap
        if (Mathf.Abs(d.y) < sphere.radius)
        {
            //index +- 2;
            yCross = true;
            axis += 1;
        }

        //checks z axis overlap
        if (Mathf.Abs(d.z) < sphere.radius)
        {
            //index +- 4;
            zCross = true;
            axis += 1;
        }

        //checks for if the sphere overlaps all the axis
        if (axis == 3)
        {
            foreach (var child in children) //adds it into all children
            {
                child.Insert(sphere);
            }

            return;
        }

        //adds the sphere into the correct child
        spheres.Add(index); //adds into the hashSet
        children[index].Insert(sphere);

        //gets the values from the table from the correct index
        switch (index)
        {
            case 0:
                xNeg = true;
                yNeg = true;
                zNeg = true;
                break;
            case 1:
                xNeg = false;
                yNeg = true;
                zNeg = true;
                break;
            case 2:
                xNeg = true;
                yNeg = false;
                zNeg = true;
                break;
            case 3:
                xNeg = false;
                yNeg = false;
                zNeg = true;
                break;
            case 4:
                xNeg = true;
                yNeg = true;
                zNeg = false;
                break;
            case 5:
                xNeg = false;
                yNeg = true;
                zNeg = false;
                break;
            case 6:
                xNeg = true;
                yNeg = false;
                zNeg = false;
                break;
            case 7:
                xNeg = false;
                yNeg = false;
                zNeg = false;
                break;
        }

        int oldIndex = index; //used for resetting the index back to its original value

        //Checks for crossing
        //xcross
        if (xCross && xNeg)
        {
            index += 1;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }

        }
        else if (xCross)
        {
            index -= 1;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }

        }

        index = oldIndex; //resets the index

        if (yCross && yNeg)
        {
            index = 2;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }
        }
        else if (yCross)
        {
            index -= 2;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }
        }

        index = oldIndex;

        if (zCross && zNeg)
        {
            index += 4;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }
        }
        else if (zCross)
        {
            index -= 4;
            if (!spheres.Contains(index))
            {
                children[index].Insert(sphere);
                spheres.Add(index);
            }
        }

        //Compare multiple

        int num = 0;

        if (!xNeg && xCross) num -= 1;
        if (xNeg && xCross) num += 1;

        if (!yNeg && yCross) num -= 2;
        if (yNeg && yCross) num += 2;

        if (!zNeg && zCross) num -= 4;
        if (zNeg && zCross) num += 4;

        if (num != 0)
        {
            if (!spheres.Contains(oldIndex + num))
            {
                children[oldIndex + num].Insert(sphere);
                spheres.Add(index);
            }
        }

    }

    /// <summary>
    /// Resolves collisions in all children, as only leaf nodes can hold particles.
    /// </summary>
    public void ResolveCollisions()
    {
        foreach (var child in children)
        {
            if (child.GetType() == typeof(OctreeObjects))
            {
                child.ResolveCollisions();
            }
        }
    }

    /// <summary>
    /// Removes all particles in each child.
    /// </summary>
    public void Clear()
    {
        int i;
        for (i = 0; i < 8; i++)
        {
            children[i].Clear(); //should recurse until it reaches the OctreeObjects, where it calls that implementation
        }
    }
}

/// <summary>
/// An octree that holds only particles.
/// </summary>
public class OctreeObjects : Octree //represents one of the leaf nodes
{
    private List<Particle3D> _spheresList = new List<Particle3D>();
    public ICollection<Particle3D> Objects
    {
        get
        {
            return _spheresList;
        }
    }

    // TODO: YOUR CODE HERE!

    /// <summary>
    /// Inserts the particle into this node. It will be compared with all other
    /// particles in this node in ResolveCollisions().
    /// </summary>
    /// <param name="particle">The particle to insert.</param>
    public void Insert(Particle3D particle)
    {
        _spheresList.Add(particle);

    }

    /// <summary>
    /// Calls CollisionDetection.ApplyCollisionResolution() on every pair of
    /// spheres in this node.
    /// </summary>
    public void ResolveCollisions()
    {
        for (int i = 0; i < _spheresList.Count; i++)
        {
            Particle3D s1 = _spheresList[i];
            for (int j = i + 1; j < _spheresList.Count; j++)
            {
                Particle3D s2 = _spheresList[j];
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER //TODO: DO THIS LATER
                //CollisionDetection.ApplyCollisionResolution(s1, s2);
            }
        }
    }

    /// <summary>
    /// Removes all objects from this node.
    /// </summary>
    public void Clear()
    {
        _spheresList.Clear();
    }
}

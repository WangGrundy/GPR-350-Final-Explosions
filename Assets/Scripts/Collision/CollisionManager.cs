using System.Collections;
using System.Collections.Generic;
using static CollisionDetection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CollisionManager : MonoBehaviour
{
    Octree tree;
    [HideInInspector] public GameObject[] allSphereObjects;

    public enum CollisionType
    {
        Standard,
        Octree
    }

    public static CollisionType collisionType = CollisionType.Octree;

    private void Start()
    {
        //creates octree
        tree = Octree.Create(new Vector3(0,0,0), 5f, 5);
    }

    private void TreeCollisionResolution()
    {
        // // Perform sphere-sphere collisions using the Octree
        tree.ResolveCollisions();

        PlaneCollider[] planes = FindObjectsOfType<PlaneCollider>();
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        
        foreach (PlaneCollider p in planes)
        {
            foreach (Sphere s in spheres)
            {
                ApplyCollisionResolution(s,p);
            }
        }
    }

    private void StandardCollisionResolution()
    {
        Sphere[] spheres = FindObjectsOfType<Sphere>();
        PlaneCollider[] planes = FindObjectsOfType<PlaneCollider>();
        for (int i = 0; i < spheres.Length; i++)
        {
            Sphere s1 = spheres[i];
            for (int j = i + 1; j < spheres.Length; j++)
            {
                Sphere s2 = spheres[j];
                ApplyCollisionResolution(s1, s2);
            }
            foreach (PlaneCollider plane in planes)
            {
                ApplyCollisionResolution(s1, plane);
            }
        }
    }

    private void FixedUpdate()
    {
         CollisionChecks = 0;
        
        // TODO: YOUR CODE HERE
        // Call correct collision resolution type based
        // on collisionType variable.
        
        if (collisionType == CollisionType.Standard)
        {
            StandardCollisionResolution();
        }
        else if (collisionType == CollisionType.Octree)
        {
            TreeCollisionResolution();
        }
    }

    private void Update()
    {
        // TODO: YOUR CODE HERE
        // Switch collision types if the "C" key is pressed.
        
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (collisionType == CollisionType.Standard)
            {
                collisionType = CollisionType.Octree;
            }
            else if (collisionType == CollisionType.Octree)
            {
                collisionType = CollisionType.Standard;
            }
        }
    }
}

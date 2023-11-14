using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    public Vector3 velocity {get; set;}
    public Vector3 acceleration {get; set;}
    public Vector3 accumulatedForces {get; private set;}
    public float damping;
    public Vector3 gravity;
    public float inverseMass;

    public void FixedUpdate()
    {
        DoFixedUpdate(Time.fixedDeltaTime);
    }

    private void DoFixedUpdate(float dt)
    {
        //I don't know if this version actually works, but we need to declare a var to itt over a collection
        var generators = GetComponents<ForceGenerator>();
        foreach (var forceGen in generators)
        {
            if (forceGen.enabled) forceGen.UpdateForce(this);
        }
        
        Integrator.Integrate(this, dt);
        ClearForces();
    }

    public void ClearForces()
    {
        accumulatedForces = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        accumulatedForces += force;
    }
    
}
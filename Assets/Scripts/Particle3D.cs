using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 position;
    public Vector3 accumulatedForces;
    public float damping;
    public Vector3 gravity;
    public float inverseMass;

    public void FixedUpdate()
    {
        DoFixedUpdate(Time.fixedDeltaTime);
    }

    private void DoFixedUpdate(float dt)
    {
        System.Array.ForEach(GetComponents<ForceGenerator>(),
         generator => { if (generator.enabled) generator.UpdateForce(this); });

        Integrator.Integrate(this, dt);
        this.ClearForces();
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
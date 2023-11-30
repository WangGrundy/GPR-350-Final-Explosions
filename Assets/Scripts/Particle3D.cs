using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    [HideInInspector] public Vector3 velocity; //{get; set;}
    [HideInInspector] public Vector3 acceleration;
    [HideInInspector] public Vector3 accumulatedForces;
    public float damping;
    public Vector3 gravity;
    public float inverseMass;
    public float multiplier = 500;
    private ParticleSpawner spawner;

    //Find position of spawner, find magnitude, add some velocity depending on that vector
    //children particle take velocity from parent
    //adjust the explosion force generator

    private void Awake()
    {
        spawner = GameObject.FindObjectOfType<ParticleSpawner>();
    }

    private void Start()
    {
        SetInitialVelocity();
    }

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

    //mulitplier, magnitude * velocity
    public void SetInitialVelocity()
    {
        Vector3 direction = transform.position - spawner.gameObject.transform.position;
        direction = direction.normalized;

        velocity = direction;
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
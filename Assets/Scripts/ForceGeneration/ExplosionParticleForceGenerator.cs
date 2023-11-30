using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosionParticleForceGenerator : ForceGenerator
{
    public Vector3 spawnerPosition;
    public float radius;
    private Particle3D particleScript;
    private ParticleSpawner spawnerScript;
    private float multiplier;
    private Vector3 force;

    private void Awake()
    {
        particleScript = GetComponent<Particle3D>();
        spawnerScript = GameObject.FindObjectOfType<ParticleSpawner>();
    }

    private void Start()
    {
        multiplier = particleScript.multiplier;
        spawnerPosition = spawnerScript.gameObject.transform.position;
    }

    public override void UpdateForce(Particle3D particle)
    {
        radius = (spawnerScript.gameObject.transform.position - particle.transform.position).magnitude;

        Vector3 directionVector = particle.velocity.normalized;
        // 1/r^2
        float magnitude = 1 / (radius * radius);
        force = magnitude * directionVector;

        //vector *= particle.damping; //??
        particle.AddForce(force);


        ///////////////////////////////////////////////////////////////////
        //Vector2 vector = targetPos - particle.transform.position;
        //float radiusSquared = vector.sqrMagnitude;
        //vector = vector.normalized;

        //float k = power / radiusSquared;
        //vector *= k;  

        ////vector *= particle.damping; //??
        //particle.AddForce(vector);

    }




}


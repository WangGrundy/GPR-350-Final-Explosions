using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosionParticleForceGenerator : ForceGenerator
{
    public Vector3 targetPos;
    public float radius = 200;
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
        targetPos = spawnerScript.gameObject.transform.position;
    }

    public override void UpdateForce(Particle3D particle)
    {

        // 1/r^2
        Vector3 newVelocity = particle.velocity.normalized;
        float k = 1 / (radius * radius);
        force = k * ;
        particle.AddForce(vectorce);


        // power/r^2 * vector
        Vector3 vector = targetPos - particle.transform.position;
        float radiusSquared = 1 / vector.sqrMagnitude;
        vector = vector.normalized; fo* particle,.


        //vector *= particle.damping; //??
        particle.AddForce(vectorce);


    }


}


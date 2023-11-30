using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExplosionParticleForceGenerator : ForceGenerator
{
    public Vector3 spawnerPosition;
    public float radius;
    private ParticleSpawner spawnerScript;
    private float multiplier;
    private Vector3 force;

    private void Awake()
    {
        spawnerScript = GameObject.FindObjectOfType<ParticleSpawner>();
    }

    private void Start()
    {
        spawnerPosition = spawnerScript.gameObject.transform.position;
    }

    public override void UpdateForce(Particle3D particle)
    {
        multiplier = particle.multiplier;
        radius = (spawnerScript.gameObject.transform.position - particle.transform.position).magnitude;
        Vector3 directionVector = particle.velocity.normalized;
        // 1/r^2
        float magnitude = multiplier / (radius * radius);
        force = magnitude * directionVector;
        particle.AddForce(force);
    }
}
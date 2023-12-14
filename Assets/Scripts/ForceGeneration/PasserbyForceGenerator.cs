using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PasserbyForceGenerator : ForceGenerator
{
    private Octree tree;
    public float multiplier;
    private float radius;
    private ParticleSpawner spawnerScript;
    Vector3 force;
    public Sphere sphere;
    [SerializeField] private GameObject originalSpawner;
    private CollisionManager ogCollisionManagerScript;
    public bool doExplosion = false;

    private void Start()
    {
        //spawnerScript = FindObjectOfType<ParticleSpawner>();
        spawnerScript = originalSpawner.GetComponent<ParticleSpawner>();
        ogCollisionManagerScript = originalSpawner.GetComponent<CollisionManager>();
        tree = ogCollisionManagerScript.tree;
        tree.Insert(sphere);

    }

    public override void UpdateForce(Particle3D particle)
    {
        force = Vector3.zero;
        if (doExplosion)
        {
            Debug.Log("Moving Passerby");
            multiplier = particle.multiplier;
            radius = (spawnerScript.gameObject.transform.position - particle.transform.position).magnitude;
            Vector3 directionVector = particle.velocity.normalized;
            // 1/r^2
            float magnitude = multiplier / (radius * radius);
            force = magnitude * directionVector;
        }
        particle.AddForce(force);
    }
}

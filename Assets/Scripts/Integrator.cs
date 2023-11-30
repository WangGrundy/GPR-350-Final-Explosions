using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    public static void Integrate(Particle3D particle, float dt)
    {
        //Add delta velocity to position
        particle.transform.position += new Vector3(particle.velocity.x * dt, particle.velocity.y * dt, particle.velocity.z * dt);

        //Find Acceleration:  f/m = a
        particle.acceleration = particle.inverseMass * particle.accumulatedForces;
        particle.acceleration += particle.gravity;
        
        //add velocity to the particle
        particle.velocity += particle.acceleration * dt;
        particle.velocity *= Mathf.Pow(particle.damping, dt);
    }
}
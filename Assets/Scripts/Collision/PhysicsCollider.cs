using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollider : MonoBehaviour
{
    public float invMass
    {
        get
        {
            {
                Particle3D particle;
                if (TryGetComponent(out particle))
                {
                    return particle.inverseMass;
                }
            }
            return 0;
        }
        set
        {
            {
                Particle3D particle;
                if (TryGetComponent(out particle))
                {
                    particle.inverseMass = value;
                }
            }
        }
    }

    public Vector3 velocity
    {
        get
        {
            {
                Particle3D particle;
                if (TryGetComponent(out particle))
                {
                    return particle.velocity;
                }
            }
            return Vector3.zero;
        }
        set
        {
            {
                Particle3D particle;
                if (TryGetComponent(out particle))
                {
                    particle.velocity = value;
                }
            }
        }
    }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
        set
        {
            {
                Particle3D particle;
                if (TryGetComponent(out particle))
                {
                    particle.transform.position = value;
                }
            }
        }
    }
}

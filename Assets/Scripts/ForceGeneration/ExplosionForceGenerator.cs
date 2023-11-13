using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class ExplosionForceGenerator : ForceGenerator
{
    public Vector3 targetPos;
    public float power;

    public override void UpdateForce(Particle3D particle)
    {
        // TODO: YOUR CODE HERE
        // power/r^2 * vector
        
        Vector2 vector =  targetPos - particle.transform.position;
        float radiusSquared = vector.sqrMagnitude;
        vector = vector.normalized;
        
        float k = power / radiusSquared;
        vector *= k ;
        
        //vector *= particle.damping; //??
        particle.AddForce(vector); 
    }
}
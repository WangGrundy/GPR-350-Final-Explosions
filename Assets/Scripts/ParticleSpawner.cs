using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private float radiusOfSpawnage;
    
    // Using Gaussian distribution for all three coordinates of your point will
    // ensure an uniform distribution on the surface of the sphere. You should proceed as follows
    
    // 1. Generate three random numbers x,y,z
    
    // 2. using Gaussian distribution
    // Multiply each number by 1/x^2+y^2+z^2−−−−−−−−−−√ (a.k.a. Normalise)
    
    // 2.5. You should handle what happens if x=y=z=0
    // Multiply each number by the radius of your sphere.
    
    // 3. Multiply each number by the radius of your sphere.
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

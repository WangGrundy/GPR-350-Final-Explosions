using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private float radiusOfSpawnage;
    [SerializeField] private int particleNumber;
    [SerializeField] private GameObject[] particles;
    [SerializeField] private GameObject particlePrefab;

    private void Start()
    {
        spawnParticles();
    }

    /// <summary>
    /// Spawn a bunch of particles depending on particle number and particle spawner position
    /// </summary>
    public void spawnParticles()
    {
        Vector3 position = Vector3.zero;
        particles = new GameObject[particleNumber];

        for(int i = 0; i < particleNumber; i++)
        {
            // + difference from particle spawner
            position = ReturnRandomPositionOnSurface() + transform.position;
            particles[i] = Instantiate(particlePrefab);
            particles[i].transform.position = position;
        }
    }

    /// <summary>
    /// This will get a random position within a radius
    /// </summary>
    /// <returns></returns>
    public Vector3 ReturnRandomPosition()
    {
        // 1. Generate three random numbers x,y,z
        Vector3 randomNum;

        randomNum.x = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.y = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.z = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);

        // 2a. You should handle what happens if x=y=z=0
        if (randomNum.x == 0 && randomNum.y == 0 && randomNum.z == 0)
        {
            ReturnRandomPosition();
        }

        return randomNum;
    }

    /// <summary>
    /// Using Gaussian distribution for all three coordinates of your point will
    /// ensure an uniform distribution on the surface of the sphere. You should proceed as follows
    /// https://math.stackexchange.com/questions/1585975/how-to-generate-random-points-on-a-sphere
    /// </summary>
    /// <returns></returns>
    public Vector3 ReturnRandomPositionOnSurface()
    {
        // 1. Generate three random numbers x,y,z
        Vector3 randomNum;

        randomNum.x = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.y = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.z = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);

        // 2a. You should handle what happens if x=y=z=0
        if (randomNum.x == 0 && randomNum.y == 0 && randomNum.z == 0)
        {
            return randomNum;
        }

        // 2b. using Gaussian distribution
        // Multiply each number by 1/x^2+y^2+z^2−−−−−−−−−−√ (a.k.a. Normalise)
        randomNum.x = 1 / randomNum.x;
        randomNum.y = 1 / randomNum.y;
        randomNum.z = 1 / randomNum.z;
        randomNum = randomNum.normalized;

        // 3. Multiply each number by the radius of your sphere.
        randomNum *= radiusOfSpawnage;
        return randomNum;
    }

    public Vector3 ReturnRandomPositionOnAsymtope()
    {
        // 1. Generate three random numbers x,y,z
        Vector3 randomNum;

        randomNum.x = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.y = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);
        randomNum.z = Random.Range(-radiusOfSpawnage, radiusOfSpawnage);

        // 2a. You should handle what happens if x=y=z=0
        if (randomNum.x == 0 && randomNum.y == 0 && randomNum.z == 0)
        {
            return randomNum;
        }

        // 2b. using Gaussian distribution
        // Multiply each number by 1/x^2+y^2+z^2−−−−−−−−−−√ (a.k.a. Normalise)
        randomNum = randomNum.normalized;
        randomNum.x = 1 / randomNum.x;
        randomNum.y = 1 / randomNum.y;
        randomNum.z = 1 / randomNum.z;

        // 3. Multiply each number by the radius of your sphere.
        randomNum *= radiusOfSpawnage;
        return randomNum;
    }

}

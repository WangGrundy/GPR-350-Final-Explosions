using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : PhysicsCollider
{

    [SerializeField] private float delay;

    public Material newMaterial;
    public Vector3 Center => transform.position;
    public float Radius = .5f;
    private Renderer renderer;
    public bool destroyOnCollision = false;

    private bool collided = false;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void changeTexture()
    {
        if(newMaterial != renderer.material)
        {
            renderer.material = newMaterial;
            collided = true;
        }

        if (collided)
        {
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}

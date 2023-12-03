using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : PhysicsCollider
{
    public Vector3 Origin => transform.position;
    public Vector3 Normal => transform.up;
    public float Offset => Vector3.Dot(Origin, Normal);
}

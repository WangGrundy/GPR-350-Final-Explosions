using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionDetection
{
    public static int CollisionChecks;

    public struct VectorDeltas
    {
        public Vector3 s1;
        public Vector3 s2;
        public static VectorDeltas zero
        {
            get
            {
                return new VectorDeltas { s1 = Vector3.zero, s2 = Vector3.zero };
            }
        }

        public void ApplyToPosition(PhysicsCollider s1, PhysicsCollider s2)
        {
            s1.position += this.s1;
            s2.position += this.s2;
        }

        public void ApplyToVelocity(PhysicsCollider s1, PhysicsCollider s2)
        {
            s1.velocity += this.s1;
            s2.velocity += this.s2;
        }
    };

    public class CollisionInfo
    {
        public Vector3 normal = Vector3.zero;
        public float penetration = 0;
        public float pctToMoveS1 = 0;
        public float pctToMoveS2 = 0;
        public float separatingVelocity = 0;
        public bool IsColliding => penetration > 0;
        public bool HasInfiniteMass => pctToMoveS1 + pctToMoveS2 == 0;
    }

    public static void GetNormalAndPenetration(Sphere s1, Sphere s2, out Vector3 normal, out float penetration)
    {
        Vector3 s2ToS1 = s1.Center - s2.Center;
        float dist = s2ToS1.magnitude;
        float sumOfRadii = (s1.Radius + s2.Radius);
        penetration = sumOfRadii - dist;
        normal = (s2ToS1 / dist);
    }

    public static void GetNormalAndPenetration(Sphere s, PlaneCollider p, out Vector3 normal, out float penetration)
    {
        float offset = Vector3.Dot(s.Center, p.Normal) - p.Offset;
        float dist = Mathf.Abs(offset);
        penetration = s.Radius - dist;
        normal = offset >= 0 ? p.Normal : -p.Normal;
        
    }

    private static void GetGenericCollisionInfo(PhysicsCollider c1, PhysicsCollider c2, ref CollisionInfo info)
    {
        float sumOfInvMasses = c1.invMass + c2.invMass;
        if (sumOfInvMasses == 0) return; // Both masses infinite, avoid divide-by-zero error
        info.pctToMoveS1 = c1.invMass / sumOfInvMasses;
        info.pctToMoveS2 = c2.invMass / sumOfInvMasses;

        info.separatingVelocity = Vector3.Dot(c1.velocity - c2.velocity, info.normal);
    }

    public static CollisionInfo GetCollisionInfo(Sphere s1, Sphere s2)
    {
        CollisionInfo info = new CollisionInfo();
        GetNormalAndPenetration(s1, s2, out info.normal, out info.penetration);

        GetGenericCollisionInfo(s1, s2, ref info);

        return info;
    }

    public static CollisionInfo GetCollisionInfo(Sphere s, PlaneCollider p)
    {
        CollisionInfo info = new CollisionInfo();
        GetNormalAndPenetration(s, p, out info.normal, out info.penetration);

        GetGenericCollisionInfo(s, p, ref info);

        return info;
    }

    public static void ApplyCollisionResolution(Sphere s1, Sphere s2)
    {
        CollisionChecks++;
        CollisionInfo info = GetCollisionInfo(s1, s2);

        ApplyCollisionResolution(s1, s2, info);
    }

    public static void ApplyCollisionResolution(Sphere s, PlaneCollider p)
    {
        CollisionChecks++;
        CollisionInfo info = GetCollisionInfo(s, p);

        ApplyCollisionResolution(s, p, info);
    }

    private static void ApplyCollisionResolution(PhysicsCollider c1, PhysicsCollider c2, CollisionInfo info)
    {
        VectorDeltas delPos = ResolvePosition(info);
        VectorDeltas delVel = ResolveVelocity(info);

        delPos.ApplyToPosition(c1, c2);
        delVel.ApplyToVelocity(c1, c2);
    }

    public static VectorDeltas ResolvePosition(CollisionInfo info)
    {
        if (!info.IsColliding) return VectorDeltas.zero;
        if (info.HasInfiniteMass) return VectorDeltas.zero;

        return new VectorDeltas
        {
            s1 = info.pctToMoveS1 * info.normal * info.penetration,
            s2 = info.pctToMoveS2 * -info.normal * info.penetration
        };
    }

    public static VectorDeltas ResolveVelocity(CollisionInfo info)
    {
        if (!info.IsColliding) return VectorDeltas.zero;
        if (info.HasInfiniteMass) return VectorDeltas.zero;
        float restitution = 1;

        float separatingVelocity = info.separatingVelocity;
        if (separatingVelocity >= 0) return VectorDeltas.zero;
        float newSeparatingVelocity = -separatingVelocity * restitution;
        float deltaVelocity = newSeparatingVelocity - separatingVelocity;

        return new VectorDeltas
        {
            s1 = deltaVelocity * info.pctToMoveS1 * info.normal,
            s2 = deltaVelocity * info.pctToMoveS2 * -info.normal
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class StaticMaths
{

    public static Vector3 GetPos(Vector3 origin, float radius)
    {
        Vector3 s = origin + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        if (NavMesh.SamplePosition(s, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            s = hit.position;
        }

        return s;
    }

    public static float GetAngle(Vector3 targetPos, Vector3 currentPos, Vector3 currentForward)
    {
        Vector3 dir = GetDirection(targetPos, currentPos);
        dir.y = 0;
        return Vector3.Angle(currentForward, dir);
    }

    public static Vector3 GetDirection(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 directionToLookTo = targetPos - currentPos;
        directionToLookTo.y = 0;

        return directionToLookTo;
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static Quaternion GetLookRotation(Vector3 targetPos, Vector3 currentPos, Vector3 currentForward, out bool shouldRotate, float threshold = 0.1f)
    {
        shouldRotate = false;
        Vector3 dir = GetDirection(targetPos, currentPos);
        float angle = Vector3.Angle(currentForward, dir);
        if (angle > threshold)
        {
            shouldRotate = true;
            if (dir == Vector3.zero) dir = currentForward;
            return Quaternion.LookRotation(dir);
        }

        return Quaternion.identity;
    }

    public static string ProcessObjectName(string name)
    {
        if (name.Length > 7)
            name = name.Remove(name.Length - 7);
        return name;
    }
    public static float CalculatePercent(float percentAmount, float outOf)
    {
        return CalculateNormalisedPercent(percentAmount, outOf) * 100;
    }

    public static float CalculateNormalisedPercent(float percentAmount, float outOf)
    {
        return (percentAmount / outOf);
    }

    public static Vector3 RandomVector(float range)
    {
        return new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
    }

    public static Transform ZeroOutLocal(Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.Euler(Vector3.zero);
        return t;
    }
    /// <summary>
    /// Determine the signed angle between two vectors, with normal 'n'
    /// as the rotation axis.
    /// </summary>
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static void AddExplosiveForce(Transform item, float minDamage, float maxDamage, Vector3 origin, float radius)
    {
        IHealth h = item.GetComponent<IHealth>();
        float damageDiff = maxDamage - minDamage;
        if (h != null)
        {
            float damageAmount = minDamage + (damageDiff / Vector3.Distance(origin, item.transform.position));
            damageAmount += damageAmount * 0.5f;
            h.DamageHealth(damageAmount);
        }
        else
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb && rb.gameObject.layer != 9)
            {
                rb.AddExplosionForce(5, origin, radius, 1.5f);
            }
        }
    }
}

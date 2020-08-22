using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipRayDetection : MonoBehaviour
{
    Vector3 overrideEscape;
    Vector3 target;
    [SerializeField] float stoppingDistance = 15;
    float curStoppingDist = 15;
    [SerializeField] float escapeDistance = 40;
    [SerializeField] bool debug;
    [SerializeField] EscapeRay[] raySpawners = null;
    [SerializeField] LayerMask mask = ~(1<<10);


    public Vector3 GenerateRandom()
    {
        if(CanSetNewTarget())
        {
            target = StaticMaths.RandomVector(150 + curStoppingDist);
        }
        return target;
    }

    bool CanSetNewTarget()
    {
        if (target != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, target) < curStoppingDist)
            {
                target = Vector3.zero;
            }
        }
        return target == Vector3.zero;
    }

    public Vector3 SetNewTarget(Vector3 target)
    {
        if (CanSetNewTarget())
        {
            this.target = target;
        }
        return this.target;
    }

    private bool ObstacleHitRay(Transform trans, float length)
    {
        if (debug)
            Debug.DrawRay(trans.position, trans.forward * length);
        if (Physics.Raycast(trans.position, trans.forward, length, mask))
        {
            return true;
        }
        return false;
    }

    public Vector3 CheckHits(out bool hit)
    {
        bool mustEscape = false;
        bool escapeUp = true;
        bool escapeRight = true;
        bool escapeLeft = true;
        bool escapeDown = true;
        for (int i = 0; i < raySpawners.Length; i++)
        {
            if (ObstacleHitRay(raySpawners[i].pos, raySpawners[i].rayLength))
            {
                if (raySpawners[i].direction == Vector3.up)
                {
                    escapeUp = false;
                }
                else if (raySpawners[i].direction == Vector3.right)
                {
                    escapeRight = false;
                }
                else if (raySpawners[i].direction == Vector3.left)
                {
                    escapeLeft = false;
                }
                else if (raySpawners[i].direction == Vector3.down)
                {
                    escapeDown = false;
                }
                else
                {
                    mustEscape = true;
                }
            }
        }

        hit = mustEscape;

        if (mustEscape == false)
        {
            if (Vector3.Distance(transform.position, overrideEscape) < curStoppingDist)
            {
                overrideEscape = Vector3.zero;
                target = Vector3.zero;
            }
            return overrideEscape;
        }

        if (escapeUp)
        {
            overrideEscape = transform.position + transform.up * escapeDistance + transform.forward * 2;
        }
        else if (escapeRight)
        {
            overrideEscape = transform.position + transform.right * escapeDistance;
        }
        else if (escapeLeft)
        {
            overrideEscape = transform.position - transform.right * escapeDistance;
        }
        else if (escapeDown)
        {
            overrideEscape = transform.position - transform.up * escapeDistance + transform.forward * 2;
        }
        target = overrideEscape;
        return overrideEscape;
    }

    public void SetStoppingDist(float curSpeed)
    {
        float dist = stoppingDistance * (curSpeed / 10);
        if (dist < 1) dist = 1;
        curStoppingDist = dist;
    }

}
[System.Serializable]
public class EscapeRay
{
    public Transform pos;
    public Vector3 direction;
    [Range(1, 30)] public float rayLength = 1;
}

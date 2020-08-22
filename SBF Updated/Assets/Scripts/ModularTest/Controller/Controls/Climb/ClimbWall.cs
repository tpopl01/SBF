using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class ClimbWall : LerpCurveOverTimeBase, IClimb, ITick, ISetup, IGetDisableIK

{
    [SerializeField] float vaultOverHeight = 2f;
    [SerializeField] float rayDist = 2;
    [SerializeField] bool debug = false;

    public void SetUp(Transform root)
    {
        Init(root, new string[] { "climb_up_high" }, Resources.Load<CurveHolder>("Curves/climb_curve"));
    }

    public void BeginClimb()
    {
        if (!CanStart())
        {
            return;
        }
        Vector3 rayPos = col.transform.position + Vector3.up * 0.4f;
        if (Physics.Raycast(rayPos, col.transform.forward, rayDist, (1<<8)))
        {
            if (!Physics.Raycast(rayPos + Vector3.up * vaultOverHeight, col.transform.forward, rayDist, ~(1 << 10)))
            {
                if (Physics.Raycast(rayPos + Vector3.up * vaultOverHeight + col.transform.forward, -col.transform.up, out RaycastHit hit, rayDist * 1.5f, (1 << 8)))
                {
                    SetSpeedModifier(0.7f * (hit.point.y - col.transform.position.y));
                    Begin(hit.point);
                }
            }
        }
    }


    public void Tick()
    {
        if (!inProgress) return;
        if (debug)
        {
            Vector3 rayPos = col.transform.position + Vector3.up * 0.4f;
            Debug.DrawRay(rayPos, col.transform.forward);
            Debug.DrawRay(rayPos + Vector3.up * vaultOverHeight, col.transform.forward);
            Debug.DrawRay(rayPos + Vector3.up * vaultOverHeight + col.transform.forward, -col.transform.up);
        }

        OnTick();
    }

    public bool GetClimbing()
    {
        return inProgress;
    }

    public bool GetDisableIK()
    {
        return GetClimbing();
    }
}

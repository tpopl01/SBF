using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class Vaulting : LerpCurveOverTimeBase, IVault, ISetup, ITick, IGetDisableIK
{
    [SerializeField] float vaultOverHeight = 0.6f;
    [SerializeField] float rayDist = 2;
    [SerializeField] bool debug = false;

    public void BeginVault()
    {
        if(!CanStart())
        {
            return;
        }
        Vector3 rayPos = col.transform.position + Vector3.up * 0.4f;
        if (debug)
        {
            Debug.DrawRay(rayPos, col.transform.forward);
            Debug.DrawRay(rayPos + Vector3.up * vaultOverHeight, col.transform.forward);
        }

        if (Physics.Raycast(rayPos, col.transform.forward, rayDist, (1 << 8)))
        {
            if (!Physics.Raycast(rayPos + Vector3.up * vaultOverHeight, col.transform.forward, rayDist, ~(1 << 10)))
            {
                SetSpeedModifier(5);
                Begin(col.transform.position + col.transform.forward * 3);
            }
        }
    }

    public bool GetDisableIK()
    {
        return GetVaulting();
    }

    public bool GetVaulting()
    {
        return inProgress;
    }

    public void SetUp(Transform root)
    {
        Init(root, new string[] { "vault_over_walk_1", "vault over walk 2", "vault_over_run" }, Resources.Load<CurveHolder>("Curves/vault_curve"));
    }

    public void Tick()
    {
        OnTick();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClimbWallAI : ClimbWall
{
    NavMeshAgent agent;

    protected override void Init(Transform root, string[] animations, CurveHolder curve, float alterPosOnCurve = 0)
    {
        base.Init(root, animations, curve);
        agent = root.GetComponentInChildren<NavMeshAgent>();
    }

    protected override void Begin(Vector3 targetPos)
    {
        agent.enabled = false;
        base.Begin(targetPos);
    }

    protected override void Stop()
    {
        base.Stop();
        agent.enabled = true;
        inProgress = false;
    }
}

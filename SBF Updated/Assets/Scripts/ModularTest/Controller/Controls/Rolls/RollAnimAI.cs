using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollAnimAI : RollAnim
{
    NavMeshAgent agent;
    protected override void Init(Transform root)
    {
        base.Init(root);
        agent = root.GetComponentInChildren<NavMeshAgent>();
    }

    protected override void Begin()
    {
        agent.enabled = false;
        base.Begin();
    }

    protected override void RollFinished()
    {
        base.RollFinished();
        agent.enabled = true;
    }
}

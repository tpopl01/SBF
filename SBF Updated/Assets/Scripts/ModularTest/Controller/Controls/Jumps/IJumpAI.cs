using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IJumpAI : Jump, ISetup
{
    NavMeshAgent agent;
    public override void SetUp(Transform root)
    {
        agent = root.GetComponentInChildren<NavMeshAgent>();
    }

    public override void Begin()
    {
        base.Begin();
        agent.enabled = false;
    }

    protected override void Complete()
    {
        base.Complete();
        agent.enabled = true;
    }

    
}

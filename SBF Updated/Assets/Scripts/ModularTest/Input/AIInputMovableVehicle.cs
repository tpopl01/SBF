using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIInputMovableVehicle : InputBase, ISetup
{
    NavMeshAgent agent;
    public override void Execute(ModularController controller)
    {
        ModularControllerMoveable c = (ModularControllerMoveable)controller;
        MoveToRandomTarget(agent, agent.transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), this);
        if (c.OnGround)
            c.Move(agent.velocity, controller.AIStats().GetRunSpeed());
    }

    void MoveToRandomTarget(NavMeshAgent agent, Vector3 targetPos, InputBase i)
    {
        i.Sprint = false;
        agent.updateRotation = true;
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            MoveToTarget(agent, hit.position);
        }
        else
        {
            Debug.Log("No area found");
        }
    }

    public bool MoveToTarget(NavMeshAgent agent, Vector3 targetPos, float stoppingDist = 8f)
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            if (agent.remainingDistance < agent.stoppingDistance + 0.2f || agent.isStopped || ((agent.isPathStale || !agent.hasPath) && !agent.pathPending))
            {
                agent.stoppingDistance = stoppingDist;
                agent.isStopped = false;
                agent.SetDestination(targetPos);
                return true;
            }
        }
        return false;
    }

    public void SetUp(Transform root)
    {
        agent = root.GetComponentInChildren<NavMeshAgent>();
    }
}

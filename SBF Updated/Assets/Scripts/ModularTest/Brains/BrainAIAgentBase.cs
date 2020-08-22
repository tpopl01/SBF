using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BrainAIAgentBase : BrainAIBase
{
    //public override void Execute(ModularController controller)
    //{

    //}

    protected virtual bool MoveToRandomTarget(ModularController c, NavMeshAgent agent, float range)
    {
        Vector3 targetPos = c.Position + Vector3.right * Random.Range(-range, range) + Vector3.forward * Random.Range(-range,range);
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            agent.updateRotation = true;
            c.input.Sprint = false;
            return MoveToTarget(agent, hit.position);
        }
        else
        {
            Debug.Log("No area found");
        }
        return false;
    }

    protected virtual void Attack(ModularController controller, ModularController targetAgent, NavMeshAgent a, float shootAngle = 10)
    {
     //   if (LookAtTarget(a, controller, targetAgent.Position, shootAngle))
     //   {
     
            Stop(a);
            controller.input.Aim = true;
            base.Attack(controller, targetAgent);
      //  }
    }

    protected virtual bool FleeFrom(ModularControllerMoveable c, NavMeshAgent a, Vector3 fleeFromPos)
    {
        Vector3 dir = (fleeFromPos - c.Position).normalized;
        c.Senses.TargetPos = SampleArea(c.Position - (dir * 30));
        if( MoveToTarget(a, c.Senses.TargetPos))
        {
            c.input.Sprint = true;
            a.updateRotation = true;
            return true;
        }
        return false;
    }

    protected virtual bool Approach(NavMeshAgent a, ModularController c)
    {
        if (MoveToTarget(a, c.Senses.TargetPos))
        {
            c.input.Sprint = true;
            a.updateRotation = true;
            return true;
        }
        return false;
    }

    protected virtual void Stop(NavMeshAgent a)
    {
        if (a.isActiveAndEnabled && a.isOnNavMesh)
            a.isStopped = true;
    }

    protected virtual bool Search(NavMeshAgent a, Vector3 targetPos, float range, InputBase i)
    {
        i.Sprint = false;
        a.updateRotation = true;
        targetPos += StaticMaths.RandomVector(range);
        targetPos = SampleArea(targetPos);
        return MoveToTarget(a, targetPos);
    }

    bool LookAtTarget(NavMeshAgent a, ModularController curTrans, Vector3 target, float angle = 10)
    {
        a.updateRotation = false;
        Quaternion t = StaticMaths.GetLookRotation(target, curTrans.Position, curTrans.transform.forward, out bool rotate, angle);
        if (rotate)
        {
            curTrans.transform.rotation = Quaternion.Slerp(curTrans.transform.rotation, t, Time.deltaTime * curTrans.AIStats().GetTurnSpeed() * 3);
            return false;
        }
        return true;
    }

    Vector3 SampleArea(Vector3 target)
    {
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return target;
    }

    //move to senses
    protected bool MoveToTarget(NavMeshAgent agent, Vector3 targetPos, float stoppingDist = 0.8f)
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
}

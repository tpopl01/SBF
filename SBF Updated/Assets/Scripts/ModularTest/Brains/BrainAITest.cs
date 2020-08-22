using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu
  (
      fileName = "Brain_Test",
      menuName = "Modular/Components/Brain/Test"
  )]
public class BrainAITest : BrainAIUnitBase
{
    //public override void Execute(ModularController controller)
    //{
    //    ModularControllerAI c = (ModularControllerAI)controller;

    //    BT(c.Agent, (Senses)c.Senses, c, controller.input, (IWeaponSystomChangeable)c.weaponSystem, c.weaponSystem);
    //}

    protected override void BehaviourTree(ModularControllerAI c)
    {
        BT(c.Agent, (Senses)c.Senses, c, (IWeaponSystomChangeable)c.weaponSystem);
    }

    void BT(NavMeshAgent agent, Senses ai, ModularControllerAI c, IWeaponSystomChangeable w)
    {
        if (ai.ClosestEnemy != null)
        {
            if (w.GetAmmoRemainingInGunPercent() == 0 && w.HasAmmo())
            {
                w.Reload();
            }
            else if (w.HasAmmo() == false)
            {
                if (!TryGetNearestAmmo(c))
                {
                    if (!TryPickupWeapon(c))
                    {
                        FleeFrom(c, agent, ai.ClosestEnemy.Position);
                    }
                } 
            }
            else if(UseItemOfType(c, SpecialType.Combat))
            {

            }
            else if (ai.InRange(ai.ClosestEnemy.Position))
            {
                Attack(c, ai.ClosestEnemy, agent);
            }
            else if (ai.NormalisedHealth() < 0.3f)
            {
                if (!TryGetNearestHealth(c))
                {
                    FleeFrom(c, agent, ai.ClosestEnemy.Position);
                }
            }
            else MoveToTarget(agent, ai.ClosestEnemy.transform.position);
        }
        else
        {
            if(HandleLeaderCommands(c))
            {

            }
            else if (ai.ShotAtFrom != Vector3.zero)
            {
                FleeFrom(c, agent, ai.ShotAtFrom);
            }
            else if (UseItemOfType(c, SpecialType.Repair))
            {

            }
            else if(TryEnterVehicle(c))
            {
                return;
            }
            else if (w.HasAmmo() == false && TryGetNearestAmmo(c))
            {
                return;
            }
            else if (ai.NormalisedHealth() < 0.8f && UseItemOfType(c, SpecialType.Heal))
            {
                return;
            }
            else if (ai.NormalisedHealth() < 0.8f && TryGetNearestHealth(c))
            {
                return;
            }
            else if (w.GetAmmoRemainingInGunPercent() < 0.5f)
            {
                w.Reload();
            }
            else if(TryGetNearestNeutralGoal(c))
            {
                return;
            }
            else if (TryGetNearestHostileGoal(c))
            {
                return;
            }
            else if(UseHealableOnAllies(c))
            {

            }
            else
            {
                MoveToRandomTarget(c, agent, 10);
            }
        }
    }

    
    
    //void MoveToRandomTarget(NavMeshAgent agent, Vector3 targetPos, InputBase i)
    //{
    //    i.Sprint = false;
    //    agent.updateRotation = true;
    //    if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10, NavMesh.AllAreas))
    //    {
    //        MoveToTarget(agent, hit.position);
    //    }
    //    else
    //    {
    //        Debug.Log("No area found");
    //    }
    //}

    //void FleeFrom(NavMeshAgent a, Vector3 curPos, Vector3 target, InputBase i)
    //{
    //    i.Sprint = true;
    //    a.updateRotation = true;
    //    Vector3 dir = (target - curPos).normalized;
    //    target = SampleArea(curPos - (dir * 30));
    //    MoveToTarget(a, target);
    //}

    //void Stop(NavMeshAgent a, Vector3 curPos, ModularController target)
    //{
    //    if(a.isActiveAndEnabled && a.isOnNavMesh)
    //        a.isStopped = true;
    //}

    //public bool Search(NavMeshAgent a, Vector3 targetPos, float range, InputBase i)
    //{
    //    i.Sprint = false;
    //    a.updateRotation = true;
    //    targetPos += StaticMaths.RandomVector(range);
    //    targetPos = SampleArea(targetPos);
    //    return MoveToTarget(a, targetPos);
    //}

    //private void LookAtTarget(NavMeshAgent a, Transform curTrans, Vector3 target)
    //{
    //    a.updateRotation = false;
    //    Quaternion t = StaticMaths.GetLookRotation(target, curTrans.position, curTrans.forward, out bool rotate);
    //    if (rotate)
    //    {
    //        curTrans.rotation = Quaternion.Slerp(curTrans.rotation, t, Time.deltaTime * 8);
    //    }
    //}

    //private Vector3 SampleArea(Vector3 target)
    //{
    //    if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10, NavMesh.AllAreas))
    //    {
    //        return hit.position;
    //    }
    //    return target;
    //}

    ////move to senses
    //public bool MoveToTarget(NavMeshAgent agent, Vector3 targetPos, float stoppingDist = 0.8f)
    //{
    //    if (agent.isActiveAndEnabled && agent.isOnNavMesh)
    //    {
    //        if (agent.remainingDistance < agent.stoppingDistance + 0.2f || agent.isStopped || ((agent.isPathStale || !agent.hasPath) && !agent.pathPending))
    //        {
    //            agent.stoppingDistance = stoppingDist;
    //            agent.isStopped = false;
    //            agent.SetDestination(targetPos);
    //            return true;
    //        }
    //    }
    //    return false;
    //}



}

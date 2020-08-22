using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public abstract class BrainAIUnitBase : BrainAIAgentBase
{
    Timer timer = new Timer(0.3f);
    public override void Execute(ModularController controller)
    {
        ModularControllerAI c = (ModularControllerAI)controller;
        
        c.input.Aim = false;
        if (HandleObstacles(c))
        {
            c.input.Speed = c.AIStats().GetRunSpeed();
            BehaviourTree(c);
            if (c.input.Sprint) c.input.Speed = c.AIStats().GetSprintSpeed();
            if (c.Aiming || c.input.Crouch) c.input.Speed = c.AIStats().GetWalkSpeed();
            if (c.OnGround && !c.grounded.Disable && !c.GetJumping())
                c.Move(c.input.MoveAxis, c.input.Speed);
        }
    }

    protected abstract void BehaviourTree(ModularControllerAI c);

    bool CheckObstacle(Transform root, float height = 0.5f, float rayLength = 1.3f)
    {
        Vector3 origin = root.position + Vector3.up * height;
        if (Physics.Raycast(origin, root.forward, rayLength, ~(1 << 10)))
        {
            return true;
        }
        return false;
    }

    //check if there is a gap infront of them and then attempt to jump across
    bool CheckCanJump(Transform root)
    {
        Vector3 origin = root.position + root.forward * 2 + Vector3.up * 2;
        if (Physics.Raycast(origin, Vector3.down, 7))
        {
            return false;
        }
        return true;
    }

    bool HandleObstacles(ModularControllerAI c)
    {
        if (c.Agent.isOnNavMesh && c.Agent.isStopped) return true;

        bool obstacleForward = CheckObstacle(c.transform);

        if (c.iVault.GetVaulting())
            return false;
        for (int i = 0; i < c.iJump.Length; i++)
        {
            if (c.iJump[i].GetJumping())
            {
                return false;
            }
        }
        if (c.iClimb.GetClimbing())
            return false;

        if (obstacleForward)
        {
            c.iClimb.BeginClimb();
            if (c.iClimb.GetClimbing())
                return false;
            c.iVault.BeginVault();
            if (c.iVault.GetVaulting())
                return false;
            //for (int i = 0; i < c.iJump.Length; i++)
            //{
            //    c.iJump[i].BeginJump(c.transform, c.OnGround);
            //}
            //if (!c.GetJumping())
            //    c.input.MoveAxis = Vector3.zero;
        }
        else if (c.input.MoveAxis != Vector3.zero && /*((AIInput)c.input).hashedPos == c.Position &&*/ (CheckObstacle(c.transform, 0.3f, 0.5f))) //step
        {
            for (int i = 0; i < c.iJump.Length; i++)
            {
                c.iJump[i].BeginJump(c.transform, c.OnGround);
                if (c.iJump[i].GetJumping()) break;
            }

            if (!c.GetJumping())
                c.input.MoveAxis = Vector3.zero;
        }
        else if (CheckCanJump(c.transform) && c.input.MoveAxis != Vector3.zero)
        {
            for (int i = 0; i < c.iJump.Length; i++)
            {
                c.iJump[i].BeginJump(c.transform, c.OnGround);
                if (c.iJump[i].GetJumping())
                {
                    break;//  return false;
                }
            }

            if (!c.GetJumping())
                c.input.MoveAxis = Vector3.zero;
        }
        else if (obstacleForward && !c.iClimb.GetClimbing() && !c.iVault.GetVaulting())
        {
            Vector3 dir = c.Agent.steeringTarget - c.transform.position;
            c.transform.rotation = Quaternion.Slerp(c.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3);
            c.input.MoveAxis = Vector3.zero;
        }

        return true;
    }

    protected bool TryPickupWeapon(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.NearestWeapon()!=null)
        {
            MoveToTarget(c.Agent, ai.NearestWeapon().Position());
            if (ai.NearestWeapon().Pickup((IWeaponSystomChangeable)c.weaponSystem, c.Position))
            {
                if (timer.GetComplete())
                {
                    timer.StartTimer();
                    c.Voice.PlaCollectingItem(c.VoiceAS);
                }
            }

            return true;
        }
        return false;
    }

    protected bool TryEnterVehicle(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.NearestVehicle() != null)
        {
            MoveToTarget(c.Agent, ai.NearestVehicle().GetPosition());
            ai.NearestVehicle().Enter(c, false);
            return true;
        }
        return false;
    }

    protected bool TryGetNearestHealth(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.GetNearestHealth() != Vector3.zero)
        {
            //seek health
            if (MoveToTarget(c.Agent, ai.GetNearestHealth()))
                if (timer.GetComplete())
                {
                    timer.StartTimer();
                    c.Voice.PlaySeekHealth(c.VoiceAS);
                }
            return true;
        }
        return false;
    }

    protected bool TryGetNearestAmmo(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.GetNearestAmmo() != Vector3.zero)
        {
            //seek health
            if (MoveToTarget(c.Agent, ai.GetNearestAmmo()))

                if (timer.GetComplete())
                {
                    timer.StartTimer();
                    c.Voice.PlayReload(c.VoiceAS);
                }
            return true;
        }
        return false;
    }

    protected bool TryGetNearestNeutralGoal(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.GetGoal() != Vector3.zero)
        {
            MoveToTarget(c.Agent, ai.GetGoal());
            return true;
        }
        return false;
    }

    protected bool TryGetNearestAllyGoal(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.GetAllyGoal() != Vector3.zero)
        {
            MoveToTarget(c.Agent, ai.GetAllyGoal());
            return true;
        }
        return false;
    }

    protected bool TryGetNearestHostileGoal(ModularControllerAI c)
    {
        Senses ai = (Senses)c.Senses;
        if (ai.GetHostileGoal() != Vector3.zero)
        {
            if (MoveToTarget(c.Agent, ai.GetHostileGoal()))

                if (timer.GetComplete())
                {
                    timer.StartTimer();
                    c.Voice.PlayApproachingGoal(c.VoiceAS);
                }
            return true;
        }
        return false;
    }

    protected override bool Approach(NavMeshAgent a, ModularController c)
    {
        if( base.Approach(a, c))
        {
            ModularControllerAI ai = (ModularControllerAI)c;
            if (timer.GetComplete())
            {
                timer.StartTimer();
                ai.Voice.PlayApproach(ai.VoiceAS);
            }
            return true;
        }
        return false;
    }

    protected override void Attack(ModularController controller, ModularController targetAgent, NavMeshAgent a, float shootAngle = 10)
    {
        if (((ModularHealthOrganism)(controller.Health)).r.Ragdolled)
            return;

        base.Attack(controller, targetAgent, a, shootAngle);
        if (a.isOnNavMesh && a.isStopped == false)
        {
            ModularControllerAI ai = (ModularControllerAI)controller;
            if (timer.GetComplete())
            {
                timer.StartTimer();
                ai.Voice.PlayEngaging(ai.VoiceAS);
            }
        }
    }

    protected override bool FleeFrom(ModularControllerMoveable c, NavMeshAgent a, Vector3 fleeFromPos)
    {
        if( base.FleeFrom(c, a, fleeFromPos))
        {
            if (timer.GetComplete())
            {
                timer.StartTimer();
                ModularControllerAI ai = (ModularControllerAI)c;
                ai.Voice.PlayFlee(ai.VoiceAS);
            }
            return true;
        }
        return false;
    }


    public bool UseHealableOnAllies(ModularControllerAI c)
    {
        if (!c.SpecialTimer.GetComplete())
        {
            return false;
        }
        if (((WeaponSystem)c.weaponSystem).GetSpecialOfType(SpecialType.Heal) != null)
        {
            IHealable h = ResourceManagerModular.instance.GetHealable(10, c.Team, c.Position);
            if (h != null)
            {
                if (h.NeedsHealing(c.Team))
                {
                    if (Vector3.Distance(c.Position, h.Position()) < 3)
                    {
                        UseItemOfType(c, SpecialType.Heal);
                    }
                    else
                    {
                        MoveToTarget(c.Agent, h.Position(), 2);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public bool HandleRepair(ModularControllerAI ai)
    {
        if (!ai.SpecialTimer.GetComplete())
        {
            return false;
        }
        IWeaponSystomChangeable w = (IWeaponSystomChangeable)ai.weaponSystem;
        ISpecial s = w.GetSpecialOfType(SpecialType.Repair);
        if (s != null)
        {
            IRepair r = ((Senses)ai.Senses).NearestRepair(ai.Team);
            if (r != null)
            {
                if (!s.Use())
                {
                    //Move closer
                    MoveToTarget(ai.Agent, r.Position(), 1);
                }
                else
                {
                    if (ai.Agent.isOnNavMesh)
                    {
                        ai.Agent.isStopped = true;
                        ai.Agent.updateRotation = false;
                    }
                }
                return true;
            }
        }

        return false;
    }

    public bool UseItemOfType(ModularControllerAI controller, SpecialType s)
    {
        if(!controller.SpecialTimer.GetComplete())
        {
            return false;
        }
        if(s == SpecialType.Repair)
        {
            return HandleRepair(controller);
        }
        controller.SpecialTimer.StartTimer();
        if(s == SpecialType.Combat)
            controller.Senses.TargetPos = controller.Senses.TargetPos - Vector3.up * 1.5f;
        else if(s == SpecialType.Heal)
        {
            controller.Senses.TargetPos = controller.Position + Vector3.up * 1 + controller.transform.forward * 2;
        }
        return ((WeaponSystem)controller.weaponSystem).UseSpecialOfType(s);
    }

    public bool HandleLeaderCommands(ModularControllerAI c)
    {
        if(c.Commands.targetPos != Vector3.zero)
        {
            MoveToTarget(c.Agent, c.Commands.targetPos, 2);
            return true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrainAISpaceshipBase : BrainAIBase
{
    public override void Execute(ModularController controller)
    {
        ModularControllerSpaceship c = (ModularControllerSpaceship)controller;
        c.input.Speed = (c.input.Sprint)? c.AIStats().GetSprintSpeed(): c.AIStats().GetRunSpeed();
       // if (c.Spaceship.GetState() == ShipState.Air)
       // {
            BehaviourTree(c);
      //  }
        c.Spaceship.Move(c.Senses.TargetPos, c.input.Speed);
    }

    protected abstract void BehaviourTree(ModularControllerSpaceship c);

    protected bool Land(ModularControllerSpaceship c)
    {
        c.Spaceship.TryLand();
        return c.Spaceship.GetState() == ShipState.Land;
    }

    protected void TakeOff(ModularControllerSpaceship c)
    {
        if (c.Spaceship.GetState() == ShipState.Idle)
        {
            c.Spaceship.TryTakeOff();
        }
    }

    protected bool Exit(ModularControllerSpaceship c)
    {
        if(c.Spaceship.GetState() == ShipState.Idle)
        {
            c.EnterExit.Exit();
            return true;
        }
        else
        {
            Land(c);
        }
        return false;
    }

    protected bool MoveToRandomTarget(ModularControllerSpaceship c)
    {
        Vector3 moveDir = c.RayDetection.GenerateRandom();
        if (moveDir != Vector3.zero)
        {
            SetTarget(c, moveDir);
            if (c.Senses.TargetPos != Vector3.zero)
            {
                c.input.Sprint = false;
                return true;
            }
        }
        return false;
    }

    protected bool EscapeCollision(ModularControllerSpaceship c)
    {
        Vector3 moveDir = c.RayDetection.CheckHits(out bool hit);
        if (!hit) c.input.Sprint = true;
        if(moveDir != Vector3.zero)
        {
            c.input.Speed = c.AIStats().GetWalkSpeed();
            c.Senses.TargetPos = moveDir;
            return true;
        }
        return false;
    }

    protected override void Attack(ModularController controller, ModularController targetAgent)
    {
        if (TargetInRange(controller, targetAgent)) //TODO move to new aiming script
        {
            base.Attack(controller, targetAgent);
        }
    }

    protected bool TargetInRange(ModularController c, ModularController targetAgent)
    {
        Vector3 dir = targetAgent.Position - c.Position;
        if (Vector3.Angle(c.transform.forward, dir) < 40) //TODO move to new aiming script
        {
            if (c.Senses.InRange(targetAgent.Position))
            {
                return true;
            }
        }
        return false;
    }

    protected bool TargetIsSpaceship(ModularController c)
    {
        return c.GetType() == typeof(ModularControllerSpaceship);
    }

    protected bool FleeFrom(ModularControllerSpaceship c, Vector3 target)
    {
        Vector3 dir = (target - c.Position).normalized;

        SetTarget(c, dir * 30 + new Vector3(Random.Range(-10, 10), Random.Range(-10,10), Random.Range(-10,10)));
        if (c.Senses.TargetPos != Vector3.zero)
        {
            c.input.Sprint = true;
            return true;
        }
        return false;
    }

    protected bool Boost(ModularControllerSpaceship c)
    {
        if (!c.Spaceship.GetBoosting())
        {
            if (!Physics.Raycast(c.Position, c.transform.forward * 30))
            {
                Debug.Log("Boost");
                c.Spaceship.BeginBoost();
                return true;
            }
        }
        return false;
    }

    protected bool Approach(ModularControllerSpaceship c, ModularController target)
    {
        if(TargetIsSpaceship(target))
        {
            Boost(c);
            c.input.Sprint = true;
        }
        else
        {
            c.input.Speed = c.AIStats().GetWalkSpeed();
        }
        SetTarget(c, target.Position);
        return false;
    }

    private void SetTarget(ModularControllerSpaceship c, Vector3 target)
    {
        Vector3 t = c.RayDetection.SetNewTarget(target);
        c.Senses.TargetPos = t;
    }

}

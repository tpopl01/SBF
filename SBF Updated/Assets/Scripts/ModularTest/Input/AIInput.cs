using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;
using UnityEngine.AI;


//[CreateAssetMenu
//  (
//      fileName = "Input_AI",
//      menuName = "Modular/Components/Input_AI"
//  )]
public class AIInput : InputBase, ISetup
{
    NavMeshAgent agent;
    AnimatorHook hook;
    Rigidbody rb;
    bool changedGrounded;
    public Vector3 hashedPos = Vector3.zero;
    bool stuck;
    public BrainBase brain;
    ModularControllerAI c;
    public override void Execute(ModularController controller)
    {
        c = (ModularControllerAI)controller;
        HandleInput(c);
        brain.Execute(controller);
    }

    void FixedUpdate()
    {
        HandleReEnablingAgent(c);
        PreventOnAirStick(c);
    }

    void HandleInput(ModularControllerAI controller)
    {
        Speed = Sprint ? controller.AIStats().GetSprintSpeed() : controller.AIStats().GetRunSpeed();
        if (Crouch || controller.iCover.GetUsingCover()) Speed = controller.AIStats().GetWalkSpeed();
        MoveAxis = GetMoveAxis(hook);
    }

    void PreventOnAirStick(ModularControllerAI controller)
    {
        if (hashedPos == transform.position && !controller.OnGround && !controller.grounded.Disable && controller.GetJumping() == false && controller.GetRolling() == false && controller.iVault.GetVaulting() == false && controller.iClimb.GetClimbing() == false)
        {
            rb.position = Vector3.Slerp(rb.position, agent.transform.position, Time.deltaTime * 10);
            Debug.Log("Prevent On Air");
        }
        hashedPos = transform.position;
    }

    bool repositioned;
    void HandleReEnablingAgent(ModularControllerAI controller)
    {
      //  if (controller.OnGround == false && !changedGrounded)
      //  {
      //      changedGrounded = agent.enabled;
      //      agent.enabled = false;
      //  }
        if (hashedPos == transform.position && controller.OnGround && !controller.Health.IsDead() && !changedGrounded && agent.enabled == false)
        {
            if (agent.isOnNavMesh && agent.pathPending && agent.enabled) return;
            changedGrounded = true;
           // agent.enabled = false;
        }
        //else if(agent.isOnNavMesh == false)
        //{
        //    changedGrounded = true;
        //  //  agent.enabled = false;
        //}
        else if (changedGrounded && !controller.Health.IsDead())
        {
            if (agent.isOnNavMesh || repositioned)
            {
                changedGrounded = false;
                agent.transform.position = controller.Position;
                agent.enabled = true;
                repositioned = false;
               // StartCoroutine(EnableAgent());
            }
            else
            {
                if(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                    Debug.Log("Reposition");
                    repositioned = true;
                  //  changedGrounded = false;
                    agent.transform.position = controller.Position;
                  //  StartCoroutine(EnableAgent());
                }
            }
        }
    }

    IEnumerator EnableAgent()
    {
        yield return new WaitForEndOfFrame();
        if (gameObject.activeInHierarchy)
        {
            repositioned = false;
            //agent.enabled = false;
            agent.enabled = true;
        }
    //    if(agent.enabled == false) StartCoroutine(EnableAgent());
    }

    public Vector3 GetMoveAxis(AnimatorHook animatorHook)
    {
        //  Vector3 dir = modularController.Agent.transform.position - modularController.transform.position;
        if (float.IsNaN(animatorHook.DeltaPosition.x) || float.IsNaN(animatorHook.DeltaPosition.y) || float.IsNaN(animatorHook.DeltaPosition.z))
            return Vector3.zero;

        //if (agent.isOnNavMesh && agent.remainingDistance < agent.stoppingDistance)
        //    return Vector3.zero;

        return animatorHook.DeltaPosition * 0.6f;
    }

    public void SetUp(Transform root)
    {
        rb = GetComponentInChildren<Rigidbody>();
        hook = GetComponentInChildren<AnimatorHook>();
        agent = GetComponentInChildren<NavMeshAgent>();
    }
}

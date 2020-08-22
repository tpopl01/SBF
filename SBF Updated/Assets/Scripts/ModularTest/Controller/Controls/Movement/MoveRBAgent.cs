using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveRBAgent : MonoBehaviour, IMove, ISetup, IAim, IRespawn
{
    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;
    bool aiming;
   // AnimatorHook animatorHook;

    public void Move(Vector3 input, float speed)
    {
        if (!aiming)
        {
            rb.velocity = input;
            agent.speed = speed;
            rb.transform.rotation = agent.transform.rotation;
        }
        //else
        //{
        //    anim.SetFloat("Forward",0, 0.1f, Time.deltaTime);
        //    anim.SetFloat("Sideways", 0, 0.1f, Time.deltaTime);
        //}
        MovementAnimations("Forward", "Sideways", anim);
    }

    void LateUpdate()
    {
        if (agent.isActiveAndEnabled && !aiming)
        {
            agent.transform.localPosition = Vector3.zero;
            Vector3 dir = agent.steeringTarget - rb.transform.position;
            if (dir == Vector3.zero) return;
            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime*5);
           // rb.transform.rotation = agent.transform.rotation;
           // MovementAnimations("Forward", "Sideways", anim);
            agent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void MovementAnimations(string v, string h, Animator anim)
    {
        Vector3 relativeDirection = transform.InverseTransformDirection(agent.desiredVelocity);
        anim.SetFloat(v, relativeDirection.z, 0.1f, Time.deltaTime);
        anim.SetFloat(h, relativeDirection.x, 0.1f, Time.deltaTime);
    }

    public void SetUp(Transform root)
    {
        rb = root.GetComponent<Rigidbody>();
      //  animatorHook = root.GetComponentInChildren<AnimatorHook>();
        anim = root.GetComponentInChildren<Animator>();
        agent = root.GetComponentInChildren<NavMeshAgent>();
    }

    public void SetAim(bool aiming)
    {
        this.aiming = aiming;
    }

    public bool Respawn()
    {
        agent.enabled = false;
        EnableAgent();
        return true;
    }

    IEnumerator EnableAgent()
    {
        yield return new WaitForEndOfFrame();
        agent.enabled = true;
    }
}

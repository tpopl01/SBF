using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveRBAgentNoRootMotion : MonoBehaviour, ISetup,IMove
{
    NavMeshAgent agent;
    Rigidbody rb;
    [SerializeField] Transform[] wheelHolders;
    [SerializeField] Transform[] wheels;
    Grounded g;
    [SerializeField] float angularSpeed = 80;
    [SerializeField] float acceleration = 20;
    EnterExit e;

    public void Move(Vector3 input, float speed)
    {
        if (e.Player) return;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        rb.velocity = speed * input;// * agent.velocity.magnitude;
      //  agent.speed = speed;
     //   Vector3 dir = agent.steeringTarget - rb.transform.position;
        rb.transform.rotation = agent.transform.rotation;
     //   rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime /(speed * angularDrag));
        MovementAnimations(speed);
        g.AlignToGround(5, 0, true);
    }

    void LateUpdate()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.transform.localPosition = Vector3.zero;
            agent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }


    private void MovementAnimations(float speed)
    {
        Vector3 relativeDirection = transform.InverseTransformDirection(agent.desiredVelocity);
        for (int i = 0; i < wheelHolders.Length; i++)
        {
            Vector3 directionToLookTo = agent.steeringTarget - transform.position;
            float angle = StaticMaths.AngleSigned(transform.forward, directionToLookTo, transform.up);
            angle = Mathf.Clamp(angle, -15, 15);
            wheelHolders[i].localRotation = Quaternion.Slerp(wheelHolders[i].localRotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 1);
        }

        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(Vector3.right, 100 * speed * relativeDirection.z * Time.fixedDeltaTime);// * Time.deltaTime);
        }
    }

    public void SetUp(Transform root)
    {
        rb = root.GetComponent<Rigidbody>();
        agent = root.GetComponentInChildren<NavMeshAgent>();
        g = root.GetComponentInChildren<Grounded>();
        e = GetComponent<EnterExit>();
    }
}

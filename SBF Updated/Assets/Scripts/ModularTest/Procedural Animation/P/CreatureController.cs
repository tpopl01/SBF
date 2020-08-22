using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour, IMove, ISetup {
    public float moveInputFactor = 5f;
    public Vector3 velocity;
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotateInputFactor = 10f;
    public float rotationSpeed = 10f;
    public float averageRotationRadius = 3f;
    private float rSpeed = 0;

    public ProceduralLegPlacement[] legs;
    private int index;
    public bool dynamicGait = false;
    public float timeBetweenSteps = 0.25f;
    [Tooltip ("Used if dynamicGait is true to calculate timeBetweenSteps")] public float maxTargetDistance = 1f;
    public float lastStep = 0;
    [SerializeField] float dynamicStepLength = 2;

    void Start () {

    }

    public void SetRotateSpeed(Vector3 target)
    {
        //    this.rSpeed = rSpeed * Time.deltaTime;
        //this.rSpeed = rSpeed * Time.deltaTime * 2500;
        Rotate(target);
    }

    void Rotate(Vector3 target)
    {
        Vector3 directionToLookTo = target - transform.position;
        directionToLookTo.y = 0;
        float a = Vector3.SignedAngle(transform.forward, directionToLookTo, Vector3.up);
        float angle = Mathf.Abs(a);
        if (angle > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(directionToLookTo);
            angle = 10f / angle;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * angle);
        }
        else a = 0;
        rSpeed = Mathf.Clamp(a, -1, 1) * Time.deltaTime * 2500;
    }

    public void Move (Vector3 velocity, float mSpeed)
    {
        mSpeed = Mathf.Clamp(mSpeed, 0, sprintSpeed);
        g.AlignToGround(5, 2, true, 20);
        velocity = velocity.normalized;
      //  velocity.y = 0;
        transform.position += velocity * mSpeed * Time.deltaTime;

        if (dynamicGait) {
            timeBetweenSteps = maxTargetDistance / Mathf.Max (mSpeed * velocity.magnitude, Mathf.Abs (rSpeed * Mathf.Deg2Rad * averageRotationRadius));
            timeBetweenSteps *= dynamicStepLength;
        }

        if (Time.time > lastStep + (timeBetweenSteps / legs.Length) && legs != null) {
            if (legs[index] == null) return;

            Vector3 legPoint = (legs[index].restingPosition + velocity);
            Vector3 legDirection = legPoint - transform.position;
            //Vector3 rotationalPoint = (legs[index].transform.TransformDirection (Vector3.right)) * (rSpeed * Mathf.Deg2Rad * (legPoint - transform.position).magnitude);
            Vector3 rotationalPoint = ((Quaternion.Euler (0, rSpeed / 2f, 0) * legDirection) + transform.position) - legPoint;
            Debug.DrawRay (legPoint, rotationalPoint, Color.black, 1f);
            Vector3 rVelocity = rotationalPoint + velocity;

            legs[index].stepDuration = Mathf.Min (0.5f, timeBetweenSteps / 2f);
            legs[index].worldVelocity = rVelocity;
            legs[index].Step ();
            lastStep = Time.time;
            index = (index + 1) % legs.Length;

          //  legs[index].dynamicStep = dynamicStepLength / 2;
        }
    }

    public void OnDrawGizmosSelected () {
        Gizmos.DrawWireSphere (transform.position, averageRotationRadius);
    }

    NavMeshAgent agent;
    Grounded g;

    public void SetUp(Transform root)
    {
        agent = root.GetComponentInChildren<NavMeshAgent>();
        g = root.GetComponentInChildren<Grounded>();
    }
    void LateUpdate()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.transform.localPosition = Vector3.zero;
            agent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public class JumpAI1 : LerpCurveOverTimeBase, IJump, ISetup, ITick, IDeath
{
    NavMeshAgent agent;
    Rigidbody rb;
  //  bool jumping;
    protected float jumpDist = 6;
    protected float sampleArea = 2;
    Transform root;
    AudioProfile p1;
    AudioSource aS1;

   // Timer t;

    public virtual void SetUp(Transform root)
    {
        this.root = root;
        agent = root.GetComponentInChildren<NavMeshAgent>();
        rb = root.GetComponentInChildren<Rigidbody>();
        ModularControllerAI ai = root.GetComponentInChildren<ModularControllerAI>();
        p1 = ai.Voice;
        Debug.Log(p1);
        aS1 = ai.VoiceAS;
        Init(root, new string[] { "run_jump_take_off" }, Resources.Load<CurveHolder>("Curves/jumppack_curve"), 2);
    }
    public void BeginJump(Transform root, bool onGround)
    {
        if (!onGround || !CanStart()) return;

        Vector3 target = root.position + root.forward * jumpDist;
        if(NavMesh.SamplePosition(target, out NavMeshHit hit, sampleArea, NavMesh.AllAreas))
        {
            if (!Physics.Raycast(rb.position + Vector3.up * 1.8f, hit.position, Vector3.Distance(rb.position + Vector3.up * 1.8f, hit.position) - 0.5f))
            {
                if (CanStart())
                {
                    agent.enabled = false;
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    SetSpeedModifier(50);
                    Begin(hit.position + Vector3.up * 0.5f);
                    p1.PlayJumpEffect(aS1);
                 //   TestJump(target);
                }
            }
        }

    }

    //void TestJump(Vector3 target)
    //{
    //    inProgress = true;
    //    float h = 3;
    //    if (transform.position.y > target.y)
    //    {
    //        h = 3;// (transform.position.y - target.y);
    //    }
    //    else
    //    {
    //        h = (target.y - transform.position.y) + 2;
    //    }

    //    rb.transform.LookAt(target);
    //    rb.useGravity = true;
    //    rb.velocity = CalculateLaunchData(rb, target, Physics.gravity.y, h).initialVelocity;
    //}

    //LaunchData CalculateLaunchData(Rigidbody newBall, Vector3 target, float gravity, float h)
    //{
    //    float displacementY = target.y - newBall.position.y;
    //    Vector3 displacementXZ = new Vector3(target.x - newBall.position.x, 0, target.z - newBall.position.z);
    //    float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity); //(squrt(-2 * height / gravity) + squrt((2(end y point - height)) / gravity))
    //    Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h); //squrt(-2 * gravity * height)
    //    Vector3 velocityXZ = displacementXZ / time; //velocity = displacement / (squrt(-2 * height / gravity) + squrt((2(end y point - height)) / gravity)). Time is calculated above.
    //    //return velocityXZ + velocityY
    //    t = new Timer(time);
    //    return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time); //sign bit allows you to launch the ball downwards if you have a positive gravity value
    //}

    protected override void Stop()
    {
        base.Stop();
        rb.isKinematic = false;
        rb.useGravity = true;
        // agent.enabled = true;
        StartCoroutine(EnableAgent());
    }

    IEnumerator EnableAgent()
    {
        yield return new WaitForEndOfFrame();
        if(agent.isOnNavMesh)
            agent.enabled = true;
    }

    public bool GetJumping()
    {
        return inProgress;
    }

    public void Tick()
    {
        //if(GetJumping())
        //{
        //    if(t.GetComplete())
        //    {
        //        inProgress = false;
        //        Stop();
        //    }
        //}

        OnTick();
    }

    public void Death()
    {
        Stop();
        
       // jumping = false;
    }
}

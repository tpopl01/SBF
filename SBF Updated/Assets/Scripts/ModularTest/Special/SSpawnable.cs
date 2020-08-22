using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class SSpawnable : SpecialBase1, IGetDisableIK
{
    protected Animator anim;
    [SerializeField] string animName = "";
    [SerializeField] float animTime = 0.7f;
    [SerializeField] bool debug = false;
    //   [SerializeField] [Range(0, 5)] float spawn = 0;
    [SerializeField] [Range(1, 10)] float timer = 3;
    ModularController c;

    [SerializeField] AudioProfileGeneral throwItemSound;
    AudioSource aS;

    Timer t = new Timer(1);
    Timer disabled;
    bool thrown;
    bool disableIK;
    Vector3 targetPos;

    public override void SetUp(Transform root)
    {
        base.SetUp(root);
        aS = GetComponentInParent<AudioSource>();
        if (aS == null)
        {
            aS = transform.parent.gameObject.AddComponent<AudioSource>();
            aS.spatialBlend = 1.0f;
            aS.playOnAwake = false;
        }
        disabled = new Timer(animTime);
        t = new Timer(timer);
        t.StartTimer();
        anim = root.GetComponentInChildren<Animator>();
        c = root.GetComponent<ModularController>();
    }

    private void Update()
    {
        if (thrown)
        {
            if (disabled.GetComplete())
            {
                Rigidbody gO = Instantiate(Resources.Load<Rigidbody>("Specials/" + slug), anim.GetBoneTransform(HumanBodyBones.RightHand).position, Quaternion.identity);
                LaunchItem(gO, targetPos);
                OnSpawn(gO, c);
            }
        }
    }

    public override bool Use()
    { 
        //if (Vector3.Distance(c.Senses.TargetPos, transform.position) < 4)
        //{
        //    return false;
        //}

        if (t.GetComplete())
        {
            if (base.Use())
            {
                if (disableIK)
                    return false;
                t.StartTimer();
                disableIK = true;
                anim.Play(animName);
                disabled.StartTimer();
                thrown = true;
                targetPos = c.Senses.TargetPos;
                return true;
            }
        }
        return false;
    }

    protected virtual void OnSpawn(Rigidbody rb, ModularController c)
    {
        throwItemSound.PlaySound(aS);
        disableIK = false;
        thrown = false;
    }
    protected void LaunchItem(Rigidbody gO, Vector3 target)
    {
        float h = 3;
        if (transform.position.y > target.y)
        {
            h = 1;// (transform.position.y - target.y);
        }
        else
        {
            h = (target.y - transform.position.y) + 2;
        }

        gO.transform.LookAt(target);
        gO.useGravity = true;
        gO.velocity = CalculateLaunchData(gO, target, Physics.gravity.y, h).initialVelocity;

        if (debug)
        {
            DrawPath(gO, target, Physics.gravity.y, h);
        }
    }

    LaunchData CalculateLaunchData(Rigidbody newBall, Vector3 target, float gravity, float h)
    {
        float displacementY = target.y - newBall.position.y;
        Vector3 displacementXZ = new Vector3(target.x - newBall.position.x, 0, target.z - newBall.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity); //(squrt(-2 * height / gravity) + squrt((2(end y point - height)) / gravity))
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h); //squrt(-2 * gravity * height)
        Vector3 velocityXZ = displacementXZ / time; //velocity = displacement / (squrt(-2 * height / gravity) + squrt((2(end y point - height)) / gravity)). Time is calculated above.
        //return velocityXZ + velocityY
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time); //sign bit allows you to launch the ball downwards if you have a positive gravity value
    }

    void DrawPath(Rigidbody newBall, Vector3 target, float gravity, float h)
    {
        LaunchData launchData = CalculateLaunchData(newBall, target, gravity, h);
        Vector3 previousDrawPoint = newBall.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = newBall.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    public bool GetDisableIK()
    {
        return disableIK;
    }
}

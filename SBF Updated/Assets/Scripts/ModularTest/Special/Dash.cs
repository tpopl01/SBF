using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class Dash : MonoBehaviour, IRoll, IFixedTick, ISetup//, ITick
{
    Timer timer = new Timer(0.5f);
    protected bool rolling;
    protected Rigidbody rigid;
    protected string animName = "Dash";
    [SerializeField] protected float length = 0.05f;
    [SerializeField] protected float speed = 50;
    Animator anim;
    //  IRoll r;
    //  int wait = 0;
    ModularControllerMoveable c;
    [SerializeField]ParticleSystem[] particles;
    AudioProfileGeneral jetPackEngine;
    AudioSource aS;
    Vector3 dir;
    //protected override void Init(Transform root)
    //{
    //    base.Init(root);
    //}

    //protected override void Begin()
    //{
    //    if (inp.MoveAxis == Vector3.zero && !GetRolling())
    //    {
    //        rolling = true;
    //        anim.Play(animName);
    //        //   wait++;
    //        //  if (wait > 1)
    //        //   {
    //        //      wait = 0;
    //        base.Begin();
    //      //  }
    //    }
    //}

    //protected override void RollFinished()
    //{
    //    base.RollFinished();
    //  //  rigid.velocity = Vector3.zero;
    //}

    public bool GetRolling()
    {
        return rolling;
    }

    public void BeginRoll()
    {
        if (!rolling && (!c.OnGround || c.grounded.Disable))
        {
        //    wait++;
           // if (wait > 1)
           // {
                timer.StartTimer();
                rolling = true;
                anim.Play(animName);
            PlayParticles(true);
            dir = rigid.transform.forward;
            if (c.input.MoveAxis != Vector3.zero)
            {
                dir = CameraManager.instance.transform.forward * c.input.MoveAxis.z + CameraManager.instance.transform.right * c.input.MoveAxis.x + CameraManager.instance.transform.up * c.input.MoveAxis.y;
                dir.Normalize();
            }
            //  }
        }
    }

    void PlayParticles(bool play)
    {
        if (particles == null) return;
        for (int i = 0; i < particles.Length; i++)
        {
            if (play)
                particles[i].Play();
            else particles[i].Stop();
        }
    }

    public void FixedTick()
    {
        if (rolling)
        {
            jetPackEngine.PlaySound(aS);
            
            Vector3 v2 = (dir * speed);
            rigid.velocity = v2;
            if (timer.GetComplete())
            {
                rolling = false;
                PlayParticles(false);
                jetPackEngine.Stop(aS);
            }
        }
    }

    public void SetUp(Transform root)
    {
        rigid = root.GetComponent<Rigidbody>();
        anim = root.GetComponentInChildren<Animator>();
       // r = root.GetComponent<IRoll>();
        c = root.GetComponent<ModularControllerMoveable>();
        PlayParticles(false);
        aS = GetComponent<AudioSource>();
        if (aS == null)
        {
            aS = gameObject.AddComponent<AudioSource>();
            aS.spatialBlend = 1.0f;
            aS.playOnAwake = false;
            aS.loop = true;
        }
        jetPackEngine = Resources.Load<AudioProfileGeneral>("Audio/Audio_General_JetPack");
    }

    //public void Tick()
    //{
    //    if(wait > 0)
    //    if (r.GetRolling() == false) wait = 0;
    //}

    //public void Tick()
    // {
    //   if (!roll.GetRolling()) wait = 0;
    // }
}

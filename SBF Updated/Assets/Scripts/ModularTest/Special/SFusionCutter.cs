using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public class SFusionCutter : SpecialBase1
{
    [SerializeField] ParticleSystem[] particles = null;
    [SerializeField] string animName = "";
    [SerializeField] AudioProfileGeneral cutterAudio = null;
    AudioSource aS;
    bool inUse;
    Animator anim;
    IK iK;
    ModularController c;
    IRepair h;
    Timer rTimer = new Timer(3);

    public override void SetUp(Transform root)
    {
        base.SetUp(root);
        particles = GetComponentsInChildren<ParticleSystem>();
        gameObject.SetActive(false);
        c = root.GetComponent<ModularController>();
        anim = root.GetComponentInChildren<Animator>();
        iK = root.GetComponentInChildren<IK>();
        aS = GetComponentInParent<AudioSource>();
        if (aS == null)
        {
            aS = transform.parent.gameObject.AddComponent<AudioSource>();
            aS.spatialBlend = 1.0f;
            aS.playOnAwake = false;
            aS.loop = true;
        }
    }

    void Stop()
    {
        if (gameObject.activeInHierarchy)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (!particles[i].isPlaying)
                    particles[i].Stop();
            }
            gameObject.SetActive(false);
            if (iK)
            {
                iK.DisableIK(false);
                anim.Play("Land 1");
                WeaponSystem w = (WeaponSystem)c.weaponSystem;
                w.EnableGuns();
                cutterAudio.Stop(aS);
                inUse = false;
            }
        }
    }

    private void StartUsing()
    {
        Debug.Log("Start Using");
        inUse = true;
        //start
        gameObject.SetActive(true);
        for (int i = 0; i < particles.Length; i++)
        {
            if (!particles[i].isPlaying)
                particles[i].Play();
        }
        cutterAudio.PlaySound(aS);
        WeaponSystem w = (WeaponSystem)c.weaponSystem;
        w.DisableGuns();

        iK.DisableIK(true);
        anim.Play(animName);
    }

    private void Update()
    {
        if (rTimer.GetComplete())
        {
            rTimer.StartTimer();
            if (!inUse)
            {
                Stop();
            }
            else if (inUse)
            {
                if (h.Repair())
                {
                    Stop();
                }
                Quaternion q = StaticMaths.GetLookRotation(h.Position(), c.transform.position, c.transform.forward, out bool r);
                if (r)
                    c.transform.rotation = Quaternion.Slerp(c.transform.rotation, q, Time.deltaTime * 2);
                if (h.InRange(transform.position) == false)
                    Stop();
            }
        }
        if(inUse)
        {
            iK.DisableIK(true);
        }
    }

    public override bool Use()
    {
        h = ((Senses)c.Senses).NearestRepair(c.Team);
        if (h != null)
        {
            if (h.InRange(transform.position))
            {
                StartUsing();
                return true;
            }
        }
        Stop();
        return false;
    }
}

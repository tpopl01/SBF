using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumppackAI : JumpAI1
{
    ParticleSystem[] p;
    AudioProfileGeneral jetPackEngine;
    AudioSource aS;

    public override void SetUp(Transform root)
    {
        base.SetUp(root);
        p = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Stop();
        }
        jumpDist = 12;
        sampleArea = 8;
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

    protected override void Begin(Vector3 targetPos)
    {
        base.Begin(targetPos);
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Play();
        }
        jetPackEngine.PlaySound(aS);
    }

    protected override void Stop()
    {
        base.Stop();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Stop();
        }
        jetPackEngine.Stop(aS);
    }


}

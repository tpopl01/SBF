using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppack : Jump
{
    [SerializeField]ParticleSystem[] p;
    AudioProfileGeneral jetPackEngine;
    AudioSource aS;

    public override void SetUp(Transform root)
    {
        base.SetUp(root);
        if(p==null)
        p = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Stop();
        }
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

    public override void Begin()
    {
        base.Begin();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Play();
        }
        jetPackEngine.PlaySound(aS);
    }

    protected override void Complete()
    {
        base.Complete();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].Stop();
        }
        jetPackEngine.Stop(aS);
    }
}

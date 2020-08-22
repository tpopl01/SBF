using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public class ModularControllerAI : ModularControllerUnit
{
    public NavMeshAgent Agent { get; private set; } = null;
    public AnimatorHook Hook { get; private set; } = null;
    public AudioProfile Voice { get; private set; } = null;
    public AudioSource VoiceAS { get; private set; } = null;
    public Timer SpecialTimer { get; private set; }
    public LeaderCommands Commands { get; private set; }

    protected override void Initialise()
    {
        base.Initialise();
        Commands = GetComponentInChildren<LeaderCommands>();
        SpecialTimer = new Timer(Random.Range(20, 200));
        SpecialTimer.StartTimer();
        Agent = GetComponentInChildren<NavMeshAgent>();
        Hook = GetComponentInChildren<AnimatorHook>();

        if (Team == 0)
        {
            Voice = Resources.Load<AudioProfile>("Modular/Audio/Voice/REP/Audio_Character_REP");
        }
        else
        {
            Voice = Resources.Load<AudioProfile>("Modular/Audio/Voice/CIS/Audio_Character_CIS");
        }
        //Debug.Log(Voice + name);
        VoiceAS = Senses.gameObject.AddComponent<AudioSource>();
        VoiceAS.spatialBlend = 1;
    }

    protected override void Tick()
    {
        base.Tick();
        Vector3 euler = transform.rotation.eulerAngles;
        euler.z = 0;
        euler.x = 0;
        transform.rotation = Quaternion.Euler(euler);
    }
}

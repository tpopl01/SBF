using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class Footsteps : MonoBehaviour, ISetup, IMove
{
    AudioProfileFootstep footstep;
    AudioSource aS;
    AIStats stats;
    Timer t = new Timer(0.3f);
    float walkSpeed = 0.8f;
    float runSpeed = 0.6f;
    float sprintSpeed = 0.3f;

    public void SetUp(Transform root)
    {
        aS = gameObject.AddComponent<AudioSource>();
        aS.spatialBlend = 1;
        ModularController c = root.GetComponent<ModularController>();
        stats = c.AIStats();
        if(c.Team == 1)
        {
            footstep = Resources.Load<AudioProfileFootstep>("Modular/Audio/Footsteps/CIS/Audio_Footstep");
        }
        else
        {
            footstep = Resources.Load<AudioProfileFootstep>("Modular/Audio/Footsteps/REP/Audio_Footstep");
        }
    }

    public void Move(Vector3 input, float speed)
    {
        if (t.GetComplete())
        {
            if (speed == 0 || input == Vector3.zero)
            {
                // footstep.PlayAudio(aS, SurfaceType.standard, MovementType.idle);
            }
            else
            {
                if (stats.GetSprintSpeed() <= speed && t.Duration != sprintSpeed) t = new Timer(sprintSpeed);
                else if (stats.GetRunSpeed() <= speed && t.Duration != runSpeed) t = new Timer(runSpeed);
                else if (t.Duration != walkSpeed) t = new Timer(walkSpeed);
                footstep.PlayAudio(aS, SurfaceType.standard);
            }
            t.StartTimer();
        }
    }
}

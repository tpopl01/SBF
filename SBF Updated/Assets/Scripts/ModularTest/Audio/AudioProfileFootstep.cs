using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_Footstep",
      menuName = "Audio/Footstep"
  )]
public class AudioProfileFootstep : ScriptableObject
{
    [SerializeField] AudioClip[] sandSnow = null;
    [SerializeField] AudioClip[] standard = null;
    Timer sprintTime = new Timer(0.05f);
    Timer walkTime = new Timer(0.2f);
    Timer runTime = new Timer(0.1f);

    public void PlayAudio(AudioSource aS, SurfaceType surfaceType)
    {
        switch (surfaceType)
        {
            case SurfaceType.standard:
                PlaySound(aS, standard);
                break;
            case SurfaceType.sand:
                PlaySound(aS, sandSnow);
                break;
            default:
                break;
        }
    }

    public void PlayAudio(AudioSource aS, SurfaceType surfaceType, MovementType movementType)
    {
        if (movementType == MovementType.idle)
            return;

        if(movementType == MovementType.walk)
        {
            if (walkTime.GetComplete())
            {
                walkTime.StartTimer();
            }
            else return;
        }
        else if(movementType == MovementType.run)
        {
            if (runTime.GetComplete())
            {
                runTime.StartTimer();
            }
            else return;
        }
        else if(movementType == MovementType.sprint)
        {
            if (sprintTime.GetComplete())
            {
                sprintTime.StartTimer();
            }
            else return;
        }

        if(!aS.isPlaying)
        {
            switch (surfaceType)
            {
                case SurfaceType.standard:
                    PlaySound(aS, standard);
                    break;
                case SurfaceType.sand:
                    PlaySound(aS, sandSnow);
                    break;
                default:
                    break;
            }
        }
    }
    void PlaySound(AudioSource aS, AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

       // if (!aS.isPlaying)
      //  {
            aS.clip = clips[Random.Range(0, clips.Length)];
            aS.Play();
       // }
    }
}
public enum SurfaceType
{
    standard,
    sand
}

public enum MovementType
{
    idle,
    walk,
    run,
    sprint
}
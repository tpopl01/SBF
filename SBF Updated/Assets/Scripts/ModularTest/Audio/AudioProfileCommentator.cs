using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_Commentator",
      menuName = "Audio/Commentator"
  )]
public class AudioProfileCommentator : ScriptableObject
{
    [SerializeField] AudioClip[] start = null;
    [SerializeField] AudioClip[] defeat = null;
    [SerializeField] AudioClip[] victory = null;
    [SerializeField] AudioClip[] defeatNear = null;
    [SerializeField] AudioClip[] victoryNear = null;
    [SerializeField] AudioClip[] capturedCP = null;
    [SerializeField] AudioClip[] lostCP = null;

    public void PlayStartAudio(AudioSource aS)
    {
        PlaySound(aS, start);
    }
    public void PlayDefeatAudio(AudioSource aS)
    {
        PlaySound(aS, defeat);
    }
    public void PlayVictoryAudio(AudioSource aS)
    {
        PlaySound(aS, victory);
    }
    public void PlayDefeatNearAudio(AudioSource aS)
    {
        PlaySound(aS, defeatNear);
    }
    public void PlayVictoryNearAudio(AudioSource aS)
    {
        PlaySound(aS, victoryNear);
    }
    public void PlayCapturedCPAudio(AudioSource aS)
    {
        PlaySound(aS, capturedCP);
    }
    public void PlayLostCPAudio(AudioSource aS)
    {
        PlaySound(aS, lostCP);
    }


    void PlaySound(AudioSource aS, AudioClip[] clips)
    {
        if (!aS.isPlaying)
        {
            aS.clip = clips[Random.Range(0, clips.Length)];
            aS.Play();
        }
    }

}

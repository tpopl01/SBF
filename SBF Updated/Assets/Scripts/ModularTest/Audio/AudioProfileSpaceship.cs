using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu
  (
      fileName = "Audio_Spaceship",
      menuName = "Audio/Spaceship"
  )]
public class AudioProfileSpaceship : ScriptableObject
{
    [SerializeField] AudioClip[] ascend = null;
    [SerializeField] AudioClip[] descend = null;
    [SerializeField] AudioClip[] engine = null;

    public void Ascend(AudioSource aS)
    {
        if (!AudioTypePlaying(aS, ascend))
        {
            aS.volume = 0.5f;
            PlaySound(aS, ascend);
        }
    }
    public void Descend(AudioSource aS)
    {
        if (!AudioTypePlaying(aS, descend))
        {
            aS.volume = 0.5f;
            PlaySound(aS, descend);
        }
    }
    public void Engine(AudioSource aS, float speed = 1)
    {
        if (!AudioTypePlaying(aS, engine))
        {
            aS.volume = Mathf.Clamp01(speed);
            PlaySound(aS, engine);
        }
    }
    public void Stop(AudioSource aS)
    {
        aS.Stop();
    }

    bool AudioTypePlaying(AudioSource aS, AudioClip[] clips)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if(clips[i] == aS.clip)
            {
                return true;
            }
        }
        return false;
    }

    void PlaySound(AudioSource aS, AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

      //  if (!aS.isPlaying)
       // {
            aS.clip = clips[Random.Range(0, clips.Length)];
            aS.Play();
      //  }
    }
}

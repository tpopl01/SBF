using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_General_Volume",
      menuName = "Audio/GeneralVolume"
  )]
public class AudioProfileGeneralVolume : ScriptableObject
{
    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] bool loop = true;


    public void PlaySound(AudioSource aS, float volume = 1)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

        if (!aS.isPlaying)
        {
            aS.volume = Mathf.Clamp01(volume);
            aS.loop = loop;
            aS.clip = audioClips[Random.Range(0, audioClips.Length)];
            aS.Play();
        }
    }

    public void Stop(AudioSource aS)
    {
        if (aS.isPlaying)
        {
            aS.Stop();
        }
    }
}

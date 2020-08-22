using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_General",
      menuName = "Audio/General"
  )]
public class AudioProfileGeneral : ScriptableObject
{
    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] bool loop = true;
    [SerializeField] bool stopIfPlaying = false;


    public void PlaySound(AudioSource aS)
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

        if (!aS.isPlaying || stopIfPlaying)
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_Music",
      menuName = "Audio/Music"
  )]
public class AudioProfileMusic : ScriptableObject
{
    [SerializeField] AudioClip[] general = null;
    [SerializeField] AudioClip[] winning = null;
    [SerializeField] AudioClip[] losing = null;
    [SerializeField] AudioClip[] defeat = null;
    [SerializeField] AudioClip[] victory = null;

    public void PlayGeneralAudio(AudioSource aS)
    {
        PlaySound(aS, general);
    }
    public void PlayWinningAudio(AudioSource aS)
    {
        PlaySound(aS, winning);
    }
    public void PlayLosingAudio(AudioSource aS)
    {
        PlaySound(aS, losing);
    }
    public void PlayDefeatAudio(AudioSource aS)
    {
        aS.loop = false;
        PlaySound(aS, defeat);
    }
    public void PlayVictoryAudio(AudioSource aS)
    {
        aS.loop = false;
        PlaySound(aS, victory);
    }

    void PlaySound(AudioSource aS, AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

        if (!aS.isPlaying)
        {
            aS.clip = clips[Random.Range(0, clips.Length)];
            aS.Play();
        }
    }
}

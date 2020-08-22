using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Audio_Character",
      menuName = "Audio/Character"
  )]
public class AudioProfile : ScriptableObject
{
    [SerializeField] AudioClip[] follow = null;
    [SerializeField] AudioClip[] wait = null;
    [SerializeField] AudioClip[] overThere = null;
    [SerializeField] AudioClip[] dismiss = null;
    [SerializeField] AudioClip[] confirmWait = null;
    [SerializeField] AudioClip[] confirmOverThere = null;
    [SerializeField] AudioClip[] clear = null;
    [SerializeField] AudioClip[] no = null;
    [SerializeField] AudioClip[] yes = null;
    [SerializeField] AudioClip[] grenade = null;
    [SerializeField] AudioClip[] approachHostileGoal = null;
    [SerializeField] AudioClip[] hitSelf = null;
    [SerializeField] AudioClip[] kill = null;
    [SerializeField] AudioClip[] hitTarget = null;
    [SerializeField] AudioClip[] callAllies = null;
    [SerializeField] AudioClip[] engaging = null;
    [SerializeField] AudioClip[] collectingItem = null;
    [SerializeField] AudioClip[] jumpEffect = null;
    [SerializeField] AudioClip[] flee = null;
    [SerializeField] AudioClip[] approach = null;
    [SerializeField] AudioClip[] seekHealth = null;
    [SerializeField] AudioClip[] reload = null;


    public void PlayReload(AudioSource aS)
    {
        PlaySound(aS, reload);
    }
    public void PlaySeekHealth(AudioSource aS)
    {
        PlaySound(aS, seekHealth);
    }
    public void PlayFlee(AudioSource aS)
    {
        PlaySound(aS, flee);
    }
    public void PlayApproach(AudioSource aS)
    {
        PlaySound(aS, approach);
    }
    public void PlayFollowCommand(AudioSource aS)
    {
        PlaySound(aS, follow);
    }
    public void PlayWaitCommand(AudioSource aS)
    {
        PlaySound(aS, wait);
    }
    public void PlayOverThereCommand(AudioSource aS)
    {
        PlaySound(aS, overThere);
    }
    public void PlayDismissCommand(AudioSource aS)
    {
        PlaySound(aS, dismiss);
    }

    public void PlayConfirmOverThere(AudioSource aS)
    {
        PlaySound(aS, confirmOverThere);
    }
    public void PlayConfirmWait(AudioSource aS)
    {
        PlaySound(aS, confirmWait);
    }
    public void PlayClear(AudioSource aS)
    {
        PlaySound(aS, clear);
    }
    public void PlayApproachingGoal(AudioSource aS)
    {
        PlaySound(aS, approachHostileGoal);
    }
    public void Kill(AudioSource aS)
    {
        PlaySound(aS, kill);
    }
    public void PlayNo(AudioSource aS)
    {
        PlaySound(aS, no);
    }

    public void PlayYes(AudioSource aS)
    {
        PlaySound(aS, yes);
    }
    public void PlayGrenade(AudioSource aS)
    {
        PlaySound(aS, grenade);
    }
    public void PlayHitSelf(AudioSource aS)
    {
        PlaySound(aS, hitSelf, true);
    }
    public void PlayHitTarget(AudioSource aS)
    {
        PlaySound(aS, hitTarget);
    }
    public void PlayCallAllies(AudioSource aS)
    {
        PlaySound(aS, callAllies);
    }
    public void PlayEngaging(AudioSource aS)
    {
        PlaySound(aS, engaging);
    }
    public void PlaCollectingItem(AudioSource aS)
    {
        PlaySound(aS, collectingItem);
    }
    public void PlayJumpEffect(AudioSource aS)
    {
        PlaySound(aS, jumpEffect);
    }


    void PlaySound(AudioSource aS, AudioClip[] clips, bool forcePlay = false)
    {
        if(clips.Length == 0)
        {
            Debug.LogWarning("No Clips");
            return;
        }

        if (!aS.isPlaying || forcePlay)
        {
            aS.clip = clips[Random.Range(0, clips.Length)];
            aS.Play();
        }
    }

}

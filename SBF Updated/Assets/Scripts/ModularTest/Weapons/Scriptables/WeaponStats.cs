using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

namespace tpopl001.Weapons
{
    public class WeaponStats : ScriptableObject
    {
        [Space]
        [Header("Accuracy")]
        [SerializeField] [Range(0.5f, 3)] public float weaponSpread = 1;
        [SerializeField][Range(5, 200)] private float range = 60;
        [SerializeField] private AnimationCurve HitCurve = null;
        [Space]
        [Header("Damage")]
        [SerializeField] private AnimationCurve DamageCurve = null;
        [SerializeField] private float maxDamage = 0;

        [Space]
        [Header("Time")]
        [SerializeField] private float timeBetweenShots = 0.2f;

        [Space]
        [Header("Audio")]
        [SerializeField] AudioProfileGeneral fireAudio;

        public virtual void Init()
        {

        }


        public float GetTimeBetweenShots()
        {
            return timeBetweenShots;
        }

        //normalised
        public float GetRange(float distance)
        {
            return Mathf.Clamp01(distance / range);
        }

        public float GetActualRange()
        {
            return range;
        }


        public float hitChance(float distance)
        {
            if (distance > range)
                return 0;

            float x = Mathf.Clamp01(distance / range);
            float y = HitCurve.Evaluate(x);
            return y;
        }

        public float GetDamageAtDistance(float distance)
        {
            if (distance > range)
                return 0;

            float x = Mathf.Clamp01(distance / range);
            float y = DamageCurve.Evaluate(x); // normalised damage [0,1]

            return maxDamage * y;
        }
        public void PlayShoot(AudioSource aS)
        {
            if(aS.enabled)
                fireAudio.PlaySound(aS);
        }
    }
}
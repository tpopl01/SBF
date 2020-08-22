using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

namespace tpopl001.Weapons {
    public class Weapon : WeaponBase
    {
        [SerializeField] protected WeaponStats weaponStats;
        protected Timer timer;
        [SerializeField]protected Bullet1[] bulletSpawn;
        protected AudioSource shootAS;

        public override void Initialise()
        {
            timer = new Timer(weaponStats.GetTimeBetweenShots());
            timer.StartTimer();
            weaponStats.Init();
            shootAS = GetComponent<AudioSource>();
            if (shootAS == null)
            {
                shootAS = gameObject.AddComponent<AudioSource>();
                shootAS.spatialBlend = 1.0f;
                shootAS.loop = false;
                shootAS.playOnAwake = false;
            }
        }

        public override bool Attack(Vector3 pos, float distance, ModularController self, ModularController target)
        {
            if(timer.GetComplete())
            {
                timer.StartTimer();
                
                return true;
            }
            return false;
        }

        protected Vector3 CalculateSpread(Vector3 pos, float distance)
        {
            float hitChance = HitChance(distance); // higher means less likely
            if (Random.Range(0f, 1f) > hitChance)
            {
                float fire = (weaponStats.weaponSpread - hitChance);
                // Miss!
                pos += new Vector3(Random.Range(-fire, fire), Random.Range(-fire, fire), Random.Range(-fire, fire));
            }
            return pos;
        }

        public override float GetRange(float distance)
        {
            return weaponStats.GetRange(distance);
        }

        public override float HitChance(float distance)
        {
            return weaponStats.hitChance(distance);
        }

        public override float GetActualRange()
        {
            return weaponStats.GetActualRange();
        }

        public override float GetDamageAtDistance(float distance)
        {
            return weaponStats.GetDamageAtDistance(distance);
        }
    }
}
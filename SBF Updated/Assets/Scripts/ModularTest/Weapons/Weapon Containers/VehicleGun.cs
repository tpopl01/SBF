using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Weapons
{
    public class VehicleGun : Weapon
    {
    //    Bullet1[] b;

        private void Start()
        {
            Initialise();
        }

        public override void Initialise()
        {
            base.Initialise();
            bulletSpawn = GetComponentsInChildren<Bullet1>();
        }

        public override bool Attack(Vector3 pos, float distance, ModularController self, ModularController target)
        {
            if (base.Attack(pos,distance, self, target))
            {
                if (bulletSpawn == null) bulletSpawn = transform.GetComponentsInChildren<Bullet1>();
                if (distance == 0) distance = Vector3.Distance(bulletSpawn[0].transform.position, pos);
                //  ai.shotAtFrom = transform.position;
                return Shoot(CalculateSpread(pos, distance), self, distance, target);
            }

            return false;
        }

        private bool Shoot(Vector3 pos, ModularController self, float distance, ModularController target)
        {
            //instantiate bullet towards target
         //   float d = Vector3.Distance(pos,(transform.position));
            for (int i = 0; i < bulletSpawn.Length; i++)
            {
                weaponStats.PlayShoot(shootAS);
                bulletSpawn[i].transform.LookAt(pos);
                bulletSpawn[i].Fire(GetDamageAtDistance(GetRange(distance)), distance, pos, self, target);
                weaponStats.PlayShoot(shootAS);
            }
          //  for (int i = 0; i < b.Length; i++)
          //  {
             //   b[i].transform.LookAt(pos);
              //  b[i].Fire(weaponStats.GetDamageAtDistance(d), h, d);
                weaponStats.PlayShoot(shootAS);
          //  }
            return false;
        }
    }
}
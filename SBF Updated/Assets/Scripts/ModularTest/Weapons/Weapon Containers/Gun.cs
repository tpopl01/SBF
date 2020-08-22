using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

namespace tpopl001.Weapons {
    public class Gun : WeaponUnit
    {
        [SerializeField]
        protected Timer reloadTimer;
        protected int ammoInClip;
        protected int totalAmmo;
      //  AnimationHandlerUnit anims;
        Animator anim3;
        //will be moved to ammo class
        //  private Timer timeBetweenShots;
       // Bullet1 b;
        protected bool reloading;
        protected const float reloadTime = 3.3f;

        public bool DisableIK { get; private set; } = false;

        public override bool Attack(Vector3 pos, float distance, ModularController self, ModularController target)
        {
            if (base.Attack(pos, distance, self, target))
            {
                if (CanAttack())
                {
                    if (bulletSpawn == null) bulletSpawn = transform.GetComponentsInChildren<Bullet1>();
                    if (distance == 0) distance = Vector3.Distance(bulletSpawn[0].transform.position, pos);
                    //return Shoot(CalculateSpread(pos, GetRange(distance)), distance, targetHealth);
                    return Shoot(CalculateSpread(pos, GetRange(distance)), distance, self, target);
                }
                else
                {
                    Reload();
                }
                //  else
                //  {

                //  }
            }
            return false;
        }

        private bool Shoot(Vector3 pos, float distance, ModularController self, ModularController target)
        {
            if(Shoot())
            {
                //if(b == null)
                //   b = GetComponentInChildren<Bullet1>();
                // Bullet1 b = Instantiate<Bullet1>(Resources.Load<Bullet1>("Bullets/" + GetGunStats().GetAmmoSlug()), bulletSpawn.position, bulletSpawn.rotation);
                bool attack = false;
                for (int i = 0; i < bulletSpawn.Length; i++)
                {
                    weaponStats.PlayShoot(shootAS);
                   // bulletSpawn[i].transform.LookAt(pos);
                    if (bulletSpawn[i].CanAttack())
                    {
                        bulletSpawn[i].Fire(GetDamageAtDistance(GetRange(distance)), distance, pos, self, target);
                        attack = true;
                    }
                }
                return attack;
            }
            return false;
        }

        public override bool CanAttack()
        {
            return reloadTimer.GetComplete() && (ammoInClip > 0);
        }

        public bool Reloading()
        {
            return reloading;
        }

        public void SetAmmo(int ammoInClip, int totalAmmo)
        {
            this.ammoInClip = ammoInClip;
            this.totalAmmo = totalAmmo;
        }

        public int GetAmmoInClip()
        {
            return ammoInClip;
        }

        public int GetAmmoTotal()
        {
            return totalAmmo;
        }

        protected GunStats GetGunStats()
        {
            return (GunStats)weaponStats;
        }

        public override void Initialise()
        {
            base.Initialise();
            reloadTimer = new Timer(GetGunStats().GetReloadTime() * reloadTime);
            ammoInClip = GetGunStats().GetMaxAmmoInClip();
            totalAmmo = GetGunStats().GetMaxAmmo();
            //  b = GetComponentInChildren<Bullet1>();
            bulletSpawn = GetComponentsInChildren<Bullet1>();
           // anims = GetComponentInParent<AnimationHandlerUnit>();
            WeaponSystem w = GetComponentInParent<WeaponSystem>();
            if (w) {
                anim3 = w.GetComponentInChildren<Animator>();
            //    ik = w.GetComponentInChildren<IK>();
            }
            //  timeBetweenShots = new Timer(GetGunStats().GetTimeBetweenShots());
            // timeBetweenShots.StartTimer();
        }

        private void Update()
        {
            if(reloading && reloadTimer.GetComplete())
            {
             //   if(anims)
                 //   anims.EnableIK();
                DisableIK = false;
                reloading = false;
            }
            Tick();
        }

        protected virtual void Tick()
        {

        }

        private bool Shoot()
        {
            if (ammoInClip != 0)
            {
                ammoInClip--;
                return true;
            }
            return false;
        }

        public override bool IsTwoHanded()
        {
            return GetGunStats().IsTwoHanded();
        }

        public override Vector3 GetRHAimTarget()
        {
            return GetGunStats().GetRHAimTarget();
        }
        public override Vector3 GetRHAimRot()
        {
            return GetGunStats().GetRHAimRot();
        }
        public override Vector3 GetLHAimTarget()
        {
            return GetGunStats().GetLHAimTarget();
        }
        public override Vector3 GetLHAimRot()
        {
            return GetGunStats().GetLHAimRot();
        }

        public override bool Reload()
        {
            if (reloadTimer.GetComplete())
            {
                if (totalAmmo > 0 && ammoInClip < GetGunStats().GetMaxAmmoInClip())
                {
                    if(anim3)
                    {
                        anim3.SetFloat("ReloadSpeed", 1 / GetGunStats().GetReloadTime());
                        anim3.Play("Reload");
                        DisableIK = true;
                    }
                    else
                    {
                     //   anims.SetReloadSpeed(1 / GetGunStats().GetReloadTime());
                      //  anims.DisableIK("Reload");
                    }
                    
                    reloading = true;
                    GetGunStats().PlayReload(shootAS);
                    int ammoToAdd = GetGunStats().GetMaxAmmoInClip() - ammoInClip;
                    totalAmmo -= ammoToAdd;
                    if (totalAmmo < 0)
                    {
                        ammoToAdd -= totalAmmo;
                        totalAmmo = 0;
                    }
                    ammoInClip += ammoToAdd;
                    reloadTimer.StartTimer();
                    return true;
                }
            }
            return false;
        }

        private bool AddAmmo(float percent)
        {
            int amountToAdd = (int)StaticMaths.CalculatePercent(percent, GetGunStats().GetMaxAmmo());
            if (amountToAdd == 0)
                amountToAdd = 1;
            totalAmmo += amountToAdd;
            if (totalAmmo > GetGunStats().GetMaxAmmo()) totalAmmo = GetGunStats().GetMaxAmmo();
            ammoInClip += amountToAdd;
            if (ammoInClip > GetGunStats().GetMaxAmmoInClip()) ammoInClip = GetGunStats().GetMaxAmmoInClip();
            return ammoInClip < GetGunStats().GetMaxAmmoInClip() || totalAmmo < GetGunStats().GetMaxAmmo();
        }

        public override float RemainingAmmoPercent()
        {
            return StaticMaths.CalculateNormalisedPercent(totalAmmo, GetGunStats().GetMaxAmmo());
        }

        public override float RemainingAmmoInClipPercent()
        {
            return StaticMaths.CalculateNormalisedPercent(ammoInClip, GetGunStats().GetMaxAmmoInClip());
        }

        public override bool AddAmmo()
        {
           return AddAmmo(10);
        }

        public override bool HasAmmo()
        {
            return totalAmmo > 0;
        }

        public override Vector3 GetRHTarget()
        {
            return GetGunStats().GetRHTarget();
        }

        public override Vector3 GetLHTarget()
        {
            return GetGunStats().GetLHTarget();
        }

        public override Vector3 GetLHOffhandTarget()
        {
            return GetGunStats().GetLHOffhandTarget();
        }

        public override Vector3 GetLHOffhandRot()
        {
            return GetGunStats().GetLHOffhandRot();
        }

        public override Vector3 GetRHRot()
        {
            return GetGunStats().GetRHRot();
        }

        public override Vector3 GetLHRot()
        {
            return GetGunStats().GetLHRot();
        }

        public override bool GetReloading()
        {
            return reloading;
        }

        public override void Respawn()
        {
            ammoInClip = GetGunStats().GetMaxAmmoInClip();
            totalAmmo = GetGunStats().GetMaxAmmo();
        }
    }
}
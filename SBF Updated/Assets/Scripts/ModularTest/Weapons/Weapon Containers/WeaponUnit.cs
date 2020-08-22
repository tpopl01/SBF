using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Weapons
{
    public abstract class WeaponUnit : Weapon
    {
        public string pickUpSlug { get; protected set; }

        public override void Initialise()
        {
            base.Initialise();
            pickUpSlug = StaticMaths.ProcessObjectName(name) + "_pickable";
        }

        public abstract void Respawn();

        public abstract bool AddAmmo();

        public abstract float RemainingAmmoInClipPercent();
        public abstract float RemainingAmmoPercent();

        public abstract bool HasAmmo();

        public void RemoveWeapon(bool createPickable = true)
        {
            if (createPickable)
            {
                CreatePickable();
            }
            Destroy(this.gameObject);
        }

        public void CreatePickable()
        {
            PickableWeapon w = Instantiate(Resources.Load<PickableWeapon>("Weapons/" + pickUpSlug), transform.position, transform.rotation);
            w.transform.parent = GameManagerModular.instance.weaponsFolder;
        }

        public abstract Vector3 GetRHAimTarget();
        public abstract Vector3 GetLHAimTarget();
        public abstract Vector3 GetRHAimRot();
        public abstract Vector3 GetLHAimRot();
        public abstract Vector3 GetRHTarget();
        public abstract Vector3 GetLHTarget();
        public abstract Vector3 GetRHRot();
        public abstract Vector3 GetLHRot();
        public abstract Vector3 GetLHOffhandRot();
        public abstract Vector3 GetLHOffhandTarget();
        public abstract bool IsTwoHanded();
        public abstract bool Reload();
        public abstract bool CanAttack();
        public abstract bool GetReloading();

        public bool IsFist { get; set; }
    }
}
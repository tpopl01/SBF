using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Weapons
{
    [CreateAssetMenu
    (
        fileName = "Gun Stats",
        menuName = "Weapon/WeaponStats/Gun Stats"
    )]
    public class GunStats : WeaponStats
    {
        [SerializeField] AudioProfileGeneral reloadProfile;
        [SerializeField] private bool twoHanded = false;
        [SerializeField][Range(0.5f, 20)] private float reloadTime = 1;
        [Space]
        [Header("Ammo")]
        [SerializeField] private string ammoSlug = "Bullet_test";
        [SerializeField] [Range(1, 100)] private int maxClipAmmo = 20;
        [SerializeField] [Range(5, 500)] private int maxAmmo = 200;

        [Space]
        [Header("Right Hand")]
        [Header("Aiming")]
        [Space]
        [Header("IK")]
        [SerializeField] private Vector3 rHAimTarget = Vector3.zero;
        [SerializeField] private Vector3 rHAimRot = Vector3.zero;
        [Header("Left Hand")]
        [SerializeField] private Vector3 lHAimTarget = Vector3.zero;
        [SerializeField] private Vector3 lHAimRot = Vector3.zero;

        [Space]
        [Header("Right Hand")]
        [Space]
        [Header("Idle")]
        [SerializeField] private Vector3 rHTarget = Vector3.zero;
        [SerializeField] private Vector3 rHRot = Vector3.zero;
        [Header("Left Hand")]
        [SerializeField] private Vector3 lHTarget = Vector3.zero;
        [SerializeField] private Vector3 lHRot = Vector3.zero;
        [SerializeField] private Vector3 lHOffhandTarget = Vector3.zero; 
        [SerializeField] private Vector3 lHOffhandRot = Vector3.zero;

        [Space]
        [Header("First Person")]
        [SerializeField] private Vector3 aimfpPos = Vector3.zero;
        [SerializeField] private Vector3 aimfpRot = Vector3.zero;
        [SerializeField] private Vector3 fpPos = Vector3.zero;
        [SerializeField] private Vector3 fpRot = Vector3.zero;
        [SerializeField][Range(0.1f, 10)] private float aimSpeed = 5;




        public override void Init()
        {
            base.Init();
        }

        public string GetAmmoSlug()
        {
            return ammoSlug;
        }

        public Vector3 GetRHTarget()
        {
            return rHTarget;
        }
        public Vector3 GetLHTarget()
        {
            return lHTarget;
        }
        public Vector3 GetLHOffhandTarget()
        {
            return lHOffhandTarget;
        }
        public Vector3 GetLHOffhandRot()
        {
            return lHOffhandRot;
        }
        public Vector3 GetRHRot()
        {
            return rHRot;
        }
        public Vector3 GetLHRot()
        {
            return lHRot;
        }

        public Vector3 GetRHAimTarget()
        {
            return rHAimTarget;
        }
        public Vector3 GetLHAimTarget()
        {
            return lHAimTarget;
        }
        public Vector3 GetRHAimRot()
        {
            return rHAimRot;
        }
        public Vector3 GetLHAimRot()
        {
            return lHAimRot;
        }

        public bool IsTwoHanded()
        {
            return twoHanded;
        }
        public float GetReloadTime()
        {
            return reloadTime;
        }

        public int GetMaxAmmo()
        {
            return maxAmmo;
        }

        public int GetMaxAmmoInClip()
        {
            return maxClipAmmo;
        }

        public Vector3 GetFpPos()
        {
            return fpPos;
        }
        public Vector3 GetFpRot()
        {
            return fpRot;
        }
        public Vector3 GetFPAimRot()
        {
            return aimfpRot;
        }
        public Vector3 GetFPAimPos()
        {
            return aimfpPos;
        }
        public float GetAimSpeed()
        {
            return aimSpeed;
        }

        public void PlayReload(AudioSource aS)
        {
            reloadProfile.PlaySound(aS);
        }

    }
}
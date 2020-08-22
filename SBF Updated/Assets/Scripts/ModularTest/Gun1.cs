using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using tpopl001.Weapons;
using UnityEngine;

public class Gun1 : MonoBehaviour, IWeapons
{
    Bullet1 b;
    [SerializeField] protected WeaponStats weaponStats;
    protected Timer timer;
    [SerializeField] protected Transform[] bulletSpawn;
    protected AudioSource shootAS;

    public bool Fire1()
    {
        return Attack();
    }

    public bool Reload()
    {
        return Reload1();
    }

    protected virtual bool Reload1()
    {
        return false;
    }

    protected virtual bool Attack()
    {
        if (b == null)
            b = GetComponentInChildren<Bullet1>();

      //  weaponStats.PlayShoot(shootAS);
     //   b.transform.LookAt(pos);
      //  b.Fire(GetDamageAtDistance(GetRange(distance)), h, distance);

        return false;
    }

    public bool Drop()
    {
        throw new System.NotImplementedException();
    }

    public float GetAmmoPercent()
    {
        throw new System.NotImplementedException();
    }

    public float GetAmmoClipPercent()
    {
        throw new System.NotImplementedException();
    }

    public bool HasAmmoInGun()
    {
        throw new System.NotImplementedException();
    }

    public bool IsTwoHanded()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetIKTarget(bool aiming, out Vector3 RHPos, Vector3 RHRot, Vector3 LHPos, Vector3 LHRot)
    {
        throw new System.NotImplementedException();
    }

    public bool AddAmmo()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        Init();
    }

    protected virtual void Init()
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
}

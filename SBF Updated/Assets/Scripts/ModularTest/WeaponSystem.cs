using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

public class WeaponSystem : WeaponSystemBase, ITick, IRespawn, IDeath, IDisableIK, IWeaponSystomChangeable, IGetDisableIK, IAmmoAdd, IAiming
{
    [Header("Weapon Slots")]
    [SerializeField] protected HandSlot[] currentWeapons = new HandSlot[0];
    [SerializeField] protected HolsterSlot[] holsters = new HolsterSlot[0];

    [Space]
    [Header("Default")]
    public string[] defaultWeaponSlug = { "SE-14_sidearm" };
    string[] all = { "SE-14_sidearm", "PLX-1", "E5", "E-60R", "droid_blaster", "DC-15x", "DC-15_sidearm", "DC-15", "ACR" };

    List<WeaponUnit> addOverTime = new List<WeaponUnit>();

    protected IK iK;
   // StatsHolder statsHolder;
    bool death;
    bool useIK = true;
    protected bool aiming;
    Animator anim;

    protected ISpecial[] specials;


    public void EquipHolster()
    {
        for (int n = holsters.Length-1; n > -1; n--)
        {
            if (holsters[n] == null)
                continue;
            if (holsters[n].Weapon == null)
                continue;
            WeaponUnit w = holsters[n].Weapon;
            holsters[n].RemoveWeapon();
            Equip(w);
        }
    }


    public override void SetUp(Transform root)
    {
        //ModularController c = root.GetComponent<ModularController>();
      //  string s = "SE-14_sidearm";
      //  if (c.Team == 0) s = "DC-15_sidearm";
     // if(defaultWeaponSlug.Length<2)
       // defaultWeaponSlug = new string[] { all[Random.Range(0,all.Length-1)], all[Random.Range(0, all.Length - 1)] };
        anim = root.GetComponentInChildren<Animator>();
        //statsHolder = GetComponent<StatsHolder>();
        if (holsters.Length == 0 || currentWeapons.Length == 0)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            if (holsters.Length == 0)
            {
                List<Transform> h = new List<Transform>();
                for (int i = children.Length - 1; i > -1; i--)
                {
                    if (children[i].name.Contains("Holster"))
                    {
                        h.Add(children[i]);
                    }
                }
                holsters = new HolsterSlot[h.Count];
                for (int i = 0; i < holsters.Length; i++)
                {
                    holsters[i] = new HolsterSlot(h[i]);
                }
            }
            if (currentWeapons.Length == 0)
            {
                List<Transform> currentWeaps = new List<Transform>();
                for (int i = children.Length - 1; i > -1; i--)
                {
                    if (children[i].name.Contains("Weapon Holder"))
                    {
                        currentWeaps.Add(children[i]);
                    }
                }
                currentWeapons = new HandSlot[currentWeaps.Count];
                for (int i = 0; i < currentWeapons.Length; i++)
                {
                    currentWeapons[i] = new HandSlot(currentWeaps[i]);
                }
            }
        }

        specials = GetComponentsInChildren<ISpecial>();
        //Respawn();

        for (int i = 0; i < defaultWeaponSlug.Length; i++)
        {
            InstantiateWeapon(defaultWeaponSlug[i]);
        }

        iK = GetComponentInChildren<IK>();
        iK.Init();
    }

    bool gunsDisabled = false;
    public void DisableGuns()
    {
        
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            gunsDisabled = true;
            if (currentWeapons[i].Weapon)
                currentWeapons[i].Weapon.gameObject.SetActive(false);
        }
    }
    public void EnableGuns()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            gunsDisabled = false;
            if (currentWeapons[i].Weapon)
                currentWeapons[i].Weapon.gameObject.SetActive(true);
        }
    }

    public bool UseSpecialOfType(SpecialType specialType)
    {
        for (int i = 0; i < specials.Length; i++)
        {
            if (specials[i].GetSpecialType() == specialType)
            {
                if(specials[i].Use())
                    return true;
            }
        }
        return false;
    }

    public ISpecial GetSpecialOfType(SpecialType specialType)
    {
        for (int i = 0; i < specials.Length; i++)
        {
            if (specials[i].GetSpecialType() == specialType)
            {
                return specials[i];
            }
        }
        return null;
    }

    private void CreatePickables()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if(currentWeapons[i].Weapon && !currentWeapons[i].offhand)
            {
                currentWeapons[i].Weapon.CreatePickable();
            }
        }
    }

    private void DropAll()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (!currentWeapons[i].offhand)
                Drop(currentWeapons[i].Weapon);
        }
        for (int i = 0; i < holsters.Length; i++)
        {
            Drop(holsters[i].Weapon);
        }
    }

    public bool Respawn()
    {
        death = false;
        //if (currentWeapons != null && currentWeapons.Length > 0)
        //    for (int i = 0; i < defaultWeaponSlug.Length; i++)
        //    {
        //        InstantiateWeapon(defaultWeaponSlug[i]);
        //    }
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                currentWeapons[i].Respawn();
        }
        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].Weapon)
                holsters[i].Respawn();
        }
        for (int i = 0; i < specials.Length; i++)
        {
            specials[i].Respawn();
        }
        return true;
    }

    //public override bool Attack(Vector3 target, ModularController self, ModularController targetAgent)
    //{
    //    base.Attack(target, self, targetAgent);
    //    bool attacking = false;
    //    Weapon[] currentWeapons = GetCurrentWeapons();
    //    for (int i = 0; i < currentWeapons.Length; i++)
    //    {
    //        if (currentWeapons[i].Attack(target, 0, self, targetAgent))
    //        {
    //            attacking = true;
    //        }
    //    }
    //    return attacking;
    //}

    void SetAnim()
    {
        if (!useIK || death || gunsDisabled)
            return;

        WeaponUnit weap = currentWeapons[0].Weapon;
        if (currentWeapons[1].offhand)
        {
            anim.SetInteger("WeaponType", 0);
            //(GetAnimation()).SetWeaponType(0);
        }
        if (weap)
        {
            if (weap.IsTwoHanded() == false)
            {
                anim.SetInteger("WeaponType", 1);
               // (GetAnimation()).SetWeaponType(1);
            }
        }
        SetIK();
    }

    void SetIK()
    {
        WeaponUnit weap = currentWeapons[0].Weapon;
        if (currentWeapons[1].offhand)
        {
            iK.EnableShoulderIK();
            iK.RightHandTarget(weap.GetRHAimTarget(), weap.GetRHAimRot());
            iK.EnableRHIK();

            if (!(currentWeapons[1].Weapon && !currentWeapons[1].offhand))
            {
                iK.OffHandLHWeap(currentWeapons[0].Weapon.GetLHOffhandTarget(), weap.GetLHOffhandRot());
                iK.EnableLHIK();
            }
            //  Transform weap1 = currentWeapons[1].Weapon.transform;
            // iK.EnableLHIK();
            // if (aiming)
            // iK.OffhandLeftHandTarget(
            //    weap1.position + weap1.right * currentWeapons[1].Weapon.GetLHAimTarget().x + weap1.up * currentWeapons[1].Weapon.GetLHAimTarget().y + weap1.forward * currentWeapons[1].Weapon.GetLHAimTarget().z,
            // currentWeapons[1].Weapon.transform.position + currentWeapons[1].Weapon.GetLHAimTarget(),
            //    currentWeapons[1].Weapon.transform.eulerAngles + currentWeapons[1].Weapon.GetLHAimRot()
            //  );
            //   else
            //  {
            //iK.OffhandLeftHandTarget(
            //    weap1.position + weap1.right * currentWeapons[1].Weapon.GetLHTarget().x + weap1.up * currentWeapons[1].Weapon.GetLHTarget().y + weap1.forward * currentWeapons[1].Weapon.GetLHTarget().z,
            //    weap1.eulerAngles + currentWeapons[1].Weapon.GetLHRot()
            // );
            //  }
            //  anim.SetBool("LHWeapon", true);
            //   iK.TwoHandedWeapon();
        }
        else if (currentWeapons[1].Weapon)
        {
            iK.EnableLHIK();
            if (aiming)
                iK.LeftHandTarget(currentWeapons[1].Weapon.GetLHAimTarget(), currentWeapons[1].Weapon.GetLHAimRot());
            else
                iK.LeftHandTarget(currentWeapons[1].Weapon.GetLHTarget(), currentWeapons[1].Weapon.GetLHRot());
            anim.SetBool("LHWeapon", true);
        }
        else
        {
            anim.SetBool("LHWeapon", false);
        }
        if (weap)
        {
            iK.EnableShoulderIK();
            if (!(currentWeapons[1].Weapon && currentWeapons[1].offhand))
                iK.EnableRHIK();
            if (aiming)
            {
                iK.RightHandTarget(weap.GetRHAimTarget(), weap.GetRHAimRot());
                iK.EnableRHIK();

                if (!(currentWeapons[1].Weapon && !currentWeapons[1].offhand))
                {
                    iK.OffHandLHWeap(currentWeapons[0].Weapon.GetLHOffhandTarget(), weap.GetLHOffhandRot());
                    iK.EnableLHIK();
                }
            }
            else
            {
                iK.RightHandTarget(weap.GetRHTarget(), weap.GetRHRot());

                if (!(currentWeapons[1].Weapon && !currentWeapons[1].offhand))
                {
                    iK.OffHandLHWeap(currentWeapons[0].Weapon.GetLHOffhandTarget(), weap.GetLHOffhandRot());
                    iK.EnableLHIK();
                }
            }
        }
        if (aiming) iK.EnableAimIK();
    }

    public bool AddWeapon(string weap)
    {
        bool retVal = CanPickup();
        if (retVal)
        {
            InstantiateWeapon(weap);
        }
        return retVal;
    }

    private void InstantiateWeapon(WeaponUnit weap)
    {
        if (weap.IsTwoHanded())
        {
            if (currentWeapons[0].Weapon || currentWeapons[1].Weapon)
            {
                //Holster/drop both weapons in hands
                for (int i = 0; i < 2; i++)
                {
                    if (currentWeapons[i].Weapon == null)
                        continue;

                    if (currentWeapons[i].offhand)
                    {
                        currentWeapons[i].RemoveOffhand();
                        continue;
                    }

                    if (!Holster(currentWeapons[i].Weapon))
                    {
                        //Drop
                        Drop(currentWeapons[i].Weapon);
                    }
                    else
                    {
                        currentWeapons[i].RemoveOffhand();
                    }
                }
                addOverTime.Add(weap);
            }
            else
            {
                currentWeapons[0].Equip(weap);
                currentWeapons[1].Offhand(weap);
                weap.Initialise();
            }
        }
        else
        {
            //check for a free hand
            for (int i = 0; i < currentWeapons.Length; i++)
            {
                if (currentWeapons[i].Equip(weap))
                {
                    weap.Initialise();
                    return;
                }
            }
            //if no free hand attempt to holster
            if (Holster(currentWeapons[0].Weapon))
            {
                if (currentWeapons[1].offhand) currentWeapons[1].RemoveOffhand();
                currentWeapons[0].RemoveWeapon();
                currentWeapons[0].Equip(weap);
                weap.Initialise();
                return;
            }
            //still no free weapon then drop
            Drop(currentWeapons[0].Weapon);
            currentWeapons[0].Equip(weap);
            weap.Initialise();
        }
        //  EquipFistsIfNull();
    }

    private void InstantiateWeapon(string slug)
    {
        WeaponUnit weap = Instantiate(Resources.Load<WeaponUnit>("Weapons/" + slug));
        InstantiateWeapon(weap);
    }

    private void EquipFistsIfNull()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon == null)
            {
                currentWeapons[i].Equip(Instantiate(Resources.Load<WeaponUnit>("Weapons/Fist")));
                currentWeapons[i].Weapon.IsFist = true;
            }
        }
    }

    private bool Holster(WeaponUnit weapon)
    {
        if (weapon == null)
            return false;

        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].HosterWeapon(weapon))
            {
                return true;
            }
        }
        return false;
    }

    private void Drop(WeaponUnit weapon)
    {
        if (weapon == null)
            return;

        weapon.RemoveWeapon(true);
    }

    public bool CanAttack()
    {
        bool onlyFists = true;
        bool fistsReady = false;
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon == null) continue;
            if (currentWeapons[i].Weapon.CanAttack() && currentWeapons[i].Weapon.IsFist == false)
            {
                return true;
            }
            else if (currentWeapons[i].Weapon.CanAttack() && currentWeapons[i].Weapon.IsFist == true)
            {
                fistsReady = true;
            }
            if (currentWeapons[i].Weapon.IsFist == false)
            {
                onlyFists = false;
            }
        }
        return onlyFists && fistsReady;
    }

    public bool CanPickup()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon == null || currentWeapons[i].Weapon.IsFist)
            {
                return true;
            }
        }
        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].Weapon == null || holsters[i].Weapon.IsFist)
            {
                return true;
            }
        }
        return false;
    }

    public void EquipBestWeapon()
    {
        WeaponUnit best = currentWeapons[0].Weapon;
        int index = 0;
        bool foundBest = false;
        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].Weapon.GetDamageAtDistance(1) > best.GetDamageAtDistance(1))
            {
                best = holsters[i].Weapon;
                foundBest = true;
                index = i;
            }
        }
        if (foundBest)
        {
            holsters[index].RemoveWeapon();
            Equip(best);
        }
    }

    protected virtual void Equip(WeaponUnit weap)
    {
        bool tH = currentWeapons[0].Weapon.IsTwoHanded();
        if ((tH || weap.IsTwoHanded()) && !Holster(currentWeapons[0].Weapon))
        {
            Drop(currentWeapons[0].Weapon);
        }
        if (tH)
        {
            currentWeapons[1].RemoveOffhand();
        }

        if (weap.IsTwoHanded())
        {
            if(currentWeapons[1].Weapon)
            if (!Holster(currentWeapons[1].Weapon))
            {
                Drop(currentWeapons[1].Weapon);
            }
        }
        else if(!weap.IsTwoHanded() && !tH)
        {
            //equip as offhand
            if(!currentWeapons[1].Weapon)
            {
                currentWeapons[1].Equip(weap);
                return;
            }
        }
        if (currentWeapons[1].offhand) currentWeapons[1].RemoveOffhand();
        currentWeapons[0].RemoveWeapon();
        currentWeapons[0].Equip(weap);
        if (currentWeapons[0].Weapon.IsTwoHanded())
        {
            currentWeapons[1].Offhand(weap);

        }
       // weap.GetComponentInChildren<Bullet1>().SetStats(statsHolder.GetStats);
    }

    public virtual void Reload()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                currentWeapons[i].Weapon.Reload();
        }
    }



    public virtual float GetAmmoRemainingInGunPercent()
    {
        float amount = 0;
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                amount = currentWeapons[i].Weapon.RemainingAmmoInClipPercent();
        }
        return amount;
    }

    public virtual bool HasAmmo()
    {
        bool amount = false;
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                amount = currentWeapons[i].Weapon.HasAmmo();
        }
        return amount;
    }

    public override Weapon[] GetWeapons()
    {
        List<Weapon> weap = new List<Weapon>();
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon != null)
                weap.Add(currentWeapons[i].Weapon);
        }
        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].Weapon != null)
                weap.Add(holsters[i].Weapon);
        }
        return weap.ToArray();
    }

    public override Weapon[] GetCurrentWeapons()
    {
        List<Weapon> weap = new List<Weapon>();
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon != null)
                weap.Add(currentWeapons[i].Weapon);
        }
   //     Debug.Log(weap.Count);
        return weap.ToArray();
    }

    

    public virtual void Tick()
    {
        if (addOverTime.Count > 0)
        {
            InstantiateWeapon(addOverTime[0]);
            addOverTime.RemoveAt(0);
        }
        SetAnim();

        aiming = false;

    }

    public void Death()
    {
        if (death == false)
        {
            CreatePickables();
           // DropAll();
            death = true;
        }
    }

    public void DisableIK(bool disable)
    {
        useIK = !disable;
    }

    //public void SetAim(bool aiming)
    //{
    //    this.aiming = aiming;
    //}

    public bool GetDisableIK()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if(currentWeapons[i].Weapon)
            if(currentWeapons[i].Weapon.GetType() == typeof(Gun))
            {
                Gun g = (Gun)currentWeapons[i].Weapon;
                if (g.DisableIK)
                    return true;
            }
        }
        return false;
    }

    public virtual bool AddAmmo()
    {
        bool addAmmo = false;
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                if (currentWeapons[i].Weapon.AddAmmo()) addAmmo = true;
        }
        for (int i = 0; i < specials.Length; i++)
        {
            if (specials[i].AddAmmo()) addAmmo = true;
        }
        return addAmmo;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public bool GetAiming()
    {
        aiming = true;
        if (!iK.GetAiming()) return false;

        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (!currentWeapons[i].Weapon) continue;
            if (currentWeapons[i].Weapon.GetReloading()) return false;
        }
        return true;
    }
}

[System.Serializable]
public class HolsterSlot
{
    public Transform holsterSpot;
    public WeaponUnit Weapon { get; private set; } = null;

    public HolsterSlot(Transform holsterSpot)
    {
        this.holsterSpot = holsterSpot;
    }

    public void RemoveWeapon()
    {
        Weapon = null;
    }

    public bool HosterWeapon(WeaponUnit weapon)
    {
        if (weapon != null)
            if (weapon.IsFist) return false;
        if (this.Weapon == null)
        {
            this.Weapon = weapon;
            this.Weapon.transform.position = holsterSpot.position;
            this.Weapon.transform.rotation = holsterSpot.rotation;
            this.Weapon.transform.SetParent(holsterSpot.transform);
            return true;
        }
        return false;
    }
    public void Respawn()
    {
        Weapon.Respawn();
    }
}

[System.Serializable]
public class HandSlot
{
    public Transform hand;
    public WeaponUnit Weapon { get; private set; } = null;
    public bool offhand { get; private set; } = false;

    public HandSlot(Transform hand)
    {
        this.hand = hand;
    }

    public bool Equip(WeaponUnit weapon)
    {
        if (this.Weapon == null)
        {
            if (weapon == null)
            {
                Debug.Log("Shouldnt happen");
                return false;
            }
            offhand = false;
            this.Weapon = weapon;
            this.Weapon.transform.position = hand.position;
            this.Weapon.transform.rotation = hand.rotation;
            this.Weapon.transform.SetParent(hand.transform);
            return true;
        }
        return false;
    }

    public bool Equip(WeaponUnit weapon, bool fist)
    {
        weapon.IsFist = fist;
        return Equip(weapon);
    }

    public void RemoveOffhand()
    {
        Weapon = null;
        offhand = false;
    }

    public void Offhand(WeaponUnit weapon)
    {
        Weapon = weapon;
        offhand = true;
    }

    public void RemoveWeapon()
    {
        Weapon = null;
        if (offhand)
        {
            RemoveOffhand();
        }
    }

    public void Respawn()
    {
        Weapon.Respawn();
    }
}
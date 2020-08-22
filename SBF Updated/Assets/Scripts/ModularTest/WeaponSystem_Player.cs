using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem_Player : WeaponSystem, IFirstPersonSwitch
{
    bool firstPerson;
    FPWeapon[] fPWeapons;
    FPWeapon currentFPWeapon;
    Text weaponText;
    Text ammoText;
    Text additionalText;
    Text additionalAmmoText;
    int specialIndex = 0;
    Transform camTrans;
    //  AISensors_Player ai;

    public override void SetUp(Transform root)
    {
        if (transform.childCount == 0) return;
        base.SetUp(root);
        camTrans = CameraManager.instance.GetComponentInChildren<Camera>().transform;
        fPWeapons = CameraManager.instance.GetComponentsInChildren<FPWeapon>();
        Transform r = GameObject.FindObjectOfType<Canvas>().transform.Find("Weapon");
        weaponText = r.transform.Find("Name").GetComponent<Text>();
        ammoText = r.transform.Find("Ammo").GetComponent<Text>();
        additionalText = r.transform.Find("Additional_name").GetComponent<Text>();
        additionalAmmoText = r.transform.Find("Additional_count").GetComponent<Text>();
        if (specials.Length > 0)
        {
            additionalText.text = specials[specialIndex].GetName();
            additionalAmmoText.text = specials[specialIndex].GetRemaining().ToString();
        }
        // ai = GetComponent<AISensors_Player>();
    }

    public void NextSpecial()
    {
        specialIndex++;
        if(specialIndex > specials.Length -1)
        {
            specialIndex = 0;
        }
        additionalText.text = specials[specialIndex].GetName();
        additionalAmmoText.text = specials[specialIndex].GetRemaining().ToString();
    }

    public void UseSpecial()
    {
        if (specials.Length > 0)
        {
            specials[specialIndex].Use();
            additionalText.text = specials[specialIndex].GetName();
            additionalAmmoText.text = specials[specialIndex].GetRemaining().ToString();
        }
    }

    public override void Tick()
    {
        base.Tick();
        if (firstPerson)
        {
            if (currentFPWeapon)
            {
                weaponText.text = currentFPWeapon.name;
                ammoText.text = currentFPWeapon.GetAmmoInClip() + " / " + currentFPWeapon.GetAmmoTotal();
            }
        }
        else
        {
            Gun g = (Gun)currentWeapons[0].Weapon;
            if (g)
            {
                weaponText.text = g.name;
                ammoText.text = g.GetAmmoInClip() + " / " + g.GetAmmoTotal();
            }
        }

        if (aiming)
            iK.LookPosition = camTrans.position + camTrans.forward * 100;// + camTrans.up * 0.5f + camTrans.right *0.5f;
     //   if (specials != null && specials.Length > 0)
 //       {
     //       additionalText.text = specials[specialIndex].name;
     //       additionalAmmoText.text = specials[specialIndex].Count.ToString();
      //  }
    }

    public void SetFirstPerson(bool firstPerson)
    {
        this.firstPerson = firstPerson;
        SetActiveWeapons(firstPerson);
    }

    public override bool Attack(Vector3 target, ModularController self, ModularController targetAgent)
    {
        if (firstPerson && currentFPWeapon != null)
        {
            if (currentFPWeapon.Attack(target, 0, self, targetAgent))
            {
                return true;
            }
            return false;
        }
        bool attacking = false;
        Weapon[] currentWeapons = GetCurrentWeapons();
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Attack(target, 0, self, targetAgent))
            {
                attacking = true;
            }
        }
        return attacking;
    }

    protected override void Equip(WeaponUnit weap)
    {
        base.Equip(weap);
        //  SetActiveWeapons(firstPerson);
        EnableFPWeapon(firstPerson);

    }

    void SetActiveWeapons(bool firstPerson)
    {
        for (int i = 0; i < holsters.Length; i++)
        {
            if (holsters[i].Weapon)
                holsters[i].Weapon.gameObject.SetActive(!firstPerson);
        }
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i].Weapon)
                currentWeapons[i].Weapon.gameObject.SetActive(!firstPerson);
        }


        EnableFPWeapon(firstPerson);
    }

    void EnableFPWeapon(bool enable)
    {
        for (int i = 0; i < fPWeapons.Length; i++)
        {
            fPWeapons[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            for (int n = 0; n < fPWeapons.Length; n++)
            {
                if (currentWeapons[i].Weapon == null)
                    return;
                if (currentWeapons[i].Weapon.pickUpSlug.Equals(fPWeapons[n].pickUpSlug))
                {
                    fPWeapons[n].gameObject.SetActive(enable);
                    currentFPWeapon = fPWeapons[n];
                    Gun weap = (Gun)currentWeapons[i].Weapon;
                    if (weap)
                    {
                        if (enable)
                        {
                            weap.SetAmmo(currentFPWeapon.GetAmmoInClip(), currentFPWeapon.GetAmmoTotal());
                        }
                        else
                        {
                            currentFPWeapon.SetAmmo(weap.GetAmmoInClip(), weap.GetAmmoTotal());
                        }
                    }
                }
            }
        }
    }

    public override bool AddAmmo()
    {
        if (currentFPWeapon && firstPerson)
            return currentFPWeapon.AddAmmo();
        bool a = base.AddAmmo();
        Gun g = (Gun)currentWeapons[0].Weapon;
        if (g)
        {
            weaponText.text = g.name;
            ammoText.text = g.GetAmmoInClip() + " / " + g.GetAmmoTotal();
        }
        return a;
    }

    public override float GetAmmoRemainingInGunPercent()
    {
        if (firstPerson)
        {
            if (currentFPWeapon)
                return currentFPWeapon.RemainingAmmoInClipPercent();
        }
        return base.GetAmmoRemainingInGunPercent();
    }

    public override bool HasAmmo()
    {
        if (firstPerson)
        {
            if (currentFPWeapon)
                return currentFPWeapon.HasAmmo();
        }
        return base.HasAmmo();
    }

    public override void Reload()
    {
        if (firstPerson && currentFPWeapon)
        {
            Debug.Log(currentFPWeapon.Reload());
            return;
        }
        base.Reload();
    }

    //public void NextSpecial()
    //{
    //    specialIndex++;
    //    if (specialIndex >= specials.Length)
    //    {
    //        specialIndex = 0;
    //    }
    //}

    //public void PrevSpecial()
    //{
    //    specialIndex--;
    //    if (specialIndex < 0)
    //    {
    //        specialIndex = specials.Length - 1;
    //    }
    //}

    //public void UseSpecial(AISensorsBase ai)
    //{
    //    if (specials == null)
    //        return;
    //    if (specials.Length == 0)
    //        return;
    //    specials[specialIndex].Use(ai);
    //}
}

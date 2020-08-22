using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

public interface IWeaponSystomChangeable
{
    void EquipHolster();
    void DisableGuns();
    void EnableGuns();
    bool AddWeapon(string weap);
    bool CanAttack();
    bool CanPickup();
    void EquipBestWeapon();
    void Reload();
    float GetAmmoRemainingInGunPercent();
    bool HasAmmo();
    ISpecial GetSpecialOfType(SpecialType specialType);
    bool UseSpecialOfType(SpecialType specialType);
}

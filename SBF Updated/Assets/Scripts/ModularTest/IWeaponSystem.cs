using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

public interface IWeaponSystem
{
    bool Attack(Vector3 target, ModularController self, ModularController targetAgent);
    Weapon[] GetCurrentWeapons();

    Weapon[] GetWeapons();

    float hitChance(float distance);

    float Range(float distance);

    float GetActualRange();

    float GetDamageAtDistance(float distance);
}

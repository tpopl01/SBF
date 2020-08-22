using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace tpopl001.Weapons
{
    [CreateAssetMenu
    (
        fileName = "Vehicle Stats",
        menuName = "Weapon/WeaponStats/Vehicle Stats"
    )]
    public class VehicleGunStats : WeaponStats
    {
        [SerializeField] string bulletSlug;
    }
}
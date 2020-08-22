using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapons
{
    bool Fire1();
    bool Reload();
    bool Drop();
    float GetAmmoPercent();
    float GetAmmoClipPercent();
    bool HasAmmoInGun();
    bool IsTwoHanded();
    Vector3 GetIKTarget(bool aiming, out Vector3 RHPos, Vector3 RHRot, Vector3 LHPos, Vector3 LHRot);
    bool AddAmmo();

}

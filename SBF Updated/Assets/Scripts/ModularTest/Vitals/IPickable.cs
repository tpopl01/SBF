using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    Vector3 Position();
    bool Pickup(IWeaponSystomChangeable w, Vector3 pos);
}

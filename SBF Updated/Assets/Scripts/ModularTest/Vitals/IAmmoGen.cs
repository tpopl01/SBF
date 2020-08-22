using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmmoGen 
{
    void AddAmmo(IAmmoAdd a);
    Vector3 Position();
}

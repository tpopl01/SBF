using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectWeapon
{
    Vector3 Position();
    bool AddWeapon(string weapon);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealer
{
    bool Heal(IHealable healable);
    Vector3 Position();
}

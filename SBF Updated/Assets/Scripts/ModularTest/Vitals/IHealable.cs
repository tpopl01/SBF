using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    bool Heal();
    bool NeedsHealing(int team);
    Vector3 Position();
}

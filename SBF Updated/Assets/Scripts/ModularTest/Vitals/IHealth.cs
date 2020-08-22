using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float GetHP();
    float GetMaxHP();
    float GetHPPercent();
    bool IsDead();
    void ForceKill();
    bool DamageHealth(float amount);
}

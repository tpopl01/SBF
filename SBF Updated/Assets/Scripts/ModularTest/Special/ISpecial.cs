using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial 
{
    bool AddAmmo();
    void Respawn();
    bool Use();
    SpecialType GetSpecialType();
    int GetRemaining();
    string GetName();
}

public enum SpecialType
{
    Combat,
    Movement,
    Repair,
    Heal
}
public struct LaunchData
{
    public readonly Vector3 initialVelocity;
    public readonly float timeToTarget;

    public LaunchData(Vector3 initialVelocity, float timeToTarget)
    {
        this.initialVelocity = initialVelocity;
        this.timeToTarget = timeToTarget;
    }

}
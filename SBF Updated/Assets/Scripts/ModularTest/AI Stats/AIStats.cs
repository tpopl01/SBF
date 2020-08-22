using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
(
    fileName = "AI Stats",
    menuName = "AI/AIStats/AI Stats"
)]
public class AIStats : ScriptableObject
{
    [Header("Speed")]
    [SerializeField] [Range(0, 60)] float runSpeed = 3;
    [SerializeField] [Range(0, 60)] float sprintSpeed = 5;
    [SerializeField] [Range(0, 60)] float walkSpeed = 1;
    [SerializeField] [Range(0, 5)] float turnSpeed = 1;
    [Space]
    [Header("Health")]
    [SerializeField] [Range(10, 1000)] float maxHealth = 100;
    [SerializeField] [Range(0.1f, 10)] private float healingSpeed = 0.1f;
    [SerializeField] [Range(1f, 10)] private float armour = 1f;
    [Space]
    [Header("Perception")]
    [SerializeField] [Range(0, 180)] float vision = 70;
    [SerializeField] [Range(20, 100)] float perception = 60;

    public float GetHealingSpeed()
    {
        return healingSpeed;
    }

    public float GetVision()
    {
        return vision;
    }
    public float GetPerception()
    {
        return perception;
    }
    public float GetRunSpeed()
    {
        return runSpeed;
    }
    public float GetWalkSpeed()
    {
        return walkSpeed;
    }
    public float GetSprintSpeed()
    {
        return sprintSpeed;
    }
    public float GetTurnSpeed()
    {
        return turnSpeed;
    }
    public float GetMaxHP()
    {
        return maxHealth;
    }
    public float GetArmour()
    {
        return armour;
    }
}

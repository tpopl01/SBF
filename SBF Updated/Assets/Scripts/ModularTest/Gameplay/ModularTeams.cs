using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Modular_Team",
      menuName = "Modular/Teams/Modular_Team"
  )]
public class ModularTeams : ScriptableObject
{
    public List<ModularController> ActiveUnits { get; private set; } = new List<ModularController>();
    [SerializeField] int team = 0;
    [SerializeField]Color color;
    public int Score { get; private set; }

    public void Init()
    {
        ActiveUnits.Clear();
        Score = 0;
    }

    public void IncrementScore(int amount)
    {
        Score += amount;
    }

    public Color GetColour()
    {
        return color;
    }

    public bool IsUsersTeam(int team)
    {
        return this.team == team;
    }

    public int GetTeam()
    {
        return team;
    }

    //Debug public
    public void AddUnit(ModularController states)
    {
        if (states.Team != this.team)
            return;

        if (!ActiveUnits.Contains(states))
            ActiveUnits.Add(states);
    }

    public void RemoveUnit(ModularController states)
    {
        if (states.Team != this.team)
            return;

        ActiveUnits.Remove(states);
    }
}

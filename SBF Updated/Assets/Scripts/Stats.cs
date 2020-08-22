using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Stats",
      menuName = "Modular/Stats"
  )]
public class Stats : ScriptableObject
{
    public int Points { get; private set; }
    public int Kills { get; private set; }
    public int Deaths { get; private set; }
    public string Name { get; set; }
    public int Team { get; set; }

    //public Stats(string name, int team)
    //{
    //    this.Name = name;
    //    this.Team = team;
    //}

    public void OnKill()
    {
        Kills++;
        Points++;
    }

    public void OnPoints()
    {
        Points += 5;
    }

    public void OnDie()
    {
        Points--;
        Deaths++;
    }

}

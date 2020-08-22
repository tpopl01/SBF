using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Events;

[CreateAssetMenu
 (
     fileName = "Conquest",
     menuName = "GameMode/Conquest"
 )]
public class GameMode_Conquest : GameMode
{
    public override void Setup(string gameModeSlug, int maxScore, Transform folder)
    {
        base.Setup(gameModeSlug, maxScore, folder);
        EventHandling.OnDeath += OnDeath;
    }

    void OnDeath(int team)
    {
        ModularTeams t = GameManagerModular.instance.GetTeam(team);
        t.IncrementScore(1 * ResourceManagerModular.instance.GetCommandPostTeamCount(team));//SpawnManager.instance.GetSpawnersOnTeamCount(team));
        if(t.Score == MaxScore)
        {
            IsComplete = true;
        }
    }

    public override void DestroyReferences()
    {
        EventHandling.OnDeath -= OnDeath;
    }


}

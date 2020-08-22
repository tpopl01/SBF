using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Events;

[CreateAssetMenu
 (
     fileName = "CTF",
     menuName = "GameMode/CTF"
 )]
public class GameMode_CTF : GameMode
{
    public override void Setup(string gameModeSlug, int maxScore, Transform folder)
    {
        base.Setup(gameModeSlug, maxScore, folder);
        EventHandling.OnCapture += OnCapture;
        this.MaxScore /= 100;
    }

    private void OnCapture(int team)
    {
        ModularTeams t = GameManagerModular.instance.GetTeam(team);
        t.IncrementScore(1);
        if (t.Score == MaxScore)
        {
            IsComplete = true;
        }
    }

    public override void DestroyReferences()
    {
        EventHandling.OnCapture -= OnCapture;
    }

}

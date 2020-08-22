using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameMode : ScriptableObject
{
    public int MaxScore { get; protected set; }
    public bool IsComplete { get; protected set; }

    public bool VictoryNear(int teamW, int teamL)
    {
        if (teamW == 0)
            return false;

        if (teamW / MaxScore > 0.8f)
        {
            if(teamW > teamL * 1.5f)
            {
                return true;
            }
        }

        return false;
    }

    public virtual void Setup(string gameModeSlug, int maxScore, Transform folder)
    {
        IsComplete = false;
        //Load relevant setup data
        GameModeSetup gameModeSetup = Resources.Load<GameModeSetup>("GameMode/Setup/" + gameModeSlug + "_setup");
        gameModeSetup.SetUp(folder);
        this.MaxScore = maxScore;
        
    }

    public abstract void DestroyReferences();
}
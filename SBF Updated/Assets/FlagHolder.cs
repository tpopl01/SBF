using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHolder : MonoBehaviour, IGoal
{
    int team;

    public bool hasFlag = true;
    public FlagHolder enemyFlagHolder;

    public bool GetHasFlag()
    {
        return hasFlag;
    }

    public void Capture(int team, Vector3 pos)
    {
        
    }

    public bool GetNearestGoal(int team)
    {
        if(enemyFlagHolder.hasFlag == false)
        {
            if (team == this.team)
                return false;
            if (team == -1)
                return true;
        }
        if (hasFlag == false)
            return team != this.team && team != -1;
        
        return team == this.team;// team ==this.team;
    }
    public bool InRange(Vector3 pos)
    {
        return false;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    public void Initialise(int team)
    {
        this.team = team;
    }

    private void Start()
    {
        FlagHolder[] fH = GameObject.FindObjectsOfType<FlagHolder>();
        Debug.Log(fH.Length);
        for (int i = 0; i < fH.Length; i++)
        {
            if (fH[i].team != team)
            {
                Debug.Log("Enemy flag set for: " + team);
                enemyFlagHolder = fH[i];
                break;
            }
        }
    }

    public void ResetFlag(Transform flag)
    {
        flag.transform.position = transform.position;
        flag.SetParent(transform);
        hasFlag = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Flag f = other.transform.GetComponentInChildren<Flag>();
        if(f)
        {
            if(f.GetTeam() != team)
            {
                f.ResetFlag();
                tpopl001.Events.EventHandling.Capture(team);
            }
        }
    }
}

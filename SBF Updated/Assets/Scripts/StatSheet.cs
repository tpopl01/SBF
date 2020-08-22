using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSheet : MonoBehaviour
{
    [SerializeField] RectTransform team1GO = null;
    [SerializeField] RectTransform team2GO = null;


    public void AddStats(Stats stats)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        StatsPrefab newStats = Instantiate(Resources.Load<StatsPrefab>("Stats/stats_prefab"));
        newStats.SetStats(stats);
        if(stats.Team == 0)
            newStats.transform.SetParent(team1GO);
        else if(stats.Team==1)
            newStats.transform.SetParent(team2GO);
    }


    public static StatSheet instance;
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
}

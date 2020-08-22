using System.Collections;
using System.Collections.Generic;
using tpopl001.Events;
using UnityEngine;

public class StatsHolder : MonoBehaviour
{
    public Stats GetStats { get; private set; } = null;

    private void Awake()
    {
        Init(GetComponent<ModularController>().Team);
        EventHandling.OnComplete += AddStats;
    }
    private void OnDestroy()
    {
        EventHandling.OnComplete -= AddStats;
    }

    public void Init(int team)
    {
     //   GetStats = new Stats(name, team);
    }
    void AddStats()
    {
        StatSheet.instance.AddStats(GetStats);
    }
}

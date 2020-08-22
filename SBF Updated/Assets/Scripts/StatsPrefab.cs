using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPrefab : MonoBehaviour
{
    [SerializeField] Text nameText = null;
    [SerializeField] Text killText = null;
    [SerializeField] Text deathText = null;
    [SerializeField] Text pointsText = null;

    public void SetStats(Stats stats)
    {
        nameText.text = stats.Name;
        killText.text = stats.Kills.ToString();
        deathText.text = stats.Deaths.ToString();
        pointsText.text = stats.Points.ToString();
    }
}

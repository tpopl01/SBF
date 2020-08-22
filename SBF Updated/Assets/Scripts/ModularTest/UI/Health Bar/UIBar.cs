using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    protected Slider bar;

    protected float current;
    protected float max = 100;

    public virtual void Init(float max)
    {
        this.max = max;
        bar = GetComponent<Slider>();
        current = max;
        bar.value = CalculatePercentage();
        UpdateUI(max);
    }


    public void UpdateUI(float current)
    {
        this.current = current;
        bar.value = CalculatePercentage();
    }

    float CalculatePercentage()
    {
        return current / max;
    }


}

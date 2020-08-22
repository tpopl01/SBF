using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

public class CaptureBar : UIBar
{
    Timer t = new Timer(2);
    bool capturing = false;
    GameObject child;

    public override void Init(float max)
    {
        child = transform.GetChild(0).gameObject;
        base.Init(max);
        Disable();
    }

    public void Capture(float current, float max)
    {
        if(child.activeSelf == false)
            child.SetActive(true);
        this.current = current;
        this.max = max;
        UpdateUI(current);
        capturing = true;
        t.StartTimer();
    }

    private void Update()
    {
        if (child == null) return;

        if(!capturing)
        {
            if(t.GetComplete())
            {
                Disable();
            }
        }
        capturing = false;
    }

    void Disable()
    {
        if(child.activeSelf == true)
            child.SetActive(false);
    }
}

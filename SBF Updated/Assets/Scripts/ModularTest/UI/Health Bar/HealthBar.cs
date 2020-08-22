using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : UIBar {
    GameObject child;
    IHealth player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<WeaponSystem_Player>().GetComponent<IHealth>();
        Init(player.GetMaxHP());
    }

    public override void Init(float max)
    {
        child = transform.GetChild(0).gameObject;
        base.Init(max);
        Disable();
    }

    private void Update()
    {
        if (child == null) return;

        if (player.GetHPPercent() == 1)
        {
            Disable();
        }
        else
        {
            if (child.activeSelf == false)
                child.SetActive(true);
            UpdateUI(player.GetHP());
        }
    }

    void Disable()
    {
        if (child.activeSelf == true)
            child.SetActive(false);
    }
}

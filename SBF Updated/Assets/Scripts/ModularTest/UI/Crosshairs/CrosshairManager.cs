using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour {

    public int index;
    [SerializeField]Crosshair activeCrosshair = null;
    [SerializeField]Crosshair[] crosshairs = null;

    public static CrosshairManager instace;
    public static CrosshairManager GetInstance()
    {
        return instace;
    }

    void Awake()
    {
        instace = this;
    }

    void Start()
    {
        for (int i = 0; i < crosshairs.Length; i++)
        {
            crosshairs[i].gameObject.SetActive(false);
        }

        crosshairs[index].gameObject.SetActive(true);
        activeCrosshair = crosshairs[index];
    }

    public void SetNewCrosshair(int findIndex)
    {
        //Debug.Log("Setting crosshair to index " + findIndex);
        if(findIndex >= crosshairs.Length)
        {
            Debug.LogWarning("No Crosshair of that index");
            return;
        }

        activeCrosshair = crosshairs[findIndex];
        foreach (var item in crosshairs)
        {
            if(item != activeCrosshair)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
        index = findIndex;
    }

    public void DefineCrosshairByIndex(int findIndex)
    {
        activeCrosshair = crosshairs[findIndex];
    }

    public void DefineCrosshairByName(string name)
    {
        for (int i = 0; i < crosshairs.Length; i++)
        {
            if(string.Equals(crosshairs[i].name,name))
            {
                activeCrosshair = crosshairs[i];
                break;
            }
        }
    }

    public void WiggleCrosshair()
    {
        activeCrosshair.WiggleCrosshair();
    }

    public void FadeAlpha(float alpha)
    {
        foreach (Crosshair.CrosshairPart part in activeCrosshair.parts)
        {
            part.image.color = new Color(1f, 1f, 1f, alpha);
        }
    }

}


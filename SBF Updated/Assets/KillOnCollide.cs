using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollide : MonoBehaviour, ISetup
{
    IHealth h;
    Spaceship s;

    public void SetUp(Transform root)
    {
        h = root.GetComponentInChildren<IHealth>();
        s = root.GetComponentInChildren<Spaceship>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(s.GetState() == ShipState.Air)
        {
            if (collision.gameObject.layer == 10)
                return;
            Debug.Log("Force Kill");
            h.ForceKill();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class HealthPackPool : ObjectPool<HealthPickable>
{
    Timer timeTillDespawn = new Timer(10);

    List<HealthPickable> active = new List<HealthPickable>();
    public void SpawnHealth(Vector3 position)
    {
        var health = Get();
        if (health == null)
        {
            return;
        }

        if (timeTillDespawn.GetComplete())
            timeTillDespawn.StartTimer();

        health.transform.position = position;
        health.gameObject.SetActive(true);
        ResourceManagerModular.instance.AddHealer(health);
        active.Add(health);
    }

    public void ReturnHealth(HealthPickable h)
    {
        if (active.Contains(h))
        {
            ReturnObject(h);
            ResourceManagerModular.instance.RemoveHealer(h);
            active.Remove(h);
        }
    }

    private void Update()
    {
        if(active.Count > 0)
        {
            if(timeTillDespawn.GetComplete())
            {
                ReturnObject(active[0]);
                ResourceManagerModular.instance.RemoveHealer(active[0]);
                active.RemoveAt(0);
                timeTillDespawn.StartTimer();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class AmmoPackPool : ObjectPool<AmmoPickable>
{
    Timer timeTillDespawn = new Timer(10);

    List<AmmoPickable> active = new List<AmmoPickable>();
    public void SpawnAmmo(Vector3 position)
    {
        var ammo = Get();
        if (ammo == null)
        {
            return;
        }

        if (timeTillDespawn.GetComplete())
            timeTillDespawn.StartTimer();

        ammo.transform.position = position;
        ammo.gameObject.SetActive(true);
        ResourceManagerModular.instance.AddAmmoGen(ammo);
        active.Add(ammo);
    }

    public void ReturnAmmo(AmmoPickable a)
    {
        if(active.Contains(a))
        {
            ReturnObject(a);
            ResourceManagerModular.instance.RemoveAmmoGen(a);
            active.Remove(a);
        }
    }

    private void Update()
    {
        if (active.Count > 0)
        {
            if (timeTillDespawn.GetComplete())
            {
                ReturnObject(active[0]);
                ResourceManagerModular.instance.RemoveAmmoGen(active[0]);
                active.RemoveAt(0);
                timeTillDespawn.StartTimer();
            }
        }
    }
}

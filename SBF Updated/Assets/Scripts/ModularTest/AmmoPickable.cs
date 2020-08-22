using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickable: MonoBehaviour, IAmmoGen
{
    [SerializeField] float radius = 1;
    [SerializeField] bool destroy;

    private void Start()
    {
        if (destroy) ResourceManagerModular.instance.AddAmmoGen(this);
    }

    private void OnDestroy()
    {
        if (destroy) ResourceManagerModular.instance.RemoveAmmoGen(this);
    }

    public void AddAmmo(IAmmoAdd a)
    {
        if (this != null)
        {
            if (Vector3.Distance(Position(), a.Position()) < radius)
            {
                if (a.AddAmmo())
                {
                    if (destroy)
                    {
                        ResourceManagerModular.instance.RemoveAmmoGen(this);
                        Destroy(gameObject);
                    }
                    else
                    {
                        ResourceManagerModular.instance.ReturnAmmo(this);
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public Vector3 Position()
    {
        if (this == null)
            return Vector3.zero;

        return transform.position;
    }
}

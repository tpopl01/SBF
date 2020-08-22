using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour, IPickable
{
    [SerializeField] string slug = "";
    [SerializeField] float range = 1;

    private void Start()
    {
        if (string.IsNullOrEmpty(slug))
            slug = name;

        ResourceManagerModular.instance.AddPickable(this);
    }

    public bool Pickup(IWeaponSystomChangeable w, Vector3 pos)
    {
        if (w.CanPickup()) {
            if (Vector3.Distance(pos, Position()) < range)
            {
                w.AddWeapon(slug);
                ResourceManagerModular.instance.RemovePickable(this);
                Destroy(this.gameObject);
                return true;
            }
        }
        return false;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    private void OnDestroy()
    {
        ResourceManagerModular.instance.RemovePickable(this);
    }
}

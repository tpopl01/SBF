using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : MonoBehaviour, IHealer
{
    [SerializeField]float radius = 1;
    [SerializeField] bool destroy;

    private void Start()
    {
        if (destroy) ResourceManagerModular.instance.AddHealer(this);
    }

    private void OnDestroy()
    {
        if (destroy) ResourceManagerModular.instance.RemoveHealer(this);
    }
    public bool Heal(IHealable healable)
    {
        // for (int i = 0; i < healable.Length; i++)
        //  {
        IHealable h = healable;//[i];
        if (this != null)
            if (Vector3.Distance(h.Position(), transform.position) < radius)
            {
                h.Heal();
                if (destroy)
                {
                    ResourceManagerModular.instance.RemoveHealer(this);
                    Destroy(gameObject);
                }
                else
                {
                    ResourceManagerModular.instance.ReturnHealth(this);
                    gameObject.SetActive(false);
                }
            }
      //  }

        return true;
    }

    public Vector3 Position()
    {
        if(this != null)
        return transform.position;
        return Vector3.zero;
    }
}

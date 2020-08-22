using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class SpecialBase1 : MonoBehaviour, ISpecial,ISetup
{
    public int Count { get; protected set; } = 0;
    [SerializeField] protected int max = 3;
    [SerializeField] protected SpecialType type;
    [SerializeField] protected string slug = "";
    Timer coolDown = new Timer(3);

    public void Respawn()
    {
        Count = max;
    }

    public virtual void SetUp(Transform root)
    {
        Count = max;
    }

    public string GetName()
    {
        return this.slug;
    }

    public int GetRemaining()
    {
        return Count;
    }

    public virtual bool Use()
    {
        if (coolDown.GetComplete())
        {
            coolDown.StartTimer();
            if (Count == 0) return false;
            Count--;
            return true;
        }
        return false;
    }

    public SpecialType GetSpecialType()
    {
        return type;
    }

    public bool AddAmmo()
    {
        Count++;
        if (Count > max) Count = max;
        return Count < max;
    }

}

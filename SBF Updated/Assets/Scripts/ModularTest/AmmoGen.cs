using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class AmmoGen : MonoBehaviour, IAmmoGen
{
    [SerializeField]float radius = 5;
    [SerializeField] [Range(0.1f, 100)] float speed = 5;
    List<IAmmoAdd> ammoAdds = new List<IAmmoAdd>();
    Tick t = new Tick(5, 20);
    Timer timer;
    IHealth h;

    private void Start()
    {
        ResourceManagerModular.instance.AddAmmoGen(this);
        timer = new Timer(1 / speed);
        timer.StartTimer();
        h = GetComponent<IHealth>();
    }

    private void Update()
    {
       
    }

    private void LateUpdate()
    {
        if (h.IsDead()) return;
        if (t.IsDone())
        {
            if (timer.GetComplete())
            {
                timer.StartTimer();
                AddAmmo();
            }
        }
        ammoAdds.Clear();
    }

    public void AddAmmo(IAmmoAdd a)
    {
        if (h.IsDead()) return;
        if (Vector3.Distance(a.Position(), transform.position) < radius)
            if (!ammoAdds.Contains(a))
                ammoAdds.Add(a);
    }

    void AddAmmo()
    {
        if (h.IsDead()) return;
        for (int i = 0; i < ammoAdds.Count; i++)
        {
            ammoAdds[i].AddAmmo();
        }
            
    }

    public Vector3 Position()
    {
        if (h.IsDead()) return Vector3.zero;
        return StaticMaths.GetPos(transform.position, radius);
    }
}

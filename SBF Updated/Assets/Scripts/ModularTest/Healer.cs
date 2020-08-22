using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public class Healer : MonoBehaviour, IHealer
{
   // [SerializeField][Range(0.1f, 10)] float healAmount = 5;
    [SerializeField] float radius = 5;
    [SerializeField] [Range(0.1f, 100)] float speed = 5;
    List<IHealable> healables = new List<IHealable>();
    Tick t = new Tick(5, 20);
    Timer timer;
    IHealth h;

    private void Start()
    {
        ResourceManagerModular.instance.AddHealer(this);
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
                Heal();
            }
        }
        healables.Clear();
    }

    public bool Heal(IHealable healable)
    {
        if (h.IsDead()) return false;
        //for (int i = 0; i < healable.Length; i++)
        //   {
        if (Vector3.Distance(healable.Position(), transform.position) < radius)
            if (!healables.Contains(healable)) healables.Add(healable);
      //  }
        
        return true;
    }

    void Heal()
    {
        for (int i = 0; i < healables.Count; i++)
        {
            healables[i].Heal();
        }
    }

    public Vector3 Position()
    {
        if (h.IsDead()) return Vector3.zero;
        Vector3 s = transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
        if (NavMesh.SamplePosition(s, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            s = hit.position;
        }

        return s;
    }
}

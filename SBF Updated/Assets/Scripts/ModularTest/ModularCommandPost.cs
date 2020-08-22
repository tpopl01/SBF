using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ModularCommandPost : MonoBehaviour, ISpawner
{
    [SerializeField]protected int team = -1;
    [SerializeField]protected float range = 8;

    protected Material mat;
    float alpha = 0.3f;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        mat = GetComponentInChildren<MeshRenderer>().material;
        InitColours();
    }

    public int GetTeam()
    {
        return team;
    }

    protected virtual void SetCapture(Color c, int team)
    {
        this.team = team;
        c.a = alpha;
        mat.color = c;
    }

    void InitColours()
    {
        ModularTeams[] t = GameManagerModular.instance.Teams;
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].GetTeam() == team)
            {
                Color c = t[i].GetColour();
                SetCapture(c, team);
            }
        }

        if (team == -1)
        {
            SetCapture(Color.white, -1);
        }
    }

    public bool CanSpawn(int team)
    {
        return team == this.team;
    }

    public Vector3 GetSpawnPosition()
    {
        Vector3 s = transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        if(NavMesh.SamplePosition(s, out NavMeshHit hit, 2, NavMesh.AllAreas))
        {
            s = hit.position;
        }

        return s;
    }
}

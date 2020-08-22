using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Utils;

public class Flag : MonoBehaviour, IGoal
{
    [SerializeField] int team = 0;
    FlagHolder flagHolder = null;
    public Vector3 StartPos { get; private set; }
    public bool IsCarried { get; private set; }

    ModularControllerUnit host = null;

    Timer timer = new Timer(20);

    private void Awake()
    {
        flagHolder = GetComponentInParent<FlagHolder>();
        flagHolder.Initialise(team);
        StartPos = transform.position;
    }

    private void Update()
    {
        if(host == null)
        {
            if(transform.position != StartPos)
            {
                if(timer.GetComplete())
                {
                    ResetFlag();
                }
            }
        }
        else if(host.Health.IsDead() || host.gameObject.activeInHierarchy == false)
        {
            timer.StartTimer();
            transform.SetParent(null);
          //  host.HasFlag = false;
            host = null;
            IsCarried = false;
        }
        else
        {
           // transform.position = host.Senses.IdealHitPos - host.transform.forward * 0.2f;
        }
        
    }

    private void LateUpdate()
    {
        if(host)
        {
        //    transform.position = host.Senses.IdealHitPos - host.transform.forward * 0.2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsCarried)
        {
            ModularControllerUnit ai = other.GetComponentInParent<ModularControllerUnit>();
            if (ai)
            {
                //reset
                if (ai.Team == team)
                {
                    ResetFlag();
                }
                //pick up
                else
                {
                    host = ai;
                    transform.SetParent(ai.transform);
                    flagHolder.hasFlag = false;
                    //   host.HasFlag = true;
                    transform.SetParent( ai.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Chest).transform);
                    transform.position = host.Senses.IdealHitPos - host.transform.forward * 0.2f;
                }
            }
        }
    }

    public int GetTeam()
    {
        return team;
    }

    public void ResetFlag()
    {
        host = null;
        IsCarried = false;
        flagHolder.ResetFlag(transform);
        transform.localPosition = Vector3.zero;
    }

    public Vector3 Position()
    {
       // if (host) return flagHolder.enemyFlagHolder.Position();

        return transform.position;
    }

    public bool GetNearestGoal(int team)
    {
        if (IsCarried)
        {
            //   if (team == host.Team || team == -1)
            return this.team == host.Team || this.team == -1;
            //   else
            //     return false;
        }
        else if (transform.position != StartPos)
        {
            return team == -1;
        }
        return team == this.team;
    }

    public bool InRange(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) < 6;
    }

    public void Capture(int team, Vector3 pos)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class ModularCommandPostCapturable : ModularCommandPost, IGoal
{
    UnitInTeam[] unitInTeams;
    Tick t = new Tick(5, 10);

    public bool GetNearestGoal(int team)
    {
        return team == this.team;
    }

    public Vector3 Position()
    {
        return GetSpawnPosition();
    }

    Timer timer;// = new Timer(10 / speed);
   // UnitInTeam[] unitInTeams;
    [Space]
    [Header("Capture Settings")]
    [SerializeField] [Range(0.1f, 100)] float speed = 20f;
    [SerializeField] [Range(100, 200)] int maxCapture = 100;

    public int GetMaxCapture()
    {
        return maxCapture;
    }

    public int CaptureAmount { get; private set; } = 0;

    //private UnitInTeam[] GetNearbyUnits()
    //{
    //    UnitInTeam[] unitsInTeam = new UnitInTeam[GameManagerModular.instance.Teams.Length];
    //    for (int i = 0; i < unitsInTeam.Length; i++)
    //    {
    //        unitsInTeam[i] = new UnitInTeam();
    //        unitsInTeam[i].team = GameManagerModular.instance.Teams[i].GetTeam();
    //        unitsInTeam[i].count = GameManagerModular.instance.GetNearbyUnitCountInTeam(unitsInTeam[i].team, range, transform.position);
    //    }
    //    return unitsInTeam;
    //}


    protected override void Init()
    {
        base.Init();
        timer = new Timer(1 / speed);
        timer.StartTimer();
        unitInTeams = new UnitInTeam[GameManagerModular.instance.teams_id.Length];
        for (int i = 0; i < unitInTeams.Length; i++)
        {
            unitInTeams[i] = new UnitInTeam(GameManagerModular.instance.teams_id[i], 0);
        }
    }

    void Update()
    {
      
    }

    private void LateUpdate()
    {
        if (t.IsDone())
        {
            if (timer.GetComplete())
            {
                timer.StartTimer();
                UpdateCapture();
            }
        }
        ResetUnitInTeam();
    }

    private void ResetUnitInTeam()
    {
        for (int i = 0; i < unitInTeams.Length; i++)
        {
            unitInTeams[i].count = 0;
        }
    }

    protected override void SetCapture(Color c, int team)
    {
        base.SetCapture(c, team);
        CaptureAmount = (team == -1) ? 0 : maxCapture;
        if (team != -1)
        {
            ModularTeams t = GameManagerModular.instance.GetTeam(team);
            for (int i = 0; i < t.ActiveUnits.Count; i++)
            {
                t.ActiveUnits[i].Stats.OnPoints();
                //StatsHolder sH = t.ActiveUnits[i].GetComponent<StatsHolder>();
                //if (sH)
                //{
                //    sH.GetStats.OnCapture();
                //}
            }
        }
    }

    private void Capturing(int index, int capturingTeam, UnitInTeam[] unitInTeams)
    {

        if (team == -1)
        {
            CaptureAmount = Mathf.Clamp(CaptureAmount += unitInTeams[index].count, 0, maxCapture);
            if (CaptureAmount == maxCapture)
            {
                SetCapture(GameManagerModular.instance.Teams[index].GetColour(), capturingTeam);
                tpopl001.Events.EventHandling.Capture(capturingTeam);
            }
        }
        else if (capturingTeam != team)
        {
            CaptureAmount = Mathf.Clamp(CaptureAmount -= unitInTeams[index].count, 0, maxCapture);
            if (CaptureAmount == 0)
            {
                SetCapture(Color.white, -1);
            }
        }
    }

    private void UpdateCapture()
    {
       // UnitInTeam[] unitInTeams = GetNearbyUnits();
        //prevent capture or reset code
        int teams = 0;
        int teamIndex = -1;
        bool ownedTeamPresent = false;
        for (int i = 0; i < unitInTeams.Length; i++)
        {
            if (unitInTeams[i].count > 0 && unitInTeams[i].team != -1)
            {
                teamIndex = i;
                teams++;
                if (unitInTeams[i].team == team)
                {
                    ownedTeamPresent = true;
                }
            }
        }
        if (ownedTeamPresent)
            return;

        if (teams > 1)
            return;
        //capture code
        if (teamIndex != -1)
            Capturing(teamIndex, GameManagerModular.instance.teams_id[teamIndex], unitInTeams);

    }

    public bool InRange(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos) < range;
    }

    public void Capture(int team, Vector3 pos)
    {
        for (int i = 0; i < unitInTeams.Length; i++)
        {
            if(unitInTeams[i].team == team)
            {
                if(InRange(pos))
                {
                    unitInTeams[i].count++;
                }
                break;
            }
        }
    }
}
public class UnitInTeam
{
    public int team;
    public int count;
    public UnitInTeam(int team, int count)
    {
        this.team = team;
        this.count = count;
    }
}
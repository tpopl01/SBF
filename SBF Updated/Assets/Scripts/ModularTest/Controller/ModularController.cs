using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularController : MonoBehaviour
{
    public InputBase input { get; private set; }
    [SerializeField] AIStats aIStats; public AIStats AIStats() { return aIStats; } public void SetStats(AIStats s) { aIStats = s; }
    [SerializeField] SetupComponents setup;
    public Stats Stats { get; private set; }

    public IWeaponSystem weaponSystem { get; private set; }

    protected ITick[] ticks;
    protected IFixedTick[] fixedTicks;
    protected ITeamChange[] teamChanges;
    protected IAim[] iAims;
    protected IAiming[] iAimings;

    // public float Speed { get; private set; } = 0;
    //  public float TurnSpeed { get; private set; } = 0;
    // public bool Crouching { get; private set; }
    public Vector3 Position { get; private set; }
    public SensesBase Senses { get; private set; }
    public IHealth Health { get; private set; }
    public bool Aiming { get; private set; }
    public bool Dead { get; private set; }

    public int Team = -1;
    public bool debugAim = false;

    public void SetTeam(int team)
    {
        this.Team = team;
        if (teamChanges != null)
        {
            for (int i = 0; i < teamChanges.Length; i++)
            {
                teamChanges[i].SetTeam(Team);
            }
        }
    }


    private void Awake()
    {
        if(setup)
        setup.Setup(transform);
        Stats = Instantiate<Stats>(Resources.Load<Stats>("Stats/Stats"));
        Stats.Team = Team;
        Stats.Name = gameObject.name;
    }

    void Start()
    {
        Initialise();
        ISetup[] setups = GetComponentsInChildren<ISetup>();
        for (int i = 0; i < setups.Length; i++)
        {
            setups[i].SetUp(transform);
        }
        ticks = GetComponentsInChildren<ITick>();
        fixedTicks = GetComponentsInChildren<IFixedTick>();
        iAims = GetComponentsInChildren<IAim>();
        iAimings = GetComponentsInChildren<IAiming>();
        Health = GetComponentInChildren<IHealth>();
        weaponSystem = GetComponentInChildren<IWeaponSystem>();
    }

    protected virtual void Initialise()
    {
        Senses = GetComponentInChildren<SensesBase>();
        input = GetComponentInChildren<InputBase>();

    }

    void Update()
    {
        Dead = Health.GetHP() <= 0;
        if (Dead) return;
        Tick();
    }

    protected virtual void Tick()
    {
        Position = transform.position;

        input.Execute(this);

        if (input.Aim)
        {
            Aiming = GetAiming();
        }
        else Aiming = false;
        Aim((Aiming) || debugAim);
       // if (input.Attack) weaponSystem.Attack(input.TargetPos, this, Senses.ClosestEnemy);

        for (int i = 0; i < ticks.Length; i++)
        {
            ticks[i].Tick();
        }

        //if(Health.GetHP() <= 0 && !Dead)
        //{
        //    Respawn();
        //}
    }

    private void FixedUpdate()
    {
        FixedTick();
    }

    protected virtual void FixedTick()
    {
        for (int i = 0; i < fixedTicks.Length; i++)
        {
            fixedTicks[i].FixedTick();
        }
    }


    public void Aim(bool enable)
    {
        for (int i = 0; i < iAims.Length; i++)
        {
            iAims[i].SetAim(enable);
        }
    }

    bool GetAiming()
    {
        bool aiming = true;
        for (int i = 0; i < iAimings.Length; i++)
        {
            if (iAimings[i].GetAiming() == false)
            {
                aiming = false;
            }
        }
        return aiming;
    }

    private void OnDestroy()
    {
        GameManagerModular.instance.RemoveUnitFromTeam(this);
    }

}

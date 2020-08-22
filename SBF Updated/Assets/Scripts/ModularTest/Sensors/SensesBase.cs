using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tpopl001.Utils;
using UnityEngine;

public class SensesBase : MonoBehaviour, ITick, IRespawn
{
    public Vector3 TargetPos { get; set; }
    protected ModularController self;
    [SerializeField] DetermineTargetsBase determineTargets;
    List<ModularController> allies = new List<ModularController>();
    List<ModularController> enemies = new List<ModularController>();
   // List<ModularController> alliesNotInSight = new List<ModularController>();
   // List<ModularController> enemiesNotInSight = new List<ModularController>();

    ModularController closestEnemy;
    ModularController furthestEnemy;
    ModularController weakestEnemy;
    ModularController strongestEnemy;

    public ModularController ClosestEnemyNotInSight { get; private set; }
    public ModularController ClosestEnemy { get { if (closestEnemy) TargetPos = closestEnemy.Senses.IdealHitPos; return closestEnemy; } }
    public ModularController FurthestEnemy { get { if (furthestEnemy) TargetPos = furthestEnemy.Senses.IdealHitPos; return furthestEnemy; } private set { } }
    public ModularController ClosestAlly { get; private set; }
    public ModularController FurthestAlly { get; private set; }
    public ModularController WeakestEnemy { get { if (weakestEnemy) TargetPos = weakestEnemy.Senses.IdealHitPos; return weakestEnemy; } private set { } }
    public ModularController StrongestEnemy { get { if (strongestEnemy) TargetPos = strongestEnemy.Senses.IdealHitPos; return strongestEnemy; } private set { } }
    public ModularController WeakestAlly { get; private set; }
    public ModularController StrongestAlly { get; private set; }
    public Vector3 EnemyBarycenter { get; private set; }
    public Vector3 AllyBarycenter { get; private set; }
    public Vector3 IdealHitPos { get; protected set; }
    public Vector3 ShotAtFrom { get; set; }

    private readonly Tick tickObject = new Tick(5, 20);
    private readonly Tick sightTick = new Tick(3, 10);

    public virtual void Init(Transform root, string determineTargets_slug)
    {
        self = root.GetComponent<ModularController>();
        determineTargets = Resources.Load<DetermineTargetsBase>(determineTargets_slug);
    }

    public virtual void Tick()
    {
        IdealHitPos = transform.position;
        if (tickObject.IsDone())
        {
            ScatteredTick();
        }
        if (sightTick.IsDone())
        {
            determineTargets.DetermineControllers(transform, self, GameManagerModular.instance.Teams, self.Team, out allies, out enemies, self.AIStats().GetPerception());
            ClosestAlly = Closest(allies);
            closestEnemy = Closest(enemies);
           // ClosestEnemyNotInSight = Closest(enemiesNotInSight);
            furthestEnemy = Furthest(enemies);
            strongestEnemy = Strongest(enemies);
            weakestEnemy = Weakest(enemies);
            FurthestAlly = Furthest(allies);
            StrongestAlly = Strongest(allies);
            WeakestAlly = Weakest(allies);
            AllyBarycenter = BaryCenter(allies);
            EnemyBarycenter = BaryCenter(enemies);
        }
    }

    protected virtual void ScatteredTick()
    {
        
    }

    private void LateUpdate()
    {
        ShotAtFrom = Vector3.zero;
    }

    #region AI Senses
    public bool HasEnemy()
    {
        return enemies.Count > 0;
    }

    public bool WeakerThan(ModularController unit)
    {
        return unit.Health.GetHPPercent() > self.Health.GetHPPercent();
    }

    //public bool EnemiesInArea()
    //{
    //    return enemies.Count > 0 || enemiesNotInSight.Count > 0;
    //}

    //public bool AlliesInArea()
    //{
    //    return allies.Count > 0 || alliesNotInSight.Count > 0;
    //}

    public bool IsEnemy(ModularController sensor)
    {
        return sensor.Team != self.Team;
    }

    public float DistanceFrom(Vector3 position)
    {
        return Vector3.Distance(self.Position, position);
    }

    public float NormalisedHealth()
    {
        return self.Health.GetHPPercent();
    }

    public float DamageAtDistance(float distance)
    {
        return self.weaponSystem.GetDamageAtDistance(distance);
    }

    public float HitChance(float distance)
    {
        return self.weaponSystem.hitChance(distance);
    }

    public bool InRange(Vector3 position)
    {
        return GetRange(position) < 1;
    }

    public float GetRange(ModularController enemy)
    {
        return self.weaponSystem.Range(Vector3.Distance(enemy.Position, self.Position));
    }

    public float GetRange(Vector3 enemy)
    {
        return self.weaponSystem.Range(Vector3.Distance(enemy, self.Position));
    }
    #endregion

    #region Targets
    private ModularController Closest(List<ModularController> units)
    {
        if (units.Count == 0) return null;
        float dist = Mathf.Infinity;
        ModularController unit = null;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Health.IsDead()) continue;
            float newDist = DistanceFrom(units[i].Position);
            if (dist > newDist)
            {
                unit = units[i];
                dist = newDist;
            }
        }
        return unit;
    }

    private ModularController ClosestByPassSight(List<ModularController> units)
    {
        if (units.Count == 0) return null;
        return units.Aggregate((x, y) => (DistanceFrom(x.Position) <= DistanceFrom(y.Position)) ? x : y);
    }

    private ModularController Furthest(List<ModularController> units)
    {
        if (units.Count == 0) return null;
        float dist = 0;
        ModularController unit = null;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Health.IsDead()) continue;
            float newDist = DistanceFrom(units[i].Position);
            if (dist < newDist)
            {
                unit = units[i];
                dist = newDist;
            }
        }
        return unit;
    }

    //Test
    private Vector3 BaryCenter(List<ModularController> units)
    {
        if (units.Count == 0) return self.Position;

        Vector3 pos = units[0].Position;
        for (int i = 1; i < units.Count; i++)
        {
            pos += units[i].Position;
        }
        pos /= units.Count;
        return pos;
    }

    private ModularController Weakest(List<ModularController> units)
    {
        if (units.Count == 0) return null;
        float h = Mathf.Infinity;
        ModularController unit = null;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].Health.IsDead()) continue;
            float newDist = units[i].Health.GetHP();
            if (h > newDist)
            {
                unit = units[i];
                h = newDist;
            }
        }
        return unit;
    }

    private ModularController Strongest(List<ModularController> units)
    {
        if (units.Count == 0) return null;
        float h = 0;
        ModularController unit = null;
        if(units.Count > 0)
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Health.IsDead()) continue;
                //Debug.Log(units[i]);
                float newDist = units[i].Health.GetHP();
                if (h < newDist)
                {
                    unit = units[i];
                    h = newDist;
                }
            }
        return unit;
    }

    public virtual bool Respawn()
    {
        closestEnemy = null;
        furthestEnemy = null;
        strongestEnemy = null;
        weakestEnemy = null;
        return true;
    }
    #endregion


}

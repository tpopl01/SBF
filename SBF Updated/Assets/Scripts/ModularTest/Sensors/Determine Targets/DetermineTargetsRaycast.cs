using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Determine_targets_raycast",
      menuName = "Modular/Determine/Determine_targets_raycast"
  )]
public class DetermineTargetsRaycast : DetermineTargetsBase
{
    [SerializeField] Raycast sightRay;
    public override void DetermineControllers(Transform head, ModularController self, ModularTeams[] teams, int team, out List<ModularController> allies, out List<ModularController> enemies, float range)
    {
        allies = new List<ModularController>();
        enemies = new List<ModularController>();
        for (int i = 0; i < teams.Length; i++)
        {
            if ((teams[i].IsUsersTeam(team)))
            {
                allies = AddToArray(head, teams[i].ActiveUnits, allies, self, range);
            }
            else
            {
                enemies = AddToArray(head, teams[i].ActiveUnits, enemies, self, range);
            }
        }
    }

    List<ModularController> AddToArray(Transform head, List<ModularController> units, List<ModularController> inSight, ModularController self, float range)
    {
        for (int n = 0; n < units.Count; n++)
        {
            ModularController unit = units[n];
            if (unit == self) continue;
            if (unit == null || self == null)
            {
                Debug.LogWarning("This should not happen");
                continue;
            }
            if (Vector3.Distance(unit.Position, self.Position) <= range)
            {
                if (StaticMaths.GetAngle(unit.Position, head.position, head.forward) < self.AIStats().GetVision())
                {
                    if (unit.Senses)
                    {
                        if (sightRay.RaycastTarget(head.position, unit.Senses.IdealHitPos, out RaycastHit hit, range + 1, 10))
                        {
                            inSight.Add(unit);
                            //   Debug.Log("Add In Sight");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Unit doesn't have senses setup yet");
                    }
                }
            }
        }
        return inSight;
    }
}

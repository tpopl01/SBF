using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetermineTargetsBase : ScriptableObject
{
    public abstract void DetermineControllers(Transform head, ModularController self, ModularTeams[] teams, int team, out List<ModularController> allies, out List<ModularController> enemies, float range);


}

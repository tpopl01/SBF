using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Setup_components_Turret_Spawnable",
      menuName = "Modular/Components/Setup_components_Turret_Spawnable"
  )]
public class SetupTurret : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        rootGo.layer = 10;
        rootGo.AddComponent<ModularHealthIndependent>();
        rootGo.AddComponent<WeaponSystemVehicle>();
        rootGo.AddComponent<InputBrainBase>();
        SensesBase s = rootGo.AddComponent<SensesBase>();
        s.Init(root, "Modular/Determine/Determine_targets_raycast");
    }
}

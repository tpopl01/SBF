using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Setup_components_Turret",
      menuName = "Modular/Components/Setup_components_Turret"
  )]
public class SetupTurretComponents : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        rootGo.layer = 10;
       // rootGo.AddComponent<ModularHealthMachine>();
        rootGo.AddComponent<InputBrainBase>();
        rootGo.AddComponent<WeaponSystemVehicle>();

        Transform[] allChildren = rootGo.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name.Equals("Head"))
            {
                SensesBase s = allChildren[i].gameObject.AddComponent<SensesBase>();
                s.Init(root, "Modular/Determine/Determine_targets_raycast");
                break;
            }
        }

    }
}

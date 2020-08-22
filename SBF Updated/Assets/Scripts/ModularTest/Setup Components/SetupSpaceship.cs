using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Setup_components_Spaceship",
      menuName = "Modular/Components/Setup_components_Spaceship"
  )]
public class SetupSpaceship : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        rootGo.layer = 10;
        rootGo.AddComponent<ModularHealthSpaceship>();
        rootGo.AddComponent<WeaponSystemVehicle>();
        rootGo.AddComponent<Grounded>();
      //  rootGo.AddComponent<Spaceship>();
        rootGo.AddComponent<InputBrainBase>();
    //    root.GetChild(0).gameObject.AddComponent<SpaceshipRayDetection>();

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

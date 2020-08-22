using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupATSTComponents : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        rootGo.AddComponent<ModularHealthMachine>();
        rootGo.AddComponent<WeaponSystemVehicle>();
        rootGo.AddComponent<Grounded>();
        rootGo.AddComponent<AIInputMovableVehicle>();

        Rigidbody rb = rootGo.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.angularDrag = 999;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.drag = 10;
        rb.mass = 5;

        CapsuleCollider col = rootGo.AddComponent<CapsuleCollider>();
        col.center += Vector3.up * 1f;
        col.height = 1.4f;
        col.radius = 0.2f;
        rootGo.layer = 10;


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

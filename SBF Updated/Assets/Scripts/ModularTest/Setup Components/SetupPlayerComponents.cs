using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "Setup_Components_Player",
      menuName = "Modular/Components/Setup_Components_Player"
  )]
public class SetupPlayerComponents : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        Rigidbody rb = rootGo.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.angularDrag = 999;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        CapsuleCollider col = rootGo.AddComponent<CapsuleCollider>();
        col.center += Vector3.up * 1f;
        col.height = 1.4f;
        col.radius = 0.2f;
        rootGo.layer = 10;

        rootGo.AddComponent<RollPlayer>();
        rootGo.AddComponent<Vaulting>();
        rootGo.AddComponent<ClimbWall>();
        rootGo.AddComponent<Cover>();
        rootGo.AddComponent<PlayerInput>();
        rootGo.AddComponent<MoveRBBasedOnCamera>();
        rootGo.AddComponent<Grounded>();
        rootGo.AddComponent<Jump>();
        rootGo.AddComponent<ModularHealthOrganism>();
        rootGo.AddComponent<WeaponSystem_Player>();
        rootGo.AddComponent<AddPlayer>();

        Transform[] allChildren = rootGo.GetComponentsInChildren<Transform>();
        //List<Transform> holsters = new List<Transform>();
        //List<Transform> cWeaps = new List<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if(allChildren[i].name.Equals("Head"))
            {
                Senses s = allChildren[i].gameObject.AddComponent<Senses>();
                s.Init(root, "Modular/Determine/Determine_targets_raycast");
                break;
            }
            //else if(allChildren[i].name.Contains("Holster"))
            //{
            //    holsters.Add(allChildren[i]);
            //}
            //else if (allChildren[i].name.Contains("Weapon Holder"))
            //{
            //    cWeaps.Add(allChildren[i]);
            //}
        }
        Animator anim = rootGo.GetComponentInChildren<Animator>();
      //  CreateIK(anim);
        //WeaponManager_Player weaponManager = rootGo.AddComponent<WeaponManager_Player>();
        //weaponManager.defaultWeaponSlug = new string[] { "DC-15", "DC-15x" };

    }

    void CreateIK(Animator a)
    {
        a.gameObject.AddComponent<IK>();
        //GameObject lookTarget = CreateNew(a.transform.parent, "LookTarget");
        //lookTarget.transform.localPosition = new Vector3(0, 1.66f, 2.915f);
        //GameObject rhHolder = CreateNew(a.transform.parent, "Weapon RH holder (1)");
        //GameObject lhHolder = CreateNew(a.transform.parent, "Weapon LH holder (1)");
        //GameObject rh = CreateNew(a.transform.parent, "RHIK");
        //rh.transform.parent = rhHolder.transform;
        //GameObject lh = CreateNew(a.transform.parent, "LHIK");
        //lh.transform.parent = lhHolder.transform;
    }

    GameObject CreateNew(Transform root, string name)
    {
        GameObject aimGO = new GameObject(name);
        aimGO.transform.SetParent(root);
        aimGO.transform.localScale = Vector3.zero;
        aimGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return aimGO;
    }
}

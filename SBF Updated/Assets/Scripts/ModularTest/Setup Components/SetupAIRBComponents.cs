using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu
  (
      fileName = "Setup_Components_AIRB",
      menuName = "Modular/Components/Setup_Components_AIRB"
  )]
public class SetupAIRBComponents : SetupComponents
{
    public override void Setup(Transform root)
    {
        GameObject rootGo = root.gameObject;
        Rigidbody rb = rootGo.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.angularDrag = 999;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        CapsuleCollider col = rootGo.AddComponent<CapsuleCollider>();
        col.center += Vector3.up * 1f;
        col.height = 1.4f;
        col.radius = 0.2f;
        rootGo.layer = 10;

        Animator anim = root.GetComponentInChildren<Animator>();
        anim.applyRootMotion = true;
        anim.gameObject.AddComponent<IK>();
        anim.gameObject.AddComponent<AnimatorHook>();

        AIInput inp = rootGo.AddComponent<AIInput>();
        BrainBase[] b = Resources.LoadAll<BrainBase>("Modular/Brains/Unit/");
        inp.brain = b[Random.Range(0, b.Length)];

        //GameObject agentGO = CreateNew(root, "Agent");
        //agentGO.AddComponent<NavMeshAgent>();


        rootGo.AddComponent<RollAnimAI>();
        rootGo.AddComponent<Grounded>();
        rootGo.AddComponent<MoveRBAgent>();
       // rootGo.AddComponent<AIInput>();
        rootGo.AddComponent<ClimbWallAI>();
        rootGo.AddComponent<VaultingAI>();
        rootGo.AddComponent<JumpAI1>();
        rootGo.AddComponent<CoverAI>();
        rootGo.AddComponent<ModularHealthOrganism>();
        rootGo.AddComponent<WeaponSystem>();
        Transform[] allChildren = rootGo.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name.Equals("Head"))
            {
                Senses s = allChildren[i].gameObject.AddComponent<Senses>();
                s.Init(root, "Modular/Determine/Determine_targets_raycast");
                break;
            }
        }
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

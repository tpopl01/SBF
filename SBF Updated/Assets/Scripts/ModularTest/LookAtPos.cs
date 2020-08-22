using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LookAtPos : MonoBehaviour, IAiming, ISetup
{
    NavMeshAgent agent;
    SensesBase s;
    AIStats ai;
    [SerializeField]float aimThreshold = 10;
    ModularControllerMoveable c;

    public bool GetAiming()
    {

        Quaternion target = StaticMaths.GetLookRotation(s.TargetPos, transform.position, transform.forward, out bool rotate, aimThreshold);
        if (rotate)
        {
            agent.updateRotation = false;
            if(c.OnGround)
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * ai.GetTurnSpeed() * 8);
            return false;
        }
        return true;
    }

    public void SetUp(Transform root)
    {
        c = root.GetComponent<ModularControllerMoveable>();
        agent = root.GetComponentInChildren<NavMeshAgent>();
        s = root.GetComponentInChildren<SensesBase>();
        ai = c.AIStats();
    }
}

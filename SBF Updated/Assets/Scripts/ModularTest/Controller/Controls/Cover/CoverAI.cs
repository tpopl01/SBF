using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverAI : MonoBehaviour, ICover, ISetup, ITick
{
    NavMeshAgent agent;
    bool crouchingCover;
    bool usingCover;
    Vector3 moveAxis;
    bool aiming;
    ModularController controller;
    ModularController target;

    public void SetUp(Transform root)
    {
        agent = root.GetComponentInChildren<NavMeshAgent>();
        controller = root.GetComponent<ModularController>();
    }

    public void Begin()
    {
     //   Collider cover = null;

    //    Vector3 targetSpot = GetHidingPosition(cover, target);
    }

    public bool GetAim()
    {
        return aiming;
    }

    public bool GetCrouchingCover()
    {
        return crouchingCover;
    }

    public bool GetUsingCover()
    {
        return usingCover;
    }

    public void SetMoveAxis(Vector3 moveAxis, bool aiming)
    {
        this.moveAxis = moveAxis;
        this.aiming = aiming;
    }

    public void Tick()
    {

    }

    Vector3 GetHidingPosition(Collider obstacle, ModularController target)
    {
        float d = (obstacle.bounds.extents.x > obstacle.bounds.extents.z) ? obstacle.bounds.extents.x : obstacle.bounds.extents.z;
        float distAway = d + 0.6f;

        Vector3 dir = obstacle.transform.position - target.Position;
        dir.Normalize();

        return obstacle.transform.position + dir * distAway;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnterExitVisualUser : EnterExit
{
    IK unitIK;
    [SerializeField]Transform handR;
    [SerializeField]Transform handL;
    [SerializeField] Transform unitAlignTransform;
    NavMeshAgent a;
    InputBase unitInput;
    public override bool Enter(ModularControllerUnit character, bool player)
    {
        if(base.Enter(character, player))
        {
            unit.gameObject.SetActive(true);
            unitIK = unit.GetComponentInChildren<IK>();
            unitInput = unit.GetComponentInChildren<InputBase>();
            unitInput.enabled = false;
            a = unit.GetComponentInChildren<NavMeshAgent>();
            if(a)
            a.enabled = false;
            unit.GetComponent<WeaponSystem>().DisableGuns();
            Debug.Log("Enter");
            return true;
        }
        return false;
    }

    public override void Exit()
    {
        unit.GetComponent<WeaponSystem>().EnableGuns();
        base.Exit();
        if(a)
        {
            a.enabled = true;
            a = null;
        }
        unitIK = null;
        unitInput.enabled = true;
        unitInput = null;
        Debug.Log("Exit");
    }

    public override void Tick()
    {
        base.Tick();
        if(unit)
        {
            if(unit.Health.IsDead())
            {
                Exit();
                unitIK = null;
                unit = null;
            }
            else
            {
                unit.transform.rotation = unitAlignTransform.rotation;
                unit.transform.position = unitAlignTransform.position;
                unitIK.RightHandTarget(handR.position, handR.eulerAngles, true);
                unitIK.LeftHandTarget(handL.position, handL.eulerAngles, true);
                unitIK.EnableLHIK();
                unitIK.EnableRHIK();
                unitIK.EnableShoulderIK();
                unitIK.EnableAimIK();
                unitIK.DisableIK(false);
                // unit.transform.rotation = transform.rotation;
              //  Debug.Log("Setting Unit");
              //  unitInput.enabled = false;
            }
        }
    }

    //private void LateUpdate()
    //{
    //    if (unit)
    //    {
    //        unit.transform.localRotation = unitAlignTransform.rotation;
    //        unit.transform.localPosition = unitAlignTransform.position;
    //        unitIK.RightHandTarget(handR.position, handR.eulerAngles, true);
    //        unitIK.LeftHandTarget(handL.position, handL.eulerAngles, true);
    //        unitIK.EnableLHIK();
    //        unitIK.EnableRHIK();
    //        unitIK.EnableShoulderIK();
    //        unitIK.EnableAimIK();
    //    }
    //}
}

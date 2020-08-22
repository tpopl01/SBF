using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTargetInRange : MonoBehaviour, IAiming, ISetup, ITick
{
    [SerializeField] float clampAngle = 70;
    [SerializeField] float speed = 1;
    SensesBase s;
    bool aiming;
    Transform root;



    public bool GetAiming()
    {
        return aiming;
    }

    public void SetUp(Transform root)
    {
        s = root.GetComponent<ModularController>().Senses;
        this.root = root;
    }

    public void Tick()
    {
        Vector3 directionToLookTo = s.TargetPos - root.position;
        directionToLookTo.y = 0;
        float angle = Vector3.Angle(root.forward, directionToLookTo);
        if (angle < clampAngle && s.TargetPos != Vector3.zero)
        {
            //    if (angle > 0.01f)
            //  {
            directionToLookTo = s.TargetPos - transform.position;
            angle = Vector3.Angle(transform.forward, directionToLookTo);
          //  if (angle > 5)
          //  {
                Quaternion targetRot = Quaternion.LookRotation(directionToLookTo);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * speed);
            //  }
            if (angle < 5)
            {
                aiming = true;
                return;
            }
          //  }
          //  else
          //  {
          //      aiming = true;
          //      return;
          //  }
        }
        else
        {
            directionToLookTo = root.forward * 30;// - transform.position;
            directionToLookTo.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(directionToLookTo);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * speed);
        }

        aiming = false;
    }
}

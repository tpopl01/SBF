using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsAim : MonoBehaviour, ISetup, ITick, IAiming
{
   // bool aiming;
    SensesBase s;
    [SerializeField] Rotates[] rotates;
    bool aimingAtTarget;

    public bool GetAiming()
    {
        return aimingAtTarget;
    }

  //  public void SetAim(bool aiming)
 //   {
  //      this.aiming = aiming;
  //  }

    public void SetUp(Transform root)
    {
        s = root.GetComponent<ModularController>().Senses;
        if (rotates == null) rotates = new Rotates[1];
        rotates[0] = new Rotates(10, true, transform);
    }

    public void Tick()
    {
    //    if (aiming)
   //     {
            aimingAtTarget = true;
            for (int i = 0; i < rotates.Length; i++)
            {
                if (!TurnToDirection(s.TargetPos, rotates[i]))
                {
                    aimingAtTarget = false;
                    break;
                }
            }
      //  }
    }

    bool TurnToDirection(Vector3 targetPos, Rotates r)
    {
        Vector3 directionToLookTo = targetPos - r.transform.position;
        if(r.zeroY)
            directionToLookTo.y = 0;

        float angle = Vector3.Angle(r.transform.forward, directionToLookTo);

        if (angle > r.threshold)
        {
            r.transform.rotation = Quaternion.Slerp(r.transform.rotation, Quaternion.LookRotation(directionToLookTo), Time.deltaTime * r.speed);
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class Rotates
{
    public float threshold = 10;
    public bool zeroY;
    public float speed = 0.6f;
    public Transform transform;

    public Rotates(float t, bool y, Transform trans, float speed = 0.6f)
    {
        threshold = t;
        zeroY = y;
        transform = trans;
        this.speed = speed;
    }
}

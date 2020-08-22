using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class RollAnim : MonoBehaviour, IRoll, ISetup, IFixedTick, IGetDisableIK
{
  //  protected InputBase inp;
    protected bool rolling;
    protected Rigidbody rigid;
    float roll_t;
    CurveHolder roll_curve;
    protected Animator anim;
    protected string animName = "Roll";
    protected float length = 1.3f;
    protected float speed = 15;

    public void BeginRoll()
    {
        if(!rolling)
        {
            Begin();
        }
    }

    protected virtual void Begin()
    {
       // if (inp.MoveAxis != Vector3.zero)
       // {
            rolling = true;
            anim.Play(animName);
        //}
    }

    public void SetUp(Transform root)
    {
      //  inp = root.GetComponent<InputBase>();
        rigid = root.GetComponent<Rigidbody>();
        anim = root.GetComponentInChildren<Animator>();
        roll_curve = Resources.Load<CurveHolder>("Curves/roll_curve");
        Init(root);
    }

    protected virtual void Init(Transform root)
    {

    }

    public void FixedTick()
    {
        if (rolling)
        {
            rigid.drag = 0;

            roll_t += Time.deltaTime * length;
            if (roll_t > 1)
            {
                roll_t = 1;
                RollFinished();
            }

            float zValue = roll_curve.curve.Evaluate(roll_t);
            Vector3 v1 = Vector3.forward * zValue;
            Vector3 relative = rigid.transform.TransformDirection(v1);
            Vector3 v2 = (relative * speed);

            rigid.velocity = v2;
        }
    }

    public bool GetRolling()
    {
        return rolling;
    }


    protected virtual void RollFinished()
    {
        roll_t = 0;
        rolling = false;
    }

    public bool GetDisableIK()
    {
        return GetRolling();
    }
}

using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class Jump : MonoBehaviour, IJump, ITick, ISetup
{
    Rigidbody rb;
    Animator anims;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpLength = 0.4f;
    bool jumping;
    Timer jumpTimer = new Timer(0.4f);
    [SerializeField] bool onGround;

    public bool GetJumping()
    {
        return jumping;
    }

    public void BeginJump(Transform root, bool onGround)
    {
        if (onGround == this.onGround) return;
        if (rb == null)
        {
            rb = root.GetComponent<Rigidbody>();
            anims = root.GetComponentInChildren<Animator>();
        }
        if(!jumping)
        {
            jumpTimer.StartTimer();
            jumping = true;
            anims.CrossFade("run_jump_take_off", 0.2f);
            Begin();
        }
    }

    public virtual void Begin()
    {

    }

    public void Tick()
    {
        if (jumping)
        {
            rb.drag = 0;
            var vel = rb.velocity;
            vel.y = jumpHeight + (3);
            rb.velocity = vel;
            if (jumpTimer.GetComplete())
            {
                Complete();
            }
        }
    }

    protected virtual void Complete()
    {
        jumping = false;
    }

    public virtual void SetUp(Transform root)
    {
        jumpTimer = new Timer(jumpLength);
        rb = root.GetComponent<Rigidbody>();
        anims = root.GetComponentInChildren<Animator>();
    }
}

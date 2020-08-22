using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularControllerUnit : ModularControllerMoveable
{
    IDisableIK[] iDisableIKs;
    IGetDisableIK[] iGetDisableIK;
    public IJump[] iJump { get; private set; }
    public IRoll[] iRoll { get; private set; }
    public IVault iVault { get; private set; }
    public IClimb iClimb { get; private set; }
    public ICover iCover { get; private set; }
    public Rigidbody rb { get; private set; }
    protected Animator anim;

    [SerializeField] bool debugTeam = true;

    protected override void Initialise()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        base.Initialise();
        iDisableIKs = GetComponentsInChildren<IDisableIK>();
        iGetDisableIK = GetComponentsInChildren<IGetDisableIK>();
        iJump = GetComponentsInChildren<IJump>();
        iRoll = GetComponentsInChildren<IRoll>();
        iVault = GetComponentInChildren<IVault>();
        iClimb = GetComponentInChildren<IClimb>();
        iCover = GetComponentInChildren<ICover>();
        if(debugTeam)
        {
            GameManagerModular.instance.AddUnitToTeam(this);
        }
    }

    protected override void Tick()
    {
        iCover.SetMoveAxis(input.MoveAxis, Aiming);
        bool jump = false;
        for (int i = 0; i < iJump.Length; i++)
        {
            if (iJump[i].GetJumping())
                jump = true;
        }
        grounded.Disable = jump /*|| GetRolling()*/ || iVault.GetVaulting();

        anim.SetBool("OnGround", OnGround);
        float targetStance = (input.Crouch) ? 0 : 1;
        anim.SetFloat("Stance", Mathf.Lerp(anim.GetFloat("Stance"), targetStance, Time.deltaTime * 2));

        rb.drag = (OnGround) ? 5 : 0;
        base.Tick();

        GetDisableIks();
    }


    public bool GetJumping()
    {
        for (int i = 0; i < iJump.Length; i++)
        {
            if (iJump[i].GetJumping()) return true;
        }
        return false;
    }
    public bool GetRolling()
    {
        for (int i = 0; i < iRoll.Length; i++)
        {
            if (iRoll[i].GetRolling()) return true;
        }
        return false;
    }

    void GetDisableIks()
    {
        bool disable = false;
        for (int i = 0; i < iGetDisableIK.Length; i++)
        {
            if (iGetDisableIK[i].GetDisableIK())
                disable = true;
        }

        DisableIK(disable);
    }

    void DisableIK(bool disable)
    {
        for (int i = 0; i < iDisableIKs.Length; i++)
        {
            iDisableIKs[i].DisableIK(disable);
        }
    }
}

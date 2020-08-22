using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

//[CreateAssetMenu
//  (
//      fileName = "Input_Player",
//      menuName = "Modular/Components/Input_Player"
//  )]
public class PlayerInput : InputBase, ISetup
{
    public bool Cover { get; set; }
    public float TurnSpeed { get; set; }
    public bool Roll { get; set; }
    public bool Jump { get; set; }

    Transform cameraTrans;
    Vector3 hashedPos;
    // Check all actions

    LeaderCommands lC;

    private void Start()
    {
        lC = GetComponent<LeaderCommands>();
    }

    public override void Execute(ModularController controller)
    {
        cameraTrans = CameraManager.instance.cameraMain();
        ModularControllerPlayer c = (ModularControllerPlayer)controller;

        controller.Senses.TargetPos = cameraTrans.position + cameraTrans.forward * 30;
        //  if(Physics.Raycast(transform.position, controller.Senses.TargetPos, out RaycastHit hit, 20))
        // {
        //     if(hit.transform != c.transform)
        //         controller.Senses.TargetPos = hit.point;
        //  }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            lC.Follow();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lC.Wait();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lC.MoveToPos();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lC.Dismiss();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(controller.Senses.TargetPos); ((WeaponSystem_Player)controller.weaponSystem).UseSpecial(); }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ((WeaponSystem_Player)controller.weaponSystem).NextSpecial();
        }
        Aim = Input.GetKey(KeyCode.Mouse1);
        Attack = Input.GetKeyDown(KeyCode.Mouse0) && c.Aiming;
        if (Attack) RaycastShoot(controller);

        Cover = Input.GetKeyDown(KeyCode.B);
        if(Input.GetKeyDown(KeyCode.C))
            Crouch = !Crouch;
        Jump = Input.GetKeyDown(KeyCode.Space);
        MoveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Roll = Input.GetKeyDown(KeyCode.Z);
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ((WeaponSystem)controller.weaponSystem).EquipHolster();
        }
        Sprint = Input.GetKey(KeyCode.LeftShift);

        if (c.iCover.GetUsingCover()) Crouch = c.iCover.GetCrouchingCover();
        Speed = Sprint ? controller.AIStats().GetSprintSpeed() : controller.AIStats().GetRunSpeed();
        if (Crouch || c.iCover.GetUsingCover()) Speed = controller.AIStats().GetWalkSpeed();
        TurnSpeed = controller.AIStats().GetTurnSpeed();

        HandleInput(c);

        if (hashedPos == transform.position && !c.OnGround && !c.grounded.Disable)
        {
            transform.position = Vector3.Slerp(controller.transform.position, controller.transform.position + controller.transform.forward, Time.deltaTime);
        }
        hashedPos = transform.position;
        if(Input.GetKeyDown(KeyCode.E))
        {
            IEnter e = ((Senses)controller.Senses).NearestVehicle();
            if (e != null && e.Enter(c, true))
            {
                
            }
            else
            {
                IWeaponSystomChangeable w = (IWeaponSystomChangeable)controller.weaponSystem;
                IPickable p = ((Senses)controller.Senses).NearestWeapon();
                if (p != null) p.Pickup(w, transform.position);
                ((Senses)controller.Senses).collectWeapon = null;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            IWeaponSystomChangeable w = (IWeaponSystomChangeable)controller.weaponSystem;
            w.Reload();
        }
        ModularCommandPostCapturable g = ResourceManagerModular.instance.GetCaptureCP(controller.Position);
        if (g)
        {
            if(g.InRange(transform.position))
            {
                captureBar.Capture(g.CaptureAmount, g.GetMaxCapture());
            }
        }
    }

    void RaycastShoot(ModularController c)
    {
        c.Senses.TargetPos = cameraTrans.position + cameraTrans.forward * 100;
        c.weaponSystem.Attack(c.Senses.TargetPos, c, c.Senses.ClosestEnemy);
    }

    void Repair(ModularController c)
    {
        IWeaponSystomChangeable w = (IWeaponSystomChangeable)c.weaponSystem;
        ISpecial s = w.GetSpecialOfType(SpecialType.Repair);
        if (s != null)
        {
         //   IRepair r = ((Senses)c.Senses).NearestRepair(c.Team);
          //  if (r != null)
          //  {
                s.Use();
           // }
        }
    }

    void HandleInput(ModularControllerPlayer c)
    {
        if (c.OnGround)
        {
            if ((Aim && !c.iCover.GetUsingCover()) || (Aim && c.iCover.GetAim()))
            {
                Speed = c.AIStats().GetWalkSpeed();
                TurnSpeed = c.AIStats().GetTurnSpeed() / 1.2f;
                if (Attack)
                {
                    //Handle Attack Module
                }
            }
            else if (Jump)
            {
                c.iVault.BeginVault();
                if (!c.iVault.GetVaulting())
                {
                    c.iClimb.BeginClimb();
                    if (!c.iClimb.GetClimbing())
                        for (int i = 0; i < c.iJump.Length; i++)
                        {
                            c.iJump[i].BeginJump(c.transform, c.OnGround);
                        }
                }
            }
            else if (Roll)
            {
                for (int i = 0; i < c.iRoll.Length; i++)
                {
                    c.iRoll[i].BeginRoll();
                }
                
            }
            else if (Cover)
            {
                c.iCover.Begin();
            }

            if (!c.iCover.GetUsingCover())
                c.Move(MoveAxis, Speed);
        }
        else
        {
            if(Jump && !c.iClimb.GetClimbing() && !c.iVault.GetVaulting())
            {
                for (int i = 0; i < c.iJump.Length; i++)
                {
                    c.iJump[i].BeginJump(c.transform, c.OnGround);
                }
            }
            else if (Roll)
            {
                for (int i = 0; i < c.iRoll.Length; i++)
                {
                    c.iRoll[i].BeginRoll();
                }

            }
        }
    }

    CaptureBar captureBar;
    public void SetUp(Transform root)
    {
        cameraTrans = CameraManager.instance.cameraMain();
        captureBar = GameObject.FindObjectOfType<CaptureBar>();
        captureBar.Init(100);
    }
}

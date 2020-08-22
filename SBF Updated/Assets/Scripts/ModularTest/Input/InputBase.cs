using System.Collections;
using System.Collections.Generic;
using tpopl001.Weapons;
using UnityEngine;

// use interfaces so the modular controller can have different levels
public abstract class InputBase : MonoBehaviour
{

    public Vector3 MoveAxis { get; set; }
    public float Speed { get; set; }
    public bool Aim { get; set; }
    public bool Sprint { get; set; }
    public bool Crouch { get; set; }
    public bool Attack { get; set; }
   // public Vector3 TargetPos { get; protected set; }

    public abstract void Execute(ModularController controller);


    //public abstract bool GetCover();
    //public abstract bool GetAttack();
    //public abstract bool GetAim();
    //public abstract bool GetSprint();
    //public abstract bool GetRoll();
    //public abstract bool GetJump();
    //public abstract bool GetSpecial();
    //public abstract SpecialBase GetSelectSpecial();
    //public abstract bool GetSwitchWeapon();
    //public abstract Weapon GetSelectWeapon();
    //public abstract bool GetCrouch();
    //public abstract bool GetVault();
    //public abstract Vector3 GetMoveAxis();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.Weapons;

public class FPWeapon : Gun
{
    Animator anim;

    private void Awake()
    {
        Initialise();
    }

    public override void Initialise()
    {
        base.Initialise();
        anim = GetComponent<Animator>();
        pickUpSlug = name + "_pickable";
    }

    protected override void Tick()
    {
        base.Tick();
        Aiming(Input.GetKey(KeyCode.Mouse1));
    }

    public Vector3 GetFPAimPos()
    {
        return GetGunStats().GetFPAimPos();
    }

    public Vector3 GetFPAimRot()
    {
        return GetGunStats().GetFPAimRot();
    }

    public Vector3 GetFPRot()
    {
        return GetGunStats().GetFpRot();
    }

    public Vector3 GetFPPos()
    {
        return GetGunStats().GetFpPos();
    }

    public float GetAimSpeed()
    {
        return GetGunStats().GetAimSpeed();
    }

    void Aiming(bool aiming)
    {
        Vector3 target = GetFPPos();
        Vector3 targetRot = GetFPRot();
        if (aiming)
        {
            target = GetFPAimPos();
            targetRot = GetFPAimRot();
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * GetAimSpeed());
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * GetAimSpeed());
    }

    public override bool Reload()
    {
        if (reloadTimer.GetComplete())
        {
            if (totalAmmo > 0 && ammoInClip < GetGunStats().GetMaxAmmoInClip())
            {
                anim.SetFloat("reloadSpeed", 1 / GetGunStats().GetReloadTime());
                reloading = true;
                anim.Play("Reload");
                GetGunStats().PlayReload(shootAS);
                int ammoToAdd = GetGunStats().GetMaxAmmoInClip() - ammoInClip;
                totalAmmo -= ammoToAdd;
                if (totalAmmo < 0)
                {
                    ammoToAdd -= totalAmmo;
                    totalAmmo = 0;
                }
                ammoInClip += ammoToAdd;
                reloadTimer.StartTimer();
                return true;
            }
        }
        return false;
    }

    public void FP_PlayPowerAnim()
    {
        anim.SetBool("Power", true);
        anim.CrossFade("Power_To", 0.2f);
    }

    public void FP_PlayPowerAnimOneShot()
    {
        anim.CrossFade("Power_To", 0.1f);
    }

    public void FP_StopPowerAnim()
    {
        anim.SetBool("Power", false);
    }

    public void FP_PlayComeAnim()
    {
        anim.CrossFade("Come", 0.2f);
    }

    public void FP_PlayOverThereAnim()
    {
        anim.CrossFade("Over There", 0.2f);
    }

    public void FP_PlayMoveOutAnim()
    {
        anim.CrossFade("Move Out", 0.2f);
    }

    public void FP_PlayHoldAnim()
    {
        anim.CrossFade("Stop", 0.2f);
    }

    public void FP_PlayChangeAnim()
    {
        anim.CrossFade("Change", 0.2f);
    }

}

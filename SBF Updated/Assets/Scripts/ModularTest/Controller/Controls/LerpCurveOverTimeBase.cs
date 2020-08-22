using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class LerpCurveOverTimeBase : MonoBehaviour
{
    protected bool inProgress;
    float speed = 5;
    CurveHolder curve = null;
    float timer = 0;
    float forceOverLife;
    Vector3 startPos;
    protected Vector3 targetPos;
    protected Collider col;
    string[] animStrings = new string[] { "vault_over_walk_1", "vault over walk 2", "vault_over_run" };
    Animator anims;
    Timer coolDownTimer = new Timer(1);
    float alterPosBasedOnCurve = 0;

    protected virtual void Init(Transform root, string[] animations, CurveHolder curve, float alterPosOnCurve = 0)
    {
        col = root.GetComponent<Collider>();
        anims = root.GetComponentInChildren<Animator>();
        animStrings = animations;
        this.curve = curve;
        alterPosBasedOnCurve = alterPosOnCurve;
    }

    protected void SetSpeedModifier(float mod)
    {
        speed = mod;
    }

    protected bool CanStart()
    {
        return !(inProgress || coolDownTimer.GetComplete() == false);
    }

    public float GetCompleteAmount()
    {
        return timer;
    }

    protected virtual void Begin(Vector3 targetPos)
    {
        if (CanStart() == false) return;

        this.startPos = col.transform.position;
        this.targetPos = targetPos;
        forceOverLife = Vector3.Distance(col.transform.position, targetPos);
        inProgress = true;
        anims.Play(animStrings[Random.Range(0, animStrings.Length)]);
        col.isTrigger = true;
    }

    protected void OnTick()
    {
        if (!inProgress) return;

        float targetSpeed = speed * curve.curve.Evaluate(Time.time);
        timer += Time.deltaTime * targetSpeed / forceOverLife;

        if (timer > 1)
        {
            timer = 1;
            col.transform.position = targetPos;// Vector3.Slerp(startPos, targetPos, timer);
            timer = 0;
            Stop();
            return;
        }
        Vector3 targetPosition = Vector3.Slerp(startPos, targetPos, timer);
        targetPosition += Vector3.up * alterPosBasedOnCurve * curve.curve.Evaluate(timer);
        col.transform.position = targetPosition;

        Vector3 lookDir = targetPos - startPos;
        lookDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        col.transform.rotation = Quaternion.Slerp(col.transform.rotation, targetRot, Time.deltaTime * 5);
    }

    protected virtual void Stop()
    {
        inProgress = false;
        col.isTrigger = false;
        coolDownTimer.StartTimer();
    }
}

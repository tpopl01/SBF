using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMovementAnims : MonoBehaviour
{
    private string runAnimation = "CameraRun";
    private string idleAnimation = "IdleAnimation";
    private float adjustAnimSpeed = 7.0f;
    public Animation cameraAnimations;
    public Animation walkRunAnim;

    void Start()
    {
        walkRunAnim.wrapMode = WrapMode.Loop;
        walkRunAnim.Stop();
        cameraAnimations[runAnimation].speed = 0.8f;
    }

    public void StartAnims()
    {
        if (!walkRunAnim.isPlaying)
            walkRunAnim.Play();
        if (!cameraAnimations.isPlaying)
            cameraAnimations.Play();
    }

    public void StopAnims()
    {
        if (walkRunAnim.isPlaying)
            walkRunAnim.Stop();
        if (cameraAnimations.isPlaying)
            cameraAnimations.Stop();
    }

    public void MoveAimAnims()
    {
        walkRunAnim.CrossFade(idleAnimation);
        cameraAnimations.CrossFade(idleAnimation);
    }

    public void MoveWalkAnims(float velMagnitude)
    {
        walkRunAnim["Walk"].speed = velMagnitude / adjustAnimSpeed;
        walkRunAnim.CrossFade("Walk");
    }

    public void MoveRunAnims()
    {
        walkRunAnim.CrossFade("Run");
        cameraAnimations.CrossFade(runAnimation);
    }
}

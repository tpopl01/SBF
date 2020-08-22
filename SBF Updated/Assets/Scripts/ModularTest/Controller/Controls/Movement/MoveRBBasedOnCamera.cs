using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRBBasedOnCamera : MonoBehaviour, IMove, ISetup, IAim
{
    InputBase input;
    Rigidbody rb;
    Transform cameraTransform;
    Animator anims;
    float maxSpeed;
    bool aiming;

    public void SetUp(Transform root)
    {
        ModularController controller = root.GetComponent<ModularController>();
        maxSpeed = controller.AIStats().GetSprintSpeed();
        input = root.GetComponent<InputBase>();
        anims = root.GetComponentInChildren<Animator>();
        rb = root.GetComponent<Rigidbody>();
        //if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        //rb.isKinematic = false;
        //rb.angularDrag = 999;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        cameraTransform = CameraManager.instance.transform;
    }

    public void Move(Vector3 input, float speed)
    {
        rb.drag = 5;
        Vector3 v = cameraTransform.forward * input.z;
        Vector3 h = cameraTransform.right * input.x;
        v.y = 0;
        h.y = 0;
        Vector3 vel = (h + v).normalized * speed;
        rb.velocity = vel;
        MovementAnimations("Forward", anims, input, speed);
        if(aiming)
        {
            Aiming(speed);
        }
        else
        {
            Normal(input, speed);
        }
    }

    void MovementAnimations(string v, Animator anim, Vector3 input, float speed)
    {
        float forward = Mathf.Clamp01(Mathf.Abs(input.x) + Mathf.Abs(input.z)) * (speed / maxSpeed);
        anim.SetFloat(v, forward);
    }

    void Aiming(float speed)
    {
        Vector3 targetDir = cameraTransform.forward * 20;

        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * speed);
        transform.rotation = targetRotation;
    }
    void Normal(Vector3 input, float speed)
    {
        Vector3 v = cameraTransform.forward * input.z;
        Vector3 h = cameraTransform.right * input.x;
        v.y = 0;
        h.y = 0;
        Vector3 targetDir = (v + h);
        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * Mathf.Clamp01(Mathf.Abs(input.x) + Mathf.Abs(input.z)) * speed);
    }

    public void SetAim(bool aiming)
    {
        this.aiming = aiming;
    }
}

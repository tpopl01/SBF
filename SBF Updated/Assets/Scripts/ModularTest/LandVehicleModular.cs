using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LandVehicleModular : MonoBehaviour, IMove, IFixedTick, ISetup
{
    Rigidbody rB;
    // KnockBackEnteredColliders knockBack;
    float minAngularDrag;
    public float maxAngularDrag = 30;
    float speed;
    float curDistance;
    public float heightPos = 1;

    float hori;
    float ver;

    float drag = 5;

    Grounded grounded;

    // float mousXAxis;

    [SerializeField] Transform wheelR = null;
    [SerializeField] Transform wheelL = null;
    [SerializeField] Transform wheelHandleR = null;
    [SerializeField] Transform wheelHandleL = null;
    // [SerializeField] Transform modelTrans = null;

    [SerializeField] AudioProfileGeneralVolume engineAudio;
    AudioSource aS;
    Vector3 input;
    bool onGround = true;
    float speedBoost = 0;
    EnterExit e;

    public void SetUp(Transform root)
    {
        e = GetComponent<EnterExit>();
       // a = GetComponentInChildren<NavMeshAgent>();
        grounded = GetComponent<Grounded>();
        rB = GetComponent<Rigidbody>();
        rB.drag = 5;
        rB.angularDrag = 25f;
        minAngularDrag = rB.angularDrag;
        speed = 0;
        rB.isKinematic = false;
        aS = GetComponent<AudioSource>();
    }

    public void Move(Vector3 input, float speed)
    {
        if(!e.Player)
        {
            return;
        }

        HandleMovement(input);
        this.input = input;
        this.speedBoost = speed;
    }

    void HandleGround()
    {
        onGround = grounded.IsGrounded();
        if (onGround)
        {
            rB.drag = drag;
            grounded.AlignToGround(5, 0, true);
        }
        else
        {
            rB.drag = 0;
        }
    }

    public void FixedTick()
    {
        if (!e.Player)
        {
            if (e.CanEnter())
            {
                HandleMovement(Vector3.zero);
            }
          //  return;
        }

        wheelL.Rotate(Vector3.right, speed * 10);
        wheelR.Rotate(Vector3.right, speed * 10);
        if (!onGround)
            return;

        speed = Mathf.Clamp01(speed);
        rB.angularDrag = Mathf.Clamp(rB.angularDrag, minAngularDrag, maxAngularDrag);

        Vector3 targetRot = Vector3.up * hori * 5000 * Time.deltaTime;
        UpdateVisualRotations(targetRot);
        rB.AddForce(transform.forward * speed * speedBoost / Time.deltaTime, ForceMode.Acceleration);
        rB.AddTorque(targetRot, ForceMode.Acceleration);
    }

    void UpdateVisualRotations(Vector3 targetRot)
    {
        float mult = (maxAngularDrag / rB.angularDrag) * 0.1f;
        wheelHandleL.localRotation = Quaternion.Slerp(wheelHandleL.localRotation, Quaternion.Euler(mult * targetRot), .1f);
        wheelHandleR.localRotation = Quaternion.Slerp(wheelHandleL.localRotation, Quaternion.Euler(mult * targetRot), .1f);
    }

    private void HandleMovement(float horizontalAxis, float verticalAxis)
    {
        // base.HandleMovement(horizontalAxis, verticalAxis);
        if (verticalAxis > 0)
        {
            speed += Time.deltaTime / 3;
            rB.angularDrag += Time.deltaTime;

        }
        else
        {
            speed -= Time.deltaTime / 2;
            if (speed < 0)
            {
                speed = 0;
            }
            rB.angularDrag -= Time.deltaTime;
        }
        hori = horizontalAxis;
    }

    private void HandleMovement(Vector3 axis)
    {
        HandleGround();
        if (onGround)
        {
            HandleMovement(axis.x, axis.z);
        }
    }

}

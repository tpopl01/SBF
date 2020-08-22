using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
    Animator animator;
   // Rigidbody rb;
    public bool DisableRootMovement { get; set; } = false;
    public Vector3 DeltaPosition { get; private set; }

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        animator = GetComponent<Animator>();
      //  rb = GetComponentInParent<Rigidbody>();
    }

    private void OnAnimatorMove()
    {
        if (DisableRootMovement) return;
        DeltaPosition = animator.deltaPosition / Time.deltaTime;
    }
}

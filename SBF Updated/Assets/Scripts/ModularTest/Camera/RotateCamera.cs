using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    float turnSmoothing = .1f;
    [SerializeField] float minAngle = -35;
    [SerializeField] float maxAngle = 35;

    float smoothX;
    float smoothY;
    float smoothXvelocity;
    float smoothYvelocity;
    float lookAngle;
    float tiltAngle;
    float targetSpeed = 2;


    public void Rotate(float mouseX, float mouseY, Transform pivot, float minAngle = -35, float maxAngle = 35)
    {
        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYvelocity, turnSmoothing);
        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;
        }

        tiltAngle -= smoothY * targetSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        if (mouseX != 0)
        {
            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);
        }
        else
        {
            lookAngle = transform.eulerAngles.y;
        }
    }

}

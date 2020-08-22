using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    [SerializeField] LayerMask mask = ~(1 << 10);
    public bool Disable { get; set; }
    public bool IsGrounded(float offset = 0, float dist = 2)
    {
        if (Disable)
        {
            return false;
        }
        if (Physics.Raycast(transform.position + transform.up * 1f, -transform.up, out RaycastHit hit, dist))
        {
            Vector3 tPos = transform.position;
            tPos.y = hit.point.y + offset;
            transform.position = tPos;//hit.point + Vector3.up * offset;
            return true;
        }
        return false;
    }

    public void AlignToGround(float alignDist = 2, float offset = 0, bool useZ = false, float maxAngle = 50)
    {
        if (Disable)
        {
            return;
        }
        Vector3 targetRot = transform.eulerAngles;
        RaycastHit bHit;
        if (!useZ)
        {
            if (Physics.Raycast(transform.position + (transform.up * 0.5f) - transform.forward * 0.5f, -transform.up, out bHit, alignDist, mask))
            {
                if (Physics.Raycast(transform.position + (transform.up * 0.5f) + transform.forward * 0.5f, -transform.up, out RaycastHit fHit, alignDist, mask))
                {
                    Vector3 angle = fHit.point - bHit.point;
                    angle.y += offset;
                    targetRot.x = angle.x;

                    transform.forward = targetRot;// Vector3.Slerp(transform.forward, targetRot, Time.fixedDeltaTime * 5);
                    targetRot = transform.eulerAngles;
                    if (targetRot.x > maxAngle && targetRot.x < 360 - maxAngle)
                    {
                        if (Mathf.Abs(360 - maxAngle - targetRot.x) < Mathf.Abs(maxAngle - targetRot.x))
                        {
                            targetRot.x = 360 - maxAngle;
                        }
                        else
                        {
                            targetRot.x = maxAngle;
                        }
                    }

                    transform.rotation = Quaternion.Euler(targetRot);
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + transform.up * 1f, -transform.up, out RaycastHit hit, alignDist, mask))
            {
               // transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime * 10);
            }
        }
    }
}

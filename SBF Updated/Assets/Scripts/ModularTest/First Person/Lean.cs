using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lean : MonoBehaviour
{
    float smoothTime = 5f;
    float camZClamp = 30f;
    float move = 1f;


    public void HandleLean(Vector3 player, Vector3 offset, bool aiming)
    {
        if(!aiming)
        {
            StopLeaning();
            return;
        }
        Vector3 dir = transform.forward;
        Vector3 origin = transform.position + transform.right * offset.x + transform.up * offset.y + transform.right * offset.z;
        Debug.DrawRay(player + offset, dir);
        if (Physics.Raycast(player + offset, dir, 2))
        {
            if (Physics.Raycast(origin + transform.right * 0.5f, dir, 2))
            {
                LeanRight();
            }
            else if (Physics.Raycast(origin - transform.right * 0.5f, dir, 2))
            {
                LeanLeft();
            }
        }
        else
        {
            StopLeaning();
        }
    }

    void LeanLeft()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, camZClamp)), Time.deltaTime * smoothTime);
        transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.right * -move, Time.deltaTime * smoothTime);
    }

    void LeanRight()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, -camZClamp)), Time.deltaTime * smoothTime);
        transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.right * move, Time.deltaTime * smoothTime);
    }

    void StopLeaning()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, 0f)), Time.deltaTime * smoothTime);
        transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, Time.deltaTime * smoothTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverBehaviour : MonoBehaviour
{
    Animator anims;
    bool crouchCover;
    int coverDir;
    bool aimAtSides;
    Rigidbody rb;
    Vector3 targetPos;
    Quaternion targetRotation;
    CoverPosition cp;
    bool hasCover;
    float _t;
    Vector3 _startPos;
    float _length;
    Vector3 _targetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anims = GetComponentInChildren<Animator>();
    }

    void GetInCoverLerp()
    {
        if (!hasCover)
        {
            _length = Vector3.Distance(cp.pos1, cp.pos2);
            float hitDistance = Vector3.Distance(cp.initialHit, cp.pos1);
            float coverPerc = hitDistance / _length;
            _targetPos = Vector3.Lerp(cp.pos1, cp.pos2, coverPerc);
            _startPos = transform.position;
            _t = 0;

            crouchCover = !isCoverFull();
            coverDir = 1;
        }

        float movement = 2 * Time.deltaTime;
        float lerpMovement = movement / _length;
        _t += movement;

        if (_t > 1)
        {
            _t = 1;
            hasCover = true;
        }

        Vector3 tp = Vector3.Lerp(_startPos, _targetPos, _t);
        tp.y = transform.position.y; //stops player being above ground
        transform.position = tp;

        //make player look the same way as the helper transform
       // Quaternion targetRot = Quaternion.LookRotation(helper.transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _t);
    }

    public bool RaycastForCover(Collider col)
    {
        Vector3 origin = transform.position + Vector3.up / 2 + -transform.forward;
        Vector3 direction = transform.forward;
        RaycastHit hit;
        float distance = 3;

        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            BoxCollider b = hit.transform.GetComponent<BoxCollider>();
            if (b)
            {
                targetPos = PosWithOffset(origin, hit.point);
                targetRotation = Quaternion.LookRotation(-hit.normal);

                Vector3 extent = new Vector3(b.transform.position.x + b.bounds.extents.x, transform.position.y, hit.point.z);
                Vector3 extent1 = new Vector3(b.transform.position.x + -b.bounds.extents.x, transform.position.y, hit.point.z);

                if (Mathf.Abs(hit.point.z - (b.transform.position.x + b.bounds.extents.x)) < Mathf.Abs(hit.point.x - (b.transform.position.z + b.bounds.extents.z)))
                {
                    extent = new Vector3(hit.point.x, transform.position.y, b.transform.position.z + b.bounds.extents.z);
                    extent1 = new Vector3(hit.point.x, transform.position.y, b.transform.position.z + -b.bounds.extents.z);
                }

                cp = new CoverPosition(extent, extent1);

                //raycast from left and right position of helper to make sure it hits cover
                bool right = isCoverValid(transform.right, transform.forward, true);
                bool left = isCoverValid(transform.right, transform.forward, false);

                //the cover is at least the minimun size
                if (right && left)
                {
                    cp.initialHit = hit.point;
                    EnterCover(col);
                    return true;
                }
            }
        }
        return false;
    }

    void EnterCover(Collider col)
    {
        col.isTrigger = true;
        hasCover = true;
    }

    public void LeaveCover(Collider col)
    {
        col.isTrigger = false;
        anims.SetBool("Cover", false);
        anims.SetInteger("CoverDirection", 0);
        anims.SetBool("CrouchToUpAim", false);
        hasCover = false;
    }


    public void HandleCoverMovement(float horizontal)
    {
        if (!hasCover)
        {
            GetInCoverLerp();
            return;
        }

        bool movePositive = (horizontal > 0);
        if (horizontal != 0)
        {
            coverDir = (movePositive) ? 1 : -1;
            crouchCover = !isCoverFull();

            if (crouchCover)
            {
                //TODO FORCE PLAYER TO CROUCH
                //crouching = crouchCover;
            }
            else
            {
                //TODO FORCE PLAYER TO STOP CROUCHING
            }
        }

        bool isCover = CanMoveOnSide(movePositive);
        if (!isCover)
            isCover = CanMoveOnSide(movePositive, 0.1f);

        Vector3 targetDir = (targetPos - transform.position).normalized;
        targetDir *= Mathf.Abs(horizontal);
        if (!isCover)
        {
            targetDir = Vector3.zero;
            horizontal = 0;
        }

        aimAtSides = !isCover;

        rb.AddForce(targetDir * 8);
        Quaternion targetRot = targetRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5);
        HandleCoverAnim(horizontal);
    }

    void HandleCoverAnim(float input)
    {
        anims.SetFloat("vertical", Mathf.Abs(input), 0.3f, Time.deltaTime);
        anims.SetBool("Cover", true);
        anims.SetInteger("CoverDirection", coverDir);
        anims.SetBool("CrouchToUpAim", crouchCover);
    }

    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * 0.6f;
        Vector3 retVal = target + offset;
        return retVal;
    }

    bool isCoverValid(Vector3 rightV, Vector3 forward, bool right)
    {
        bool retVal = false;

        Vector3 side = (right) ? rightV : -rightV;
        side *= 0.2f;
        Vector3 origin = targetPos + side + -forward;
        Vector3 direction = forward;
        RaycastHit hit;

        Debug.DrawRay(origin, direction * 2);

        if (Physics.Raycast(origin, side, out hit, 0.2f))
        {
            //if there is an obstacle on th left or right the cover is invalid
            return false;
        }
        else //if not do another raycast to determine the size of the collider
        {
            RaycastHit towards;
            origin += side;

            if (Physics.Raycast(origin, direction, out towards, 3))
            {
                //if we hit a collider it is a viable pos from this side
                if (towards.transform.GetComponent<BoxCollider>())
                {
                    retVal = true;
                    if (right)
                    {
                        cp.pos2 = PosWithOffset(origin, towards.point);
                    }
                    else
                    {
                        cp.pos1 = PosWithOffset(origin, towards.point);
                    }
                }
            }
            else
            {
                return false;
            }
        }
        return retVal;
    }

    bool isCoverFull()
    {
        bool retVal = false;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, 1))
        {
            if (hit.transform.GetComponent<BoxCollider>())
            {
                retVal = true;
            }
        }
        crouchCover = !retVal;
        return retVal;
    }

    bool CanMoveOnSide(bool right, float offset = 0)
    {
        bool retVal = false;

        Vector3 side = (right) ? transform.right : -transform.right;
        side *= 0.25f + offset;
        Vector3 origin = transform.position + side;
        origin += Vector3.up / 2;
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, side, out hit, 0.2f))
        {
            return false;
        }
        else
        {
            RaycastHit towards;
            origin += side;

            if (Physics.Raycast(origin, direction, out towards, 1))
            {
                //if we hit a collider that means it's a viable cover position from this side
                if (towards.transform.GetComponent<BoxCollider>())
                {
                    float angle = Vector3.Angle(transform.forward, -towards.normal);

                    if (angle < 45)
                    {
                        retVal = true;
                        targetPos = PosWithOffset(origin, towards.point);
                        targetRotation = Quaternion.LookRotation(-towards.normal);
                    }
                }
            }
            else
            {
                return false;
            }
        }
        return retVal;
    }
}
public class CoverPosition
{
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 initialHit;

    public CoverPosition(Vector3 pos1, Vector3 pos2)
    {
        this.pos1 = pos1;
        this.pos2 = pos2;
    }
}
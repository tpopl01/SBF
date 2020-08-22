using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour, ICover, ISetup, ITick
{
    bool inCover;
    Animator anims;
    bool crouchCover;
    bool aimAtSides;
    Rigidbody rb;
    Vector3 targetPos;
    Quaternion targetRotation;
    CoverPos cp;
    float _t;
    Vector3 _startPos;
    float _length;
    Vector3 _targetPos;
    Transform helper;
    bool init;
    bool movePositive;
    int coverDirection;
    bool canAim;

    Vector3 moveAxis;

    Collider col;
    GameObject debugCube;
    bool debugCover = true;
    bool aimInput;
    bool aiming;


    public void SetUp(Transform root)
    {
        helper = new GameObject().transform;
        helper.name = "Cover Helper";
        //helper.SetParent(root);
        col = root.GetComponent<Collider>();
        rb = root.GetComponent<Rigidbody>();
        anims = root.GetComponentInChildren<Animator>();
        cp = new CoverPos();

        if (debugCover)
        {
            debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 scale = Vector3.one * 0.2f;
            debugCube.transform.localScale = scale;
            Destroy(debugCube.GetComponent<BoxCollider>());
        }
    }

    public void Begin()
    {
        if (inCover) return;

        if (RaycastForCover())
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;

            _length = Vector3.Distance(cp.pos1, cp.pos2);
          //  float hitDistance = Vector3.Distance(cp.initialHit, cp.pos1);
            //float coverPerc = hitDistance / _length;
            _targetPos = PosWithOffset(col.transform.position, cp.initialHit);// Vector3.Lerp(cp.pos1, cp.pos2, coverPerc);
            _startPos = col.transform.position;
            init = false;
            _t = 0;

            crouchCover = !isCoverFull();
            coverDirection = 1;
        }
    }

    public void Tick()
    {
        if (inCover == false) return;
        if(!init)
        {
            float movement = 2 * Time.deltaTime;
            _t += movement;

            if (_t > 1)
            {
                _t = 1;
                init = true;
            }

            Vector3 tp = Vector3.Lerp(_startPos, _targetPos, _t);
            tp.y = transform.position.y; //stops player being above ground
            transform.position = tp;

            //make player look the same way as the helper transform
            Quaternion targetRot = Quaternion.LookRotation(helper.transform.forward);
            col.transform.rotation = Quaternion.Slerp(col.transform.rotation, targetRot, _t);
        }
        else
        {
            HandleCoverMovement();

            if (debugCover)
            {
                debugCube.transform.position = helper.position;
                debugCube.transform.rotation = helper.rotation;
            }
        }
    }

    void HandleCoverMovement()
    {
        Vector3 relativeInput = new Vector3(moveAxis.x, 0, moveAxis.z);

        if (relativeInput.z < 0)
        {
            End();
            return;
        }

        if (relativeInput.x != 0)
        {
            movePositive = (relativeInput.x > 0);
            coverDirection = (movePositive) ? 1 : -1;
            crouchCover = !isCoverFull();
        }

        bool isCover = CanMoveOnSide(movePositive);
        if (!isCover)
            isCover = CanMoveOnSide(movePositive, 0.1f);

        Vector3 targetDir = (helper.position - transform.position).normalized;
        targetDir *= Mathf.Abs(relativeInput.x);
        if (!isCover)
        {
            targetDir = Vector3.zero;
            relativeInput.x = 0;
            HandleAimAnims(true);
        }
        else
            HandleAimAnims(false);

        aimAtSides = !isCover;
        canAim = !isCover;
        if(!aiming)
            rb.AddForce(targetDir * 8);
        //targetDir.y = col.transform.position.y;
        //targetDir *= 100;
        if (targetDir != Vector3.zero)
        {
            Vector3 t = transform.position + helper.right * coverDirection * 100;// (helper.position - transform.position).normalized * 30;
            t.y = transform.position.y;
            Quaternion targetRot = Quaternion.LookRotation(t);// helper.rotation;
            col.transform.rotation = Quaternion.Slerp(col.transform.rotation, targetRot, Time.deltaTime * 10f);
        }
       // col.transform.LookAt(col.transform.position + targetDir);
        HandleCoverAnim(relativeInput);
    }

    Vector3 origin;
    Vector3 tempPos;
    bool hasLerped;
    void HandleAimAnims(bool sideAim)
    {
        if (aimInput)
        {
            if (sideAim)
            {
                transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime * 5);
                hasLerped = true;
            }
            else if (crouchCover)
            {
                anims.SetFloat("Stance", Mathf.Lerp(anims.GetFloat("Stance"), 1, Time.deltaTime * 10));
            }
            transform.rotation = helper.rotation;
            anims.SetBool("Aim", true);
            aiming = true;
        }
        else
        {
            if (hasLerped)
            {
                transform.position = Vector3.Lerp(transform.position, origin, Time.deltaTime * 5);
                if (Vector3.Distance(transform.position, origin) < 0.01f)
                    hasLerped = false;
            }
            aiming = false;
            anims.SetBool("Aim", false);
        }
    }

    void HandleCoverAnim(Vector3 input)
    {
        anims.SetFloat("Forward", Mathf.Abs(input.x) * 0.3f, 0.3f, Time.deltaTime);
     //   anims.SetBool("Cover", inCover);
      //  anims.SetInteger("CoverDirection", coverDirection);
       // anims.SetBool("CrouchToUpAim", crouchCover);
    }

    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * 0.6f;
        Vector3 retVal = target + offset;
        return retVal;
    }

    bool RaycastForCover()
    {
        Vector3 origin = col.transform.position + Vector3.up / 2;// + -col.transform.forward;
        Vector3 direction = col.transform.forward;
        RaycastHit hit;
        float distance = 3;

        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            if (hit.transform.GetComponent<BoxCollider>())
            {
                helper.transform.position = PosWithOffset(origin, hit.point);
                helper.transform.rotation = Quaternion.LookRotation(-hit.normal);

                //raycast from left and right position of helper to make sure it hits cover
                bool right = isCoverValid(helper, true);
                bool left = isCoverValid(helper, false);

                //the cover is at least the minimun size
                if (right && left)
                {
                    inCover = true;
                    cp = new CoverPos();
                    cp.initialHit = hit.point;
                    return true;
                }
            }
        }
        return false;
    }

    bool isCoverValid(Transform h, bool right)
    {
        bool retVal = false;

        Vector3 side = (right) ? h.right : -h.right;
        side *= 0.2f;
        Vector3 origin = h.transform.position + side + -h.transform.forward;
        Vector3 direction = h.transform.forward;

        Debug.DrawRay(origin, direction * 2);

        if (Physics.Raycast(origin, side, 0.2f))
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

        Vector3 origin = helper.position + Vector3.up;
        Vector3 direction = helper.forward;
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

        Vector3 side = (right) ? helper.right : -helper.right;
        side *= 0.25f + offset;
        Vector3 origin = transform.position + side;
        origin += Vector3.up / 2;
        Vector3 direction = helper.transform.forward;

        if (Physics.Raycast(origin, side, 0.2f))
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
                    float angle = Vector3.Angle(helper.forward, -towards.normal);

                    if (angle < 45)
                    {
                        retVal = true;
                        helper.position = PosWithOffset(origin, towards.point);
                        helper.rotation = Quaternion.LookRotation(-towards.normal);
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

    void End()
    {
        GetComponent<Collider>().isTrigger = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        inCover = false;
     //   anims.SetBool("Cover", inCover);
     //   anims.SetInteger("CoverDirection", coverDirection);
   //     anims.SetBool("CrouchToUpAim", crouchCover);
    }

    public bool GetUsingCover()
    {
        return inCover;
    }

    public bool GetCrouchingCover()
    {
        return crouchCover;
    }

    public bool GetAim()
    {
        return aiming;
    }

    public void SetMoveAxis(Vector3 moveAxis, bool aiming)
    {
        this.moveAxis = moveAxis;
        if(aiming != this.aimInput)
        {
            if (!aimInput) origin = transform.position;

            tempPos = transform.position + helper.right * coverDirection * 1;
            tempPos.y = transform.position.y;
        }
        this.aimInput = aiming;
    }
}

public class CoverPos
{
    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 initialHit;
}
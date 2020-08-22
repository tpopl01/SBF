using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour, IDisableIK //, ISetup
{
    Animator anim;
    //AISensors aI;

    [SerializeField] float offsetY = 0;
    [Space]
    [Header("Debug")]
    public Transform overrideLookTarget;
    [Space]
    [Header("Left")]
  //  [SerializeField] private Transform leftElbowTarget;
    [SerializeField] private Transform leftHandIkTarget = null;
    [SerializeField] private Transform secondaryWeaponHolder = null;

    [Space]
    [Header("Right")]
  //  [SerializeField] private Transform rightElbowTarget;
    [SerializeField] private Transform rightHandIkTarget = null;
    [SerializeField] private Transform weaponHolder = null;
    Vector3 desiredRHTarget;
    Vector3 desiredRHRot;
    Vector3 desiredLHTarget;
    Vector3 desiredLHRot;
    bool offHand;
    float lerpSpeed = 5f;
    //  public Transform rightHandIKRotation;


    private float lookWeight = 0.8f;
    [SerializeField] private float maxLookWeight = 0.8f;
    [SerializeField] private float bodyWeight = 0.8f;
    [SerializeField] private float headWeight = 1;
    [SerializeField] private float clampWeight = 1;

    private float targetWeight;


    private Transform rightShoulder;
    private Transform leftShoulder;

    private Vector3 secondHandLookPosition;

    //public Transform rightElbowTarget; //variable for IK hints
    private float rightHandIkWeight;
    private float targetRHweight;

    private float leftHandIkWeight;
    private float targetLHweight;

    private Transform aimHelperRS;
    private Transform aimHelperLS;

    private bool disableIK = false;
    private bool disableRHIK = false;
    private bool disableLHIK = false;
    private bool disableAimIK = false;
    private bool disableShoulders = false;
    public Vector3 LookPosition { get; set; }

    bool init;
    //private void Start()
    //{
    //    Init();
    //}
    bool globalPos;
    public void Init()
    {

        if (anim != null) return;

        if (overrideLookTarget == null)
        {
            GameObject testRH = new GameObject("Look");
            testRH.transform.parent = transform.parent;
            overrideLookTarget = testRH.transform;// transform.parent.Find("LookTarget");
            overrideLookTarget.position = transform.position + transform.forward * 2 + Vector3.up * 1.4f;
        }

        if (weaponHolder == null)
        {
            GameObject testRH = new GameObject("WH");
            testRH.transform.parent = transform.parent;
            testRH.transform.localPosition = Vector3.zero;
            weaponHolder = testRH.transform;
          //  weaponHolder = transform.parent.Find("Weapon RH holder (1)");
        }

        if (secondaryWeaponHolder == null)
        {
            GameObject testRH = new GameObject("SWH");
            testRH.transform.parent = transform.parent;
            testRH.transform.localPosition = Vector3.zero;
            secondaryWeaponHolder = testRH.transform;
          //  secondaryWeaponHolder = transform.parent.Find("Weapon LH holder (1)");
        }
        if (leftHandIkTarget == null)
        {
            GameObject rhIK = new GameObject("RHIK");
            leftHandIkTarget = rhIK.transform;
            //  leftHandIkTarget = transform.parent.Find("Weapon LH holder (1)").Find("LHIK");
            leftHandIkTarget.transform.parent = secondaryWeaponHolder.transform;
        }
        if (rightHandIkTarget == null)
        {
            GameObject rhIK = new GameObject("RHIK");
            rightHandIkTarget = rhIK.transform;
            rightHandIkTarget.transform.parent = weaponHolder.transform;
        }
        if (aimHelperRS == null)
        {
            aimHelperRS = new GameObject().transform;
            aimHelperRS.name = "Right Shoulder Aim Helper";
            aimHelperRS.parent = GameManagerModular.instance.aiFolder;
        }
        if (aimHelperLS == null)
        {
            aimHelperLS = new GameObject().transform;
            aimHelperLS.name = "Left Shoulder Aim Helper";
            aimHelperLS.parent =  GameManagerModular.instance.aiFolder;
        }

        anim = GetComponentInChildren<Animator>();
        rightShoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
        leftShoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
     //   this.aI = aI;

        if (weaponHolder == null)
        {
            weaponHolder = new GameObject("IK - WeaponHolder").transform;
            weaponHolder.SetParent(transform);
            weaponHolder = StaticMaths.ZeroOutLocal(weaponHolder);
            weaponHolder.parent = null;// GameManager.instance.aiFolder;
        }
        if (secondaryWeaponHolder == null)
        {
            secondaryWeaponHolder = new GameObject("IK - SecondaryWeaponHolder").transform;
            secondaryWeaponHolder.SetParent(transform);
            secondaryWeaponHolder = StaticMaths.ZeroOutLocal(secondaryWeaponHolder);
            secondaryWeaponHolder.parent = null; //GameManager.instance.aiFolder;
        }

     //   desiredLHRot = leftHandIkTarget.eulerAngles;
     //   desiredRHRot = rightHandIkTarget.eulerAngles;
      //  desiredLHTarget = leftHandIkTarget.position;
      //  desiredRHTarget = rightHandIkTarget.position;
        init = true;
    }

    private void LateUpdate()
    {
        if (!init) return;

        //if(disableRHIK)
        //{
        //    rightHandIkTarget.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position;
        //    rightHandIkTarget.rotation = anim.GetBoneTransform(HumanBodyBones.RightHand).rotation;
        //}


        rightHandIkTarget = LerpIKPos(rightHandIkTarget, desiredRHTarget, desiredRHRot, false);
        leftHandIkTarget = LerpIKPos(leftHandIkTarget, desiredLHTarget, desiredLHRot, offHand);

        HandleRightHandIKWeight();
        HandleShoulders();
        AimWeight();
        HandleLeftHandIKWeight();
        HandleShoulderRotation();
        if (disableIK)
        {
            rightHandIkWeight = 0;
            leftHandIkWeight = 0;
            targetLHweight = 0;
            targetRHweight = 0;
            rightHandIkTarget.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position;
            leftHandIkTarget.position = anim.GetBoneTransform(HumanBodyBones.LeftHand).position;
            rightHandIkTarget.rotation = anim.GetBoneTransform(HumanBodyBones.RightHand).rotation;
            leftHandIkTarget.rotation = anim.GetBoneTransform(HumanBodyBones.LeftHand).rotation;
        }
        DisableIKs();
        LookPosition = Vector3.zero;
        overrideLookTarget.position = transform.forward * 5 + transform.position + transform.up * 1.4f;
    }

    private Transform LerpIKPos(Transform target, Vector3 desiredPos, Vector3 desiredRot, bool offHand)
    {
        if (offHand || globalPos)
        {
            target.position = Vector3.Lerp(target.position, desiredPos, Time.deltaTime * lerpSpeed * 5f);
            target.rotation = Quaternion.Slerp(target.rotation, Quaternion.Euler(desiredRot), Time.deltaTime * lerpSpeed * 5f);
        }
        else
        {
            target.localPosition = Vector3.Lerp(target.localPosition, desiredPos, Time.deltaTime * lerpSpeed * 1f);
            target.localRotation = Quaternion.Slerp(target.localRotation, Quaternion.Euler(desiredRot), Time.deltaTime * lerpSpeed * 1f);
        }
        return target;
    }

    private void DisableIKs()
    {
        disableAimIK = true;
        disableLHIK = true;
        disableRHIK = true;
        disableShoulders = true;
    }

    public void SetAiming(bool aiming)
    {
        if(!aiming)
        {
            bodyWeight = 0.1f;
            headWeight = 0f;
            disableShoulders = true;
            disableAimIK = true;
            
        }
        else
        {
            bodyWeight = 0.8f;
            headWeight = 1;
        }
    }

    public void OffHandLHWeap(Vector3 target, Vector3 targetRot)
    {
        leftHandIkTarget.parent = rightHandIkTarget;
        desiredLHTarget = target;// + rightHandIkTarget.position;// + rightHandIkTarget.forward * Vector3.Distance(target, rightHandIkTarget.position);
        desiredLHRot = targetRot;
        offHand = false;
    }

    public void OffhandLeftHandTarget(Vector3 target, Vector3 targetRot)
    {
        leftHandIkTarget.parent = secondaryWeaponHolder;
        desiredLHTarget = target;
        desiredLHRot = targetRot;
       // leftHandIkTarget.position = target;
       // leftHandIkTarget.rotation = Quaternion.Euler(targetRot);
        offHand = true;
    }

   // bool disableLHRot;
    public void LeftHandTarget(Vector3 target, Vector3 targetRot, bool globalPos = false)
    {
        leftHandIkTarget.parent = secondaryWeaponHolder;
        desiredLHTarget = target;
        desiredLHRot = targetRot;
       // disableLHRot = targetRot == Vector3.zero;
        //leftHandIkTarget.localPosition = target;
        // leftHandIkTarget.localRotation = Quaternion.Euler(targetRot);
        offHand = false;
    }

    public void RightHandTarget(Vector3 target, Vector3 targetRot, bool globalPos = false)
    {
        this.globalPos = globalPos;
        desiredRHTarget = target;
        desiredRHRot = targetRot;
        // rightHandIkTarget.localPosition = target;
        // rightHandIkTarget.localRotation = Quaternion.Euler(targetRot);
    }

    public void EnableAimIK()
    {
        disableAimIK = false;
    }

    public void EnableRHIK()
    {
        disableRHIK = false;
    }

    public void EnableLHIK()
    {
        disableLHIK = false;
    }
    public void EnableShoulderIK()
    {
        disableShoulders = false;
    }

    void HandleShoulders()
    {
        weaponHolder.position = rightShoulder.position;

      //  if (enableTwoHandWield)
    //    {
            if (secondaryWeaponHolder != null)
            {
                secondaryWeaponHolder.position = leftShoulder.position;
            }
      //  }

    }

    void AimWeight()
    {
        if (disableIK)
        {
            targetWeight = 0;
            lookWeight = 0;
        }
        else
        {
            if (!disableAimIK)
            {
                Vector3 directionTowardsTarget = aimHelperRS.position - transform.position;
                float angle = Vector3.Angle(transform.forward, directionTowardsTarget);

                if (angle < 50)
                {
                    targetWeight = maxLookWeight;
                }
                else
                {
                    targetWeight = 0;
                }
            }
            else
            {
                targetWeight =0;
            }
        }
        lookWeight = Mathf.Lerp(lookWeight, targetWeight, Time.deltaTime * 5);
    }

    void HandleRightHandIKWeight()
    {
        float multiplier = 15;
       // rightHandIkWeight = lookWeight;

        if (disableRHIK)
        {
            //we dont want IK
            targetRHweight = 0;
            multiplier = 5;
        }
        else
        {
            targetRHweight = 1;
        }
        if (disableIK)
        {
            rightHandIkWeight = 0;
            targetRHweight = 0;
            targetWeight = 0;
            lookWeight = 0;
        }

        //lerp to the desired values
        rightHandIkWeight = Mathf.Lerp(rightHandIkWeight, targetRHweight, Time.deltaTime * multiplier);
    }

    void HandleLeftHandIKWeight()
    {
        float multiplier = 15;

        if (!disableLHIK)
        {
            targetLHweight = 1;
        }
        else targetLHweight = 0;

        if (disableIK)//if we are reloading
        {
            //we dont want IK
            targetLHweight = 0;
            leftHandIkWeight = 0;
        }

        //lerp to the desired values
        leftHandIkWeight = Mathf.Lerp(leftHandIkWeight, targetLHweight, Time.deltaTime * multiplier);
    }

    void HandleShoulderRotation()
    {
        if (LookPosition == Vector3.zero)
            LookPosition = overrideLookTarget.position;

        if (LookPosition != Vector3.zero)
        {
            aimHelperRS.position = Vector3.Lerp(aimHelperRS.position, LookPosition, Time.deltaTime * 10);
          //  weaponHolder.LookAt(aimHelperRS.position);
         //   if (!disableShoulders && rightHandIkTarget)
                weaponHolder.transform.rotation = Quaternion.Slerp(weaponHolder.transform.rotation, Quaternion.LookRotation(aimHelperRS.position - weaponHolder.transform.position), Time.deltaTime * 5);
                //rightHandIkTarget.parent.transform.LookAt(aimHelperRS.position);

          //  if (enableTwoHandWield)
         //   {
                secondHandLookPosition = LookPosition;

                aimHelperLS.position = Vector3.Lerp(aimHelperLS.position, secondHandLookPosition, Time.deltaTime * 10);
                if (!disableShoulders)
                    secondaryWeaponHolder.LookAt(aimHelperLS.position);
                //leftHandIkTarget.parent.transform.LookAt (aimHelperLS.position);
           // }
        }
    }

    void OnAnimatorIK()
    {
        if (!init) return;
        anim.SetLookAtWeight(lookWeight, bodyWeight, headWeight, headWeight, clampWeight);

        Vector3 filterDirection = LookPosition;
        filterDirection.y = offsetY;// if needed
        if (filterDirection == Vector3.zero) filterDirection = overrideLookTarget.position;
        anim.SetLookAtPosition(filterDirection);// (overrideLookTarget != null) ? overrideLookTarget.position : filterDirection);

        if (leftHandIkTarget)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIkWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIkTarget.position);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIkWeight);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIkTarget.rotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

        if (rightHandIkTarget)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIkWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIkTarget.position);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIkWeight);
            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandIkTarget.rotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }

      //  if (rightElbowTarget)
       // {
      //      anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandIkWeight);
       //     anim.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTarget.position);
       // }
      //  else
      //  {
      //      anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
      //  }

        //if (leftElbowTarget)
        //{
        //    anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandIkWeight);
        //    anim.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTarget.position);
        //}
        //else
        //{
        //    anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
        //}
    }

    public void DisableIK(bool disable)
    {
        disableIK = disable;
    }

    public bool GetAiming()
    {
        return (targetRHweight - rightHandIkWeight) < 0.1f;
    }

    //public void SetUp(Transform root)
    //{
    //    Init();
    //}
}

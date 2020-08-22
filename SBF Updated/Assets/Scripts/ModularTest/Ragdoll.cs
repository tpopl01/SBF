using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour, ISetup
{
    List<RagdollBones> ragdollBones = new List<RagdollBones>();
    Collider controllerCollider;
    public bool Ragdolled { get; private set; } = false;
    Animator anim;
    Rigidbody rb;

    [SerializeField]bool toggleRagdoll = false;

    Vector3 startPos;
    // public bool stick = true;

    private void Awake()
    {
        SetUp(transform);
    }

    public void SetUp(Transform root)
    {
        Debug.Log("Setup Ragdoll");
        startPos = transform.position;
        controllerCollider = root.GetComponent<Collider>();
        anim = root.GetComponentInChildren<Animator>();
        rb = root.GetComponent<Rigidbody>();
       // rb.isKinematic = true;
        InitRagdoll();
        rb.velocity = Vector3.zero;
    }

    private void Start()
    {
        //   rb.velocity = Vector3.zero;
       // if(stick)
      //  transform.position = startPos;
   //     InitRagdoll();
    }

    bool rD;
    private void Update()
    {

        if(toggleRagdoll != rD)
        {
            if (!Ragdolled)
            {
                RagdollCharacter();
            }
            else
                GetUp();
        }
        if (Ragdolled) transform.position = startPos;
        rD = toggleRagdoll;
    }

    //private void Update()
    //{
    //    // rb.velocity = Vector3.zero;
    //    if (stick)
    //        transform.position = startPos;
    //}

    //private void FixedUpdate()
    //{
    //    //  rb.velocity = Vector3.zero;
    //    if (stick)
    //        transform.position = startPos;
    //}

    //private void LateUpdate()
    //{
    //    //  rb.velocity = Vector3.zero;
    //    if (stick)
    //        transform.position = startPos;
    //}

    void InitRagdoll()
    {
        ragdollBones.Clear();
        Rigidbody[] rigB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rigB.Length; i++)
        {
            if (rb != rigB[i])
            {
                rigB[i].isKinematic = true;
                Collider col = rigB[i].GetComponent<Collider>();
                col.isTrigger = true;
                col.enabled = false;
                ragdollBones.Add(new RagdollBones(rigB[i], col));
            }
        }
        Ragdolled = false;
       // rb.isKinematic = false;
    }

    public void RagdollCharacter()
    {
     //   return;
        if (Ragdolled)
            return;
      //  Debug.Log("Ragdoll");
        startPos = transform.position;
        rb.isKinematic = true;
        for (int i = 0; i < ragdollBones.Count; i++)
        {
            ragdollBones[i].rB.isKinematic = false;
            ragdollBones[i].col.isTrigger = false;
            ragdollBones[i].col.enabled = true;
        }
        controllerCollider.isTrigger = true;

        //disables animator after frame ends so the animations dont lose the connection to early and cause problems
     //   if (gameObject.activeInHierarchy)
            StartCoroutine(DisableAnimator());
        Ragdolled = true;
    }

    public void AddExplosiveForce(Vector3 origin, float radius, float force = 5)
    {
        if (Ragdolled == false) RagdollCharacter();
        for (int i = 0; i < ragdollBones.Count; i++)
        {
            float dist = Vector3.Distance(origin, ragdollBones[i].rB.position);
            if(dist < radius)
                ragdollBones[i].rB.AddExplosionForce(force, origin, radius, 1.5f);
        }
    }

    bool playAnim;
    public void GetUp()
    {
        DeRagdoll();
        playAnim = true;
    }

    public void DeRagdoll()
    {
        if (!Ragdolled)
            return;
        playAnim = false;

        for (int i = 0; i < ragdollBones.Count; i++)
        {
            ragdollBones[i].rB.isKinematic = true;
            //  Collider col = ragdollBones[i].GetComponent<Collider>();
            ragdollBones[i].col.isTrigger = true;
            ragdollBones[i].col.enabled = false;
        }
        controllerCollider.isTrigger = false;
        rb.isKinematic = false;
        //disables animator after frame ends so the animations dont lose the connection to early and cause problems
       // if (gameObject.activeInHierarchy)
            StartCoroutine(EnableAnimator());
        //     anim.enabled = true;
        Ragdolled = false;
    }
    
    IEnumerator DisableAnimator()
    {
        yield return new WaitForEndOfFrame();
        anim.enabled = false;
       // Debug.Log("Disable Animator");
    }
    IEnumerator EnableAnimator()
    {
        yield return new WaitForEndOfFrame();
        anim.enabled = true;
        if (playAnim)
            anim.Play("stand_up");
    }
}

public class RagdollBones
{
    public Rigidbody rB;
    public Collider col;

    public RagdollBones(Rigidbody rB, Collider col)
    {
        this.col = col;
        this.rB = rB;
    }
}
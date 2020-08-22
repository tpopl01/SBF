using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class Jetpack : MonoBehaviour, IJump, ISetup, IFixedTick, ITick
{
    Rigidbody rb;
    bool jumping;
    Timer jumpTimer = new Timer(20);
    Timer cooldown = new Timer(5);
    InputBase input;
    Transform cam;
    // Transform camBase;
    [SerializeField] ParticleSystem[] particles;
    AudioProfileGeneral jetPackEngine;
    AudioSource aS;

    public void SetUp(Transform root)
    {
        if(particles == null)
            particles = GetComponentsInChildren<ParticleSystem>();
        rb = root.GetComponent<Rigidbody>();
        input = root.GetComponent<InputBase>();
       // camBase = CameraManager.instance.transform;
        cam = CameraManager.instance.GetComponentInChildren<Camera>().transform;
        aS = GetComponent<AudioSource>();
        if (aS == null)
        {
            aS = gameObject.AddComponent<AudioSource>();
            aS.spatialBlend = 1.0f;
            aS.playOnAwake = false;
            aS.loop = true;
        }
        jetPackEngine = Resources.Load<AudioProfileGeneral>("Audio/Audio_General_JetPack");
        EndJump();
    }


    public void BeginJump(Transform root, bool onGround)
    {
        if (!jumping)
        {
            if (onGround == false && cooldown.GetComplete())
            {
                StartJump();
            }
        }
        else
        {
            EndJump();
        }
    }

    void StartJump()
    {
        Vector3 dir = rb.velocity;
        dir.y = 0;
        rb.velocity = Vector3.zero;// dir / 10;
        jumping = true;
        jumpTimer.StartTimer();
        rb.useGravity = false;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    void EndJump()
    {
        cooldown.StartTimer();
        jumping = false;
        rb.useGravity = true;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
        jetPackEngine.Stop(aS);
    }

    public bool GetJumping()
    {
        return jumping;
    }

    public void FixedTick()
    {
        if (!jumping) return;
        jetPackEngine.PlaySound(aS);
        Vector3 inp = input.MoveAxis;

        Vector3 v = cam.forward * inp.z;
        Vector3 h = cam.right * inp.x;
        rb.velocity = (h  + v).normalized * 5;
      //  rb.AddForce((h + v).normalized * 2, ForceMode.Acceleration);
    }

    public void Tick()
    {
        if (!jumping) return;
        if (jumpTimer.GetComplete())
        {
            EndJump();
        }
        Vector3 dir = -StaticMaths.GetDirection(cam.transform.position, rb.transform.position);
        dir.y = 0;
        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
    }
}

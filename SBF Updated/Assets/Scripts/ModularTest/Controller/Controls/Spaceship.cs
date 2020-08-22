using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;
using UnityEngine.AI;

public class Spaceship : MonoBehaviour, ITick, ISetup, IMove, IDeath
{
    ShipState state;
    [Space]
    [Header("Boost Shake Settings")]
    private Transform kickGO = null;
    [SerializeField] float kickUpside = 0.5f;
    [SerializeField] float kickSideways = 0.5f;
    [SerializeField] LayerMask mask= ~(1<<10);

    Timer boostTimer = new Timer(5);
    Timer takeOffTimer = new Timer(2);
    private float boostSpeed = 50;
    private bool boosting;
    AIStats stats;
    float curSpeed = 10;
    Grounded grounded;
    Transform shipModel;

    [SerializeField] AudioProfileSpaceship audioSpaceship;
    AudioSource aS;
    ParticleSystem[] particleSystems;
    public int spaceshipTeam;


   // EnterExit enterExit;

    public void SetUp(Transform root)
    {
    //    enterExit = root.gameObject.GetComponent<EnterExit>();
        grounded = root.GetComponentInChildren<Grounded>();
        shipModel = root.GetChild(0);
        kickGO = shipModel;
        stats = root.GetComponent<ModularController>().AIStats();
        aS = gameObject.GetComponent<AudioSource>();
        if(aS == null)
        aS = gameObject.AddComponent<AudioSource>();
        aS.loop = true;
        aS.spatialBlend = 1;
        aS.playOnAwake = false;
        audioSpaceship = Resources.Load<AudioProfileSpaceship>("Audio/Audio_Spaceship");
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        EnableParticles(false);
    }

    #region Movement
    public void Move(Vector3 input, float speed)
    {
        if (state == ShipState.Air)
        {
            if (boosting) speed = boostSpeed + stats.GetSprintSpeed();
            curSpeed = Mathf.Lerp(curSpeed, speed, Time.deltaTime);
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
            LookAtTarget(input);
        }
    }

    void Roll(float angle)
    {
        shipModel.localRotation = Quaternion.Slerp(shipModel.localRotation, Quaternion.Euler(0, 0, -angle), Time.deltaTime * Mathf.Clamp01(stats.GetTurnSpeed()));
    }

    private void LookAtTarget(Vector3 positionToLook)
    {
        Vector3 directionToLookTo = positionToLook - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToLookTo);
        if (angle > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(directionToLookTo);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, ((stats.GetTurnSpeed() * Time.deltaTime)/curSpeed) * 10f);
        }

        Roll(StaticMaths.AngleSigned(transform.forward, directionToLookTo, transform.up));
    }
    #endregion

    public float GetCurSpeed()
    {
        return curSpeed;
    }


    public void Tick()
    {
        switch (state)
        {
            case ShipState.Idle:
                break;
            case ShipState.TakeOff:
                TakeOff();
                break;
            case ShipState.Land:
                Land();
                break;
            case ShipState.Air:
                audioSpaceship.Engine(aS);
                if (curSpeed <5f) curSpeed = 5f;
                HandleBoost();
                break;
            default:
                break;
        }
    }

    void AccendDecend(float accendDecend)
    {
        if (accendDecend > 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5);
        }
        else if (accendDecend < 0)
        {
            transform.Translate(-Vector3.up * Time.deltaTime * 5);
        }
    }


    public void TryTakeOff()
    {
        if (state == ShipState.Idle)
        {
            takeOffTimer.StartTimer();
            audioSpaceship.Ascend(aS);
            state = ShipState.TakeOff;
            curSpeed = 10;
        }
    }

    public void TryLand()
    {
        if (state == ShipState.Air)
        {
            if (RaycastForLand(8))
            {
                EnableParticles(false);
                audioSpaceship.Descend(aS);
                state = ShipState.Land;
            }
        }
    }

    public ShipState GetState()
    {
        return state;
    }

    #region Land Take Off
    bool RaycastForLand(float dist)
    {
        return Physics.Raycast(transform.position, Vector3.down, dist, mask);
    }

    void EnableParticles(bool e)
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if(e)
                particleSystems[i].Play();
            else
                particleSystems[i].Stop();
        }
    }

    bool Land()
    {
        grounded.AlignToGround(10, 0, true);
        transform.position -= Vector3.up * Time.deltaTime * 3;
        shipModel.localRotation = Quaternion.Slerp(shipModel.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime);
        if (grounded.IsGrounded(2,2))
        {
            audioSpaceship.Stop(aS);
            state = ShipState.Idle;
            return true;
        }
        return false;
    }
    void TakeOff()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 5);
        if (takeOffTimer.GetComplete())
        {
            EnableParticles(true);
            audioSpaceship.Engine(aS);
            state = ShipState.Air;
        }
    }
    #endregion

    #region Boost
    public bool GetBoosting()
    {
        return boosting;
    }
    public void BeginBoost()
    {
        if(boosting == false)
        {
            curSpeed = boostSpeed + stats.GetSprintSpeed();
            boosting = true;
            boostTimer.StartTimer();
        }
    }

    void HandleBoost()
    {
        if(boosting)
        {
            if(boostTimer.GetComplete())
            {
                boosting = false;
            }
            else
            {
                KickBack();
            }
        }
        else
        {
            kickGO.transform.localRotation = Quaternion.Slerp(kickGO.transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime);
        }
    }

    void KickBack()
    {
        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(Random.Range(-kickUpside, kickUpside), Random.Range(-kickSideways, kickSideways), 0));
    }

    public void Death()
    {
        state = ShipState.Idle;
        shipModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        EnableParticles(false);
    }
    #endregion
}
public enum ShipState
{
    Idle,
    TakeOff,
    Land,
    Air
}
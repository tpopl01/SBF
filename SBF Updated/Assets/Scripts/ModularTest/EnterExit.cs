using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnterExit : MonoBehaviour, IEnter, ISetup, ITick, IDeath
{
    protected ModularControllerUnit unit;
    ModularController self;
    [SerializeField] float range = 5;
    public bool Player { get; private set; } = false;
    [SerializeField] string playerBrain = "";
    [SerializeField] string aiBrainPath = "";
    InputBrainBase brainBase;
    [SerializeField] float camOffset = -10;
    [SerializeField] CameraState cameraState = CameraState.spaceShip;
    [SerializeField] Vector3 pivotOffset;

    public bool CanEnter()
    {
        return unit == null && gameObject.activeInHierarchy && self.Health.IsDead()==false;
    }

    //public void RemoveUnit()
    //{
    //    unit.Health.ForceKill();
    //    Exit();
    //}

    public bool GetUnit()
    {
        return unit != null;
    }

    public virtual bool Enter(ModularControllerUnit character, bool player)
    {
        if(Vector3.Distance (character.Position, transform.position) < range)
        {
            if(player)
            {
                CameraManager.instance.SetState(transform, cameraState);
                CameraManager.instance.SetCameraOffset(pivotOffset, camOffset);
                brainBase.brain = Resources.Load<BrainBase>(playerBrain);
            }
            else
            {
                brainBase.brain = Resources.Load<BrainBase>(aiBrainPath);
            }
            this.Player = player;

            unit = character;
            unit.gameObject.SetActive(false);
            self.SetTeam(unit.Team);
            GameManagerModular.instance.AddUnitToTeam(self);
            return true;
        }
        return false;
    }

    public virtual void Exit()
    {
        Vector3 p = transform.right*2 + transform.up + transform.position;
        if (NavMesh.SamplePosition(p, out NavMeshHit hit, 20, NavMesh.AllAreas))
        {
            p = hit.position;
        }
        if(Player)
        {
            CameraManager.instance.SetCameraOffset(Vector3.zero);
            CameraManager.instance.SetState(unit.transform, CameraState.ThirdPerson);
        }
        brainBase.brain = null;
        GameManagerModular.instance.RemoveUnitFromTeam(self);
        self.SetTeam(-1);
        unit.transform.position = p;
        unit.gameObject.SetActive(true);
        unit = null;
        Player = false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetUp(Transform root)
    {
        brainBase = root.GetComponent<InputBrainBase>();
        self = GetComponent<ModularController>();
    }

    public virtual void Tick()
    {
       // if (unit)
         //   unit.transform.position = transform.position;
    }

    public void Death()
    {
        if (unit)
        {
            ModularControllerUnit character = unit;
            Exit();
            character.Health.ForceKill();
        }
    }
}

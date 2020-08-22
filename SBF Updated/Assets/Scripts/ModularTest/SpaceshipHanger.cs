using System.Collections;
using System.Collections.Generic;
using tpopl001.Utils;
using UnityEngine;

public class SpaceshipHanger : MonoBehaviour
{
    [SerializeField] HangerPosition[] hangerPositions;
    List<SpaceshipPos> spaceShipTakeOff = new List<SpaceshipPos>();
    List<SpaceshipPos> spaceShipLand = new List<SpaceshipPos>();
    public int team;

    //public static SpaceshipHanger instance;
    //private void Awake()
    //{
    //    instance = this;
    //}

    bool TakeOffContainsShip(Spaceship s)
    {
        for (int i = 0; i < spaceShipTakeOff.Count; i++)
        {
            if(spaceShipTakeOff[i].spaceship == s)
            {
                return true;
            }
        }
        return false;
    }

    bool LandContainsShip(Spaceship s)
    {
        for (int i = 0; i < spaceShipLand.Count; i++)
        {
            if (spaceShipLand[i].spaceship == s)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        for (int i = 0; i < hangerPositions.Length; i++)
        {
            if(hangerPositions[i].containedShip)
            {
                if(hangerPositions[i].containedShip.GetState() != ShipState.Idle)
                {
                    if (TakeOffContainsShip(hangerPositions[i].containedShip) == false)
                    {
                        TakeOff(hangerPositions[i].containedShip);
                    }
                }
            }
        }

        for (int i = 0; i < spaceShipTakeOff.Count; i++)
        {
            Transform ship = spaceShipTakeOff[i].spaceship.transform;
            ship.LookAt(hangerPositions[spaceShipTakeOff[i].hangerIndex].hangerExitPoint);
            ship.position += ship.forward * Time.deltaTime*10;
            Vector3 pos = ship.position;
            pos.y = hangerPositions[spaceShipTakeOff[i].hangerIndex].hangerExitPoint.position.y;
            ship.position = pos;

            if (Vector3.Distance(ship.position, hangerPositions[spaceShipTakeOff[i].hangerIndex].hangerExitPoint.position) < 3)
            {
                spaceShipTakeOff[i].spaceship.GetComponent<InputBrainBase>().enabled = true;
                hangerPositions[spaceShipTakeOff[i].hangerIndex].RemoveShip();
                spaceShipTakeOff.RemoveAt(i);
            }
        }
        for (int i = 0; i < spaceShipLand.Count; i++)
        {
            Transform ship = spaceShipLand[i].spaceship.transform;
            ship.LookAt(hangerPositions[spaceShipLand[i].hangerIndex].hangerSpawnPoint);
            ship.position += ship.forward * Time.deltaTime * 10;
            Vector3 pos = ship.position;
            pos.y = hangerPositions[spaceShipLand[i].hangerIndex].hangerExitPoint.position.y;
            ship.position = pos;

            if (Vector3.Distance(ship.position, hangerPositions[spaceShipLand[i].hangerIndex].hangerSpawnPoint.position) < 1)
            {
                spaceShipLand[i].spaceship.GetComponent<InputBrainBase>().enabled = true;
                spaceShipLand[i].spaceship.TryLand();
                hangerPositions[spaceShipLand[i].hangerIndex].containedShip = spaceShipLand[i].spaceship;
                spaceShipLand.RemoveAt(i);
            }
        }
    }

    public HangerPosition GetSpawnPoint()
    {
        for (int i = 0; i < hangerPositions.Length; i++)
        {
            if (hangerPositions[i].CanSpawn())
                return hangerPositions[i];
        }
        return null;
    }

    public void Land(Spaceship s)
    {
        if (!LandContainsShip(s) && !TakeOffContainsShip(s))
        {
            for (int i = 0; i < hangerPositions.Length; i++)
            {
                if (hangerPositions[i].containedShip == null)
                {
                    s.GetComponent<InputBrainBase>().enabled = false;
                    spaceShipLand.Add(new SpaceshipPos(s, i));
                    break;
                }
            }
        }
    }

    void TakeOff(Spaceship s)
    {
        for (int i = 0; i < hangerPositions.Length; i++)
        {
            s.GetComponent<InputBrainBase>().enabled = false;
            if (hangerPositions[i].containedShip == s)
            {
                spaceShipTakeOff.Add(new SpaceshipPos(s, i));
                hangerPositions[i].containedShip.TryTakeOff();
            }
        }
    }

}
[System.Serializable]
public class HangerPosition
{
    public Transform hangerSpawnPoint;
    public Transform hangerExitPoint;
    public Spaceship containedShip;
    public Timer newSpaceshipTimer = new Timer(5);

    public void SpawnShip(Spaceship s)
    {
        containedShip = s;
        s.transform.position = hangerSpawnPoint.position - Vector3.up * 0.8f;
        s.transform.rotation = hangerSpawnPoint.rotation;
        s.gameObject.SetActive(true);
    }

    public void RemoveShip()
    {
        newSpaceshipTimer.StartTimer();
        containedShip = null;
    }

    public bool CanSpawn()
    {
        return newSpaceshipTimer.GetComplete() && containedShip == null;
    }
}
public class SpaceshipPos
{
    public Spaceship spaceship;
    public int hangerIndex;

    public SpaceshipPos(Spaceship s, int index)
    {
        spaceship = s;
        hangerIndex = index;
    }
}
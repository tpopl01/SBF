using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHangerManager : MonoBehaviour
{
    SpaceshipHanger[] hangers;
    [SerializeField]int shipsPerTeam = 21;
    List<Spaceship> spaceShipsToSpawn = new List<Spaceship>();
    List<Spaceship> spaceShipsToSpawn1 = new List<Spaceship>();

    public static SpaceshipHangerManager instance;
    private void Awake()
    {
        instance = this;
        Spaceship[] shipsREP = Resources.LoadAll<Spaceship>("Modular/Vehicles/REP/");
        Spaceship[] shipsCIS = Resources.LoadAll<Spaceship>("Modular/Vehicles/CIS/");
        hangers = GameObject.FindObjectsOfType<SpaceshipHanger>();
        for (int i = 0; i < shipsPerTeam; i++)
        {
            Spaceship s = Instantiate(shipsREP[Random.Range(0, shipsREP.Length)]);
            AddShipToSpawnList(s, 0);
            s.gameObject.SetActive(true);
            s = Instantiate(shipsCIS[Random.Range(0, shipsCIS.Length)]);
            AddShipToSpawnList(s, 1);
            s.gameObject.SetActive(true);
        }
        StartCoroutine(LateDeactivate());
    }

    IEnumerator LateDeactivate()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < spaceShipsToSpawn.Count; i++)
        {
            spaceShipsToSpawn[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < spaceShipsToSpawn1.Count; i++)
        {
            spaceShipsToSpawn1[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < hangers.Length; i++)
        {
            if(spaceShipsToSpawn.Count > 0 && hangers[i].team == spaceShipsToSpawn[0].spaceshipTeam)
            {
                HangerPosition h = hangers[i].GetSpawnPoint();
                if (h != null)
                {
                    h.SpawnShip(spaceShipsToSpawn[0]);
                    spaceShipsToSpawn.RemoveAt(0);
                }
            }
            else
            {
                if (spaceShipsToSpawn1.Count > 0)
                {
                    HangerPosition h = hangers[i].GetSpawnPoint();
                    if (h != null)
                    {
                        h.SpawnShip(spaceShipsToSpawn1[0]);
                        spaceShipsToSpawn1.RemoveAt(0);
                    }
                }
            }
        }
    }

    public void AddShipToSpawnList(Spaceship s, int team)
    {
        if (team == 0)
        {
            if (spaceShipsToSpawn.Contains(s) == false)
            {
                s.gameObject.SetActive(false);
                spaceShipsToSpawn.Add(s);
            }
        }
        else
        {
            if (spaceShipsToSpawn1.Contains(s) == false)
            {
                s.gameObject.SetActive(false);
                spaceShipsToSpawn1.Add(s);
            }
        }
    }

}

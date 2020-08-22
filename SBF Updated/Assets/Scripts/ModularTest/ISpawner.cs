using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner 
{
    bool CanSpawn(int team);
    Vector3 GetSpawnPosition();
}

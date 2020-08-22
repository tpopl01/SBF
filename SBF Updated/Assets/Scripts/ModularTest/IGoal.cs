using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoal 
{
    Vector3 Position();
    bool GetNearestGoal(int team);
    bool InRange(Vector3 pos);
    void Capture(int team, Vector3 pos);
}

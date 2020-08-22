using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepair
{
    bool NeedsRepair(int team);
    bool Repair();
    bool InRange(Vector3 pos);
    Vector3 Position();
}

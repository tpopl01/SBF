using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnterable
{
    bool CanEnter();
    Vector3 Position();
    bool Enter(GameObject root);
}

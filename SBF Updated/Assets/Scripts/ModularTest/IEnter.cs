using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnter
{
    Vector3 GetPosition();
    bool CanEnter();
    bool Enter(ModularControllerUnit character, bool player);
    void Exit();
}

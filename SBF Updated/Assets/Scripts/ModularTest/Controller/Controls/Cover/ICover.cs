using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICover
{
    void SetMoveAxis(Vector3 moveAxis, bool aiming);
    void Begin();
    bool GetUsingCover();
    bool GetCrouchingCover();
    bool GetAim();

}

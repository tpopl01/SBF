using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJump
{
    bool GetJumping();
    void BeginJump(Transform root, bool onGround);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeaderCommands : MonoBehaviour
{
    public Vector3 targetPos;

    public abstract bool Follow();
    public abstract void Wait();
    public abstract void Dismiss();
    public abstract void MoveToPos();
}

public enum LeaderState
{
    None,
    Follow,
    Wait,
    MoveToPos
}

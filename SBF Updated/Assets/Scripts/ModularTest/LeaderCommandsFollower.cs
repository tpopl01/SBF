using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCommandsFollower : LeaderCommands
{
    public LeaderState LeaderState { get; private set; } = LeaderState.None;

    private void Update()
    {
        if (LeaderState == LeaderState.None)
            targetPos = Vector3.zero;
    }

    public override void Dismiss()
    {
        targetPos = Vector3.zero;
        LeaderState = LeaderState.None;
    }

    public override bool Follow()
    {
        LeaderState = LeaderState.Follow;
        return true;
    }

    public override void MoveToPos()
    {
        LeaderState = LeaderState.MoveToPos;
    }

    public override void Wait()
    {
        LeaderState = LeaderState.Wait;
        targetPos = transform.position;
    }
}

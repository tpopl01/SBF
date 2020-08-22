using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCommandsLeader : LeaderCommands
{
    [SerializeField] int maxFollowers = 2;
    List<LeaderCommandsFollower> followers;
    Senses s;

    private void Start()
    {
        followers = new List<LeaderCommandsFollower>(maxFollowers);
        s = GetComponentInChildren<Senses>();
    }

    private void Update()
    {
        if(followers.Count > 0)
        {
            for (int i = 0; i < followers.Count; i++)
            {
                if (followers[i].LeaderState == LeaderState.Follow)
                    followers[i].targetPos = transform.position;
            }
        }
    }

    public override void Dismiss()
    {
        for (int i = 0; i < followers.Count; i++)
        {
                followers[i].Dismiss();
                followers.RemoveAt(i);
        }
    }

    public override bool Follow()
    {
        if(followers.Count < maxFollowers)
        {
            if (s.ClosestAlly)
            {
                if (s.ClosestAlly.GetType() == typeof(ModularControllerAI))
                {
                    if (((ModularControllerAI)s.ClosestAlly).Commands.Follow())
                    {
                        followers.Add((LeaderCommandsFollower)(((ModularControllerAI)s.ClosestAlly).Commands));
                        Debug.Log("Follow");
                    }
                }
            }
        }

        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].targetPos = transform.position;
            followers[i].Follow();
        }
        return false;
    }

    public override void MoveToPos()
    {
        targetPos = CameraManager.instance.cameraMain().position + CameraManager.instance.cameraMain().forward * 20;
        for (int i = 0; i < followers.Count; i++)
        {
                followers[i].targetPos = targetPos;
                followers[i].MoveToPos();
        }
    }

    public override void Wait()
    {
        for (int i = 0; i < followers.Count; i++)
        {
                followers[i].Wait();
        }
    }
}

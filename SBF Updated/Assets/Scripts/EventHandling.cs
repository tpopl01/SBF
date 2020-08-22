using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Events
{
    public static class EventHandling
    {
        public delegate void DeathAction(int team);
        public static event DeathAction OnDeath;
        //    public static event DeathAction OnRespawn;

        public delegate void CaptureAction(int team);
        public static event CaptureAction OnCapture;

        public delegate void CompleteAction();
        public static event CompleteAction OnComplete;

        public delegate void PlayerCaptureAction(bool capturing);
        public static event PlayerCaptureAction OnPlayerCapture;

        public static void Death(int team)
        {
            OnDeath?.Invoke(team);
        }

        public static void Capture(int team)
        {
            OnCapture?.Invoke(team);
        }

        public static void Complete()
        {
            OnComplete?.Invoke();
        }

        public static void PlayerCapturing(bool capturing)
        {
            OnPlayerCapture?.Invoke(capturing);
        }

        //  public static void Respawn(AISensors states)
        //  {
        ////      OnRespawn?.Invoke(states);
        //  }

    }
}
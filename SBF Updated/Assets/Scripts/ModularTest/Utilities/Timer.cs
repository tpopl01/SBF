using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Utils
{
    [System.Serializable]
    public class Timer
    {
        private float startTime;
        public float Duration { get; set; }

        public Timer(float duration)
        {
            this.Duration = duration;
        }

        /// <summary>
        /// Begin the timer
        /// </summary>
        public void StartTimer()
        {
            startTime = GetTime();
        }

        /// <summary>
        /// Is the timer finished
        /// </summary>
        public bool GetComplete()
        {
            return GetTime() >= startTime + Duration;
        }

        /// <summary>
        /// The current game time
        /// </summary>
        private float GetTime()
        {
            return Time.time;
        }
    }
}
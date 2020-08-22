using System;
using System.Collections;
using System.Collections.Generic;

namespace tpopl001.Utils
{
    public class Tick
    {
        private int tick = 0;
        private int maxTick = 0;

        private int maxTickRange = 0;
        private int minTickRange = 0;

        public Tick(int minTickRange, int maxTickRange)
        {
            this.minTickRange = minTickRange;
            this.maxTickRange = maxTickRange;
            Reset();
        }

        public bool IsDone()
        {
            tick++;
            if (tick >= maxTick)
            {
                Reset();
                return true;
            }
            return false;
        }

        private void Reset()
        {
            Random random = new Random();
            tick = 0;
            maxTick = random.Next(minTickRange, maxTickRange);
        }

    }
}
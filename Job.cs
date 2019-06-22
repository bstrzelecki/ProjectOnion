using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    class Job
    {
        Tile tile;
        public bool IsOnTile = true; 
        float workTime;
        public IJobEvents jobEvents;
        public Job(Tile tile, IJobEvents jobEvents, float workTime = 1f)
        {
            this.tile = tile;
            this.jobEvents = jobEvents;
            this.workTime = workTime;
        }
        public void Work(float deltaWork)
        {
            workTime -= deltaWork;
            if (workTime < 0) jobEvents.OnJobCompleted();
        }

    }
}

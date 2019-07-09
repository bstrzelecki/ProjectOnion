using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class PlaceJob : Job
	{
		public PlaceJob(Tile tile, IJobEvents jobEvents,bool onTile, float workTime = 1, int jobLayer = 0) : base(tile, jobEvents,onTile, workTime, jobLayer)
		{
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class ResourceJob : Job
	{
		public ResourceJob(Tile tile, IJobEvents jobEvents, bool onTile = true, float workTime = 1, JobLayer jobLayer = JobLayer.Misc) : base(tile, jobEvents, onTile, workTime, jobLayer)
		{
			this.tile = tile;
			this.jobLayer = jobLayer;
		}
		public ResourceJob(Tile tile, Resource[] resources, JobLayer jobLayer = JobLayer.Resource)
		{
			this.tile = tile;
			this.jobLayer = jobLayer;
		}
	}
}

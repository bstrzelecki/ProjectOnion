using MBBSlib.MonoGame;
using static ProjectOnion.ZoneManager;

namespace ProjectOnion
{
	class HaulJobManager : IUpdateable
	{
		public void Update()
		{
			foreach (Tile tile in MainScene.world)
			{
				if (IsZoneOnTile(tile))
				{
					continue;
				}
				if (tile.stackItem != null)
				{
					foreach (Zone dest in GetZones())
					{
						if (dest.HasPendingJob) continue;
						if (dest.GetTile().stackItem == null)
						{
							Job job = new HaulJob(dest.GetTile(), tile.stackItem);
							dest.InZoneJob = job;
							JobQueue.AddJob(job, JobLayer.Resource);
						}
						else if (dest.GetTile().stackItem.GetResource() == tile.stackItem.GetResource() && dest.GetTile().stackItem.GetAmount() + tile.stackItem.GetAmount() <= Tile.SIZE_STACK_LIMIT)
						{
							Job job = new HaulJob(dest.GetTile(), tile.stackItem);
							dest.InZoneJob = job;
							JobQueue.AddJob(job, JobLayer.Resource);
						}
					}
				}
			}
		}
	}
}

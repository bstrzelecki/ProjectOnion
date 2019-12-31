using System;
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
							Job job = new Job(dest.GetTile(), new HaulJobEvent(dest.GetTile(), tile.stackItem), true, 1, JobLayer.Resource);
							dest.InZoneJob = job;
							JobQueue.AddJob(job, JobLayer.Resource);
						}
						else if (dest.GetTile().stackItem.GetResource() == tile.stackItem.GetResource() && dest.GetTile().stackItem.GetAmount() + tile.stackItem.GetAmount() <= Tile.SIZE_STACK_LIMIT)
						{
							Job job = new Job(dest.GetTile(), new HaulJobEvent(dest.GetTile(), tile.stackItem), true, 1, JobLayer.Resource);
							dest.InZoneJob = job;
							JobQueue.AddJob(job, JobLayer.Resource);
						}
					}
				}
			}
		}
	}
	class HaulJobEvent : IJobEvents
	{
		Tile tile;
		ItemStack stack;
		public HaulJobEvent(Tile tile, ItemStack stack)
		{
			this.tile = tile;
			this.stack = stack;
		}
		public BlueprintData GetBlueprintData()
		{
			return BlueprintData.None;
		}

		public string[] GetSerializationData()
		{
			return new string[] { string.Empty };
		}

		public void OnJobCanceled()
		{

		}

		public void OnJobCompleted()
		{
			tile.PutItemStacks(stack);
		}

		public void OnJobSuspended()
		{
			throw new NotImplementedException();
		}
	}
}

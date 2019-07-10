﻿using System;

namespace ProjectOnion
{
	internal class Job : IDisposable
	{
		public Tile tile;
		public bool IsOnTile = true;
		private float workTime;
		public IJobEvents jobEvents;
		public int jobLayer;
		public bool onTile;
		public Job(Tile tile, IJobEvents jobEvents,bool onTile = true, float workTime = 1f, int jobLayer = 0)
		{
			this.tile = tile;
			this.jobEvents = jobEvents;
			this.workTime = workTime;
			this.jobLayer = jobLayer;
			this.onTile = onTile;
		}
		public void Work(float deltaWork)
		{
			workTime -= deltaWork;
			if (workTime < 0)
			{
				jobEvents.OnJobCompleted();
				this.Dispose();
			}
		}
		public bool IsDisposed;
		public void Dispose()
		{
			IsDisposed = true;
		}
		//FIXME
		public override bool Equals(object obj)
		{
			if (obj is Job job)
			{
				if (tile.X == job.tile.X && tile.Y == job.tile.Y)
				{
					if (job.jobLayer == this.jobLayer)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
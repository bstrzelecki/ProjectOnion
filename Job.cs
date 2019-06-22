namespace ProjectOnion
{
	internal class Job
	{
		private Tile tile;
		public bool IsOnTile = true;
		private float workTime;
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
			if (workTime < 0)
			{
				jobEvents.OnJobCompleted();
			}
		}

	}
}

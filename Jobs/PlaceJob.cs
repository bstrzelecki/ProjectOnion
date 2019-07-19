namespace ProjectOnion
{
	class PlaceJob : Job
	{
		public PlaceJob(Tile tile, IJobEvents jobEvents, bool onTile, float workTime = 1, JobLayer jobLayer = 0) : base(tile, jobEvents, onTile, workTime, jobLayer)
		{
		}
	}
}

using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Zone
	{
		protected Vector2 position;
		protected Tile tile;
		public Job InZoneJob;
		public bool HasPendingJob
		{
			get
			{
				if (InZoneJob == null) return false;
				if (InZoneJob.IsCompleted == true)
				{
					InZoneJob = null;
					return false;
				}
				return true;
			}
		}
		public Zone(Tile tile)
		{
			this.tile = tile;
			position = tile.Position;
		}
		public Tile GetTile()
		{
			return tile;
		}
	}
}

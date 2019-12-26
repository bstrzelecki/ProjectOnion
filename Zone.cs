using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class Zone
	{
		protected Vector2 position;
		protected Tile tile;
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

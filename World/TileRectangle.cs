using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal class TileRectangle
	{
		private int X, Y;
		public TileRectangle(int x, int y)
		{
			X = x;
			Y = y;
		}
		public TileRectangle(Vector2 pos)
		{
			X = (int)pos.X;
			Y = (int)pos.Y;
		}

		public static implicit operator Rectangle(TileRectangle rect)
		{
			return new Rectangle(GetCorner(new Vector2(rect.X, rect.Y)).ToPoint(), GetSize().ToPoint());
		}
		public static Vector2 GetCorner(Vector2 pos)
		{
			return new Vector2(pos.X * World.TileSize + World.Offset + (int)Input.cameraOffset.X, pos.Y * World.TileSize + World.Offset + (int)Input.cameraOffset.Y);
		}
		public static Vector2 GetSize()
		{
			return new Vector2(World.TileSize, World.TileSize);
		}
	}
}

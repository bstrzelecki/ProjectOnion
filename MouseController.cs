using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal static class MouseController
	{
		private static Vector2 initialPosition;
		public static void HandleDeltaMouse()
		{
			if (Input.IsMouseKeyDown(2))
			{
				Input.cameraOffset -= initialPosition - Input.GetMousePosition();
			}
			initialPosition = Input.GetMousePosition();
		}

		private static int initialScroll;
		public static void HandleDeltaScroll()
		{
			int diff = initialScroll - (Input.GetMouseScrollDelta() / 120);
			initialScroll = Input.GetMouseScrollDelta() / 120;
			World.TileSize -= diff * 6;
			//FIXME
			World.Offset += diff * 3 * MainScene.world.Width;
		}
		public static Vector2 GetMouseOnTilePosition()
		{
			return (Input.GetMousePosition() - Input.cameraOffset - new Vector2(World.Offset, World.Offset)) / World.TileSize;
		}
	}
}

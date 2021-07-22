using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class TileInfo : MBBSlib.MonoGame.IDrawable, MBBSlib.MonoGame.IUpdateable
	{
		public TileInfo()
		{
			GameMain.RegisterRenderer(this);
			GameMain.RegisterUpdate(this);
		}
		string data;
		public void Draw(RenderBatch sprite)
		{
			sprite.DrawString(GameMain.Instance.GetFont("font"), data, new Vector2(0, GameMain.Instance.GraphicsDevice.Viewport.Height / 2), Color.White);
		}

		public void Update()
		{
			Vector2 mousePos = MouseController.GetMouseOnTilePosition();
			Tile tile = MainScene.world.GetTile(mousePos);

			data = string.Empty;
			data += tile?.sprite.ToString() + ", " + tile?.mountedObject?.registryName + "\n";
			data += tile?.stackItem?.GetResource() + ": " + tile?.stackItem?.GetAmount() + "\n";
			data += tile?.character?.Name + ": " + tile?.character?.currentJob?.ToString() + "\n";
		}
	}
}

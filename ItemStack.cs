using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class ItemStack : MBBSlib.MonoGame.IDrawable
	{
		Tile tile;
		readonly string resource;
		int amount;
		public Vector2 Position { get { return tile.Position; } }
		public Resource ResourceData { get { return ResourceDataBase.GetResource(resource); } }
		public ItemStack(Tile tile, string resource, int amount)
		{
			this.tile = tile;
			this.resource = resource;
			this.amount = amount;
		}
		public ItemStack(string resource, int amount)
		{
			this.resource = resource;
			this.amount = amount;
		}
		public string GetResource()
		{
			return resource;
		}
		public void SetTile(Tile tile)
		{
			this.tile = tile;
		}
		public void AddToStack(int am)
		{
			amount += am;
		}
		public int GetAmount()
		{
			return amount;
		}
		public void SetAmount(int am)
		{
			amount = am;
			if (amount < 0)
				amount = 0;
		}
		public void Draw(SpriteBatch sprite)
		{
			if (tile == null) return;
			TileRectangle tilePos = new TileRectangle(Position);
			Rectangle rect = new Rectangle(((Rectangle)tilePos).Location + new Point(16, 16), ((Rectangle)tilePos).Size - new Point(16, 16));
			sprite.Draw(ResourceData.GetSprite(), rect, Color.White);
			if (World.TileSize > 47)
				sprite.DrawString(GameMain.fonts["font"], amount.ToString(), ((Rectangle)tilePos).Location.ToVector2() + new Vector2(8, 8), Color.White);
		}
		public override string ToString()
		{
			return resource;
		}
	}
}

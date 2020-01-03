using System;
using System.Collections.Generic;
using System.Linq;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	abstract class Job : MBBSlib.MonoGame.IDrawable
	{
		public Tile tile;
		public Character Owner { get; set; }
		public bool IsCompleted { get; protected set; }
		public float workTime = 1f;
		public float startWorkTime = 1f;
		public JobLayer jobLayer;
		public bool onTile = false;
		public BlueprintData data;
		public List<ItemStack> resources = new List<ItemStack>();
		protected List<ItemStack> suppliedResources = new List<ItemStack>();
		public Job(Tile tile)
		{
			this.tile = tile;
		}
		public abstract void OnComplete();
		public abstract void OnWork();
		public abstract void OnCancel();
		
		public virtual void Supply(ItemStack stack)
		{
			ItemStack r = (from n in resources where n.ResourceData == stack.ResourceData select n).First();
			r.SetAmount(r.GetAmount() - stack.GetAmount());
			suppliedResources.Add(stack);
			if(r.GetAmount() == 0)
			{
				resources.Remove(r);
			}
		}
		public virtual bool IsAvailable()
		{
			return true;
		}
		public virtual void Work(float deltaWork)
		{
			workTime -= deltaWork;
			if (workTime < 0)
			{
				Complete();
				return;
			}
			OnWork();
		}
		protected virtual void Complete()
		{
			OnComplete();
			IsCompleted = true;
		}
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
		public void Draw(SpriteBatch sprite)
		{
			if (IsCompleted)
			{
				return;
			}
			sprite.Draw(data.objectSprite, new TileRectangle(data.position), Color.White);
			DrawBlips(sprite, data);
		}
		private static void DrawBlips(SpriteBatch sprite, BlueprintData data)
		{
			Vector2 cornerPos = TileRectangle.GetCorner(data.position);
			int i = 0;
			foreach (Sprite blip in data.blips)
			{
				sprite.Draw(blip, new Rectangle((int)cornerPos.X + blip.Texture.Width * i, (int)cornerPos.Y, 8, 8), Color.White);
				i++;
			}
		}
	}
	struct BlueprintData
	{

		public static BlueprintData None
		{
			get
			{
				return new BlueprintData(Vector2.Zero, new Sprite("WhitePixel"));
			}
		}

		public Sprite objectSprite;
		public List<Sprite> blips;
		public Vector2 position;
		public BlueprintData(Vector2 position, Sprite objectSprite, params Sprite[] blips)
		{
			this.position = position;
			this.objectSprite = objectSprite;
			this.blips = new List<Sprite>();
			foreach (Sprite blip in blips)
			{
				this.blips.Add(blip);
			}
		}
	}
	enum JobLayer
	{
		Any,
		Misc,
		Build,
		Resource,
		Deconstruct
	}
}

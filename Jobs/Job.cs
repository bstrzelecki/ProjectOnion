using System;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class Job : IDrawable
	{
		public Tile tile;
		public Character Owner { get; set; }
		public bool IsOnTile = true;
		private float workTime;
		public IJobEvents jobEvents;
		public JobLayer jobLayer;
		public bool onTile;
		public Job(Tile tile, IJobEvents jobEvents,bool onTile = true, float workTime = 1f, JobLayer jobLayer = 0)
		{
			this.tile = tile;
			this.jobEvents = jobEvents;
			this.workTime = workTime;
			this.jobLayer = jobLayer;
			this.onTile = onTile;
		}

		internal void Register()
		{
			tile.job[(int)jobLayer] = this;
			GameMain.RegisterRenderer(this, 6);
		}

		public void Work(float deltaWork)
		{
			workTime -= deltaWork;
			if (workTime < 0)
			{
				jobEvents.OnJobCompleted();
				Complete();
			}
		}
		public bool IsCompleted { get; protected set; }
		protected void Complete()
		{
			GameMain.UnregisterRenderer(this);
			tile.job[(int)jobLayer] = null;
			IsCompleted = true;
		}
		public void Cancel()
		{
			GameMain.UnregisterRenderer(this);
			tile.job[(int)jobLayer] = null;
			IsCompleted = true;
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
		public void Draw(SpriteBatch sprite)
		{
			if (IsCompleted)
			{
				return;
			}

			BlueprintData data = jobEvents.GetBlueprintData();
			sprite.Draw(data.objectSprite, new TileRectangle(data.position), Microsoft.Xna.Framework.Color.White);
			DrawBlips(sprite, data);
		}
		private static void DrawBlips(SpriteBatch sprite, BlueprintData data)
		{
			Microsoft.Xna.Framework.Vector2 cornerPos = TileRectangle.GetCorner(data.position);
			int i = 0;
			foreach (Sprite blip in data.blips)
			{
				sprite.Draw(blip, new Microsoft.Xna.Framework.Rectangle((int)cornerPos.X + blip.Texture.Width * i, (int)cornerPos.Y, 8, 8), Microsoft.Xna.Framework.Color.White);
				i++;
			}
		}
		#region side-overrides
		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
	}
	enum JobLayer
	{
		Misc,
		Build
	}
}

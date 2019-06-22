using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	internal class JobQueue
	{
		public static Queue<Job> jobQueue = new Queue<Job>();

		public static void AddJob(Job job)
		{
			jobQueue.Enqueue(job);
		}
		public static Job GetJob()
		{
			return jobQueue.Dequeue();
		}
		public static void DrawBlueprints(SpriteBatch sprite)
		{
			foreach (Job job in jobQueue)
			{
				BlueprintData data = job.jobEvents.GetBlueprintData();
				sprite.Draw(data.objectSprite, new TileRectangle(data.position), Color.White);
				Vector2 cornerPos = ((Rectangle)new TileRectangle(data.position)).Location.ToVector2();
				int i = 0;
				foreach (Sprite blip in data.blips)
				{
					sprite.Draw(blip, new Rectangle((int)cornerPos.X + 8 * i, (int)cornerPos.Y, 8, 8), Color.White);
					i++;
				}
			}
		}
	}
}

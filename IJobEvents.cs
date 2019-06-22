using System.Collections.Generic;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	internal interface IJobEvents
	{
		void OnJobCompleted();
		void OnJobCanceled();
		void OnJobSuspended();
		BlueprintData GetBlueprintData();
	}

	internal struct BlueprintData
	{
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
}

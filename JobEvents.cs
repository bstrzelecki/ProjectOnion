using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	class JobEvents
	{
	}
	class FloorPlaceJob : IJobEvents
	{
		private Tile tile;
		private Sprite sprite;
		private float movementCostReduction;

		public FloorPlaceJob(Tile tile, Sprite sprite, float movementCostReduction = 0f)
		{
			this.tile = tile;
			this.sprite = sprite;
			this.movementCostReduction = movementCostReduction;
			bp = new BlueprintData(new Vector2(tile.X, tile.Y), sprite, new Sprite("bp"));
		}
		BlueprintData bp;
		bool initialized = false;
		public BlueprintData GetBlueprintData()
		{
			return bp;
		}

		public void OnJobCanceled()
		{
			
		}

		public void OnJobCompleted()
		{
			tile.IsFloor = true;
			tile.sprite = sprite;
			tile.movementCost -= movementCostReduction;
		}

		public void OnJobSuspended()
		{
			
		}
	}
}

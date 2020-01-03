using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;

namespace ProjectOnion
{
	class DeconstructJob : Job
	{
		public DeconstructJob(Tile tile) : base(tile)
		{
			data = new BlueprintData(tile.Position, tile.mountedObject?.sprite ?? tile.sprite, new Sprite("ds"));
			jobLayer = JobLayer.Deconstruct;
		}

		public override void OnCancel()
		{
			
		}

		public override void OnComplete()
		{
			if (tile.mountedObject != null)
				tile.DeconstructObject();
			else
			{
				tile.sprite = new Sprite("grid");
				tile.IsFloor = false;
			}
		}

		public override void OnWork()
		{

		}
	}
}

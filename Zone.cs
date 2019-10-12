﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ProjectOnion
{
	abstract class Zone
	{
		protected Vector2 position;
		protected Tile tile;
		public Zone(Tile tile)
		{
			this.tile = tile;
			position = tile.Position;
		}
		public abstract Job GetZoneJob();
		public abstract bool IsJobAvailable();
	}
}
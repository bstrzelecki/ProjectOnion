using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
	class Architect
	{
		public static void SetBuildObject(BuildMode buildMode, BuildType buildType, NewFurniture mountedObject = null)
		{
			QueueBuilder.buildType = buildType;
			QueueBuilder.buildMode = buildMode;
			BuildController.buildMode = buildMode;

			if (mountedObject != null)
				QueueBuilder.toBuild = mountedObject;
		}
	}
}

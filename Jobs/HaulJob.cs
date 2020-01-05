using System.Linq;
using System.Xml.Linq;

namespace ProjectOnion
{
	class HaulJob : Job
	{
		public HaulJob(Tile tile, ItemStack stack) : base(tile)
		{
			onTile = true;
			resources.Add(new ItemStack(stack.GetResource(), stack.GetAmount()));
			jobLayer = JobLayer.Resource;
			flags.Add("haul");
		}
		public HaulJob(Tile tile) : base(tile)
		{
			onTile = true;
			jobLayer = JobLayer.Resource;
			flags.Add("haul");
		}
		public override XElement GetCtorData()
		{
			return new XElement("Ctor");
		}

		public override bool IsAvailable()
		{
			return (resources == null || resources.Count == 0) || (from n in resources where n.GetAmount() > 0 select n).Count() == 0;
		}
		public override void OnCancel()
		{

		}

		public override void OnComplete()
		{
			tile.PutItemStacks(suppliedResources.ToArray());
		}

		public override void OnWork()
		{

		}
	}
}

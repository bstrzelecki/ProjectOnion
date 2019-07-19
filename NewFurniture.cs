namespace ProjectOnion
{
	internal abstract class NewFurniture
	{
		protected MountedObject obj;

		protected abstract void SetProperties();

		public virtual MountedObject GetFurniture()
		{
			obj = new MountedObject();
			SetProperties();
			return obj;
		}
		public override bool Equals(object obj)
		{
			if (obj is NewFurniture nf)
			{
				return GetFurniture().registryName == nf.GetFurniture().registryName;
			}
			if (obj is MountedObject mo)
			{
				return GetFurniture().registryName == mo.registryName;
			}
			return false;
		}
	}
}

using System;

namespace ProjectOnion
{
	internal abstract class NewFurniture
	{
		protected MountedObject obj;
		protected Action OnClickAction;

		protected abstract void SetProperties();

		public virtual MountedObject GetFurniture()
		{
			obj = new MountedObject();
			obj.serializer = this is ITagCompound c ? c : null;
			SetProperties();
			return obj;
		}

		public Action GetOnClickAction()
		{
			return OnClickAction;
		}
		public virtual IUIItem GetUIItem()
		{
			return new FurnitureItem(GetFurniture(), OnClickAction);
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

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}

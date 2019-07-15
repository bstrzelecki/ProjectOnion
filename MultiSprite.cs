using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBBSlib.MonoGame;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectOnion
{
	class MultiSprite : Sprite
	{
		public string Variant { get; protected set; } = string.Empty;

		public void SetTextureVariant(string variant)
		{
			Variant = variant;

			if(Variant == string.Empty || !TextureStorage.ContainsTextureKey($"{textureName}_{Variant}"))
			{
				Texture = TextureStorage.GetTexture(textureName);
			}
			else
			{
				Texture = TextureStorage.GetTexture($"{textureName}_{Variant}");
			}

		}
		public MultiSprite(string sprite) : base(sprite)
		{
		}
	}
}

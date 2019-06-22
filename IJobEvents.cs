using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    interface IJobEvents
    {
        void OnJobCompleted();
        void OnJobCanceled();
        void OnJobSuspended();
        BlueprintData GetBlueprintData();
    }
    struct BlueprintData
    {
        public Sprite objectSprite;
        public List<Sprite> blips ;
        public Vector2 position;
        public BlueprintData(Vector2 position, Sprite objectSprite, params Sprite[] blips)
        {
            this.position = position;
            this.objectSprite = objectSprite;
            this.blips = new List<Sprite>();
            foreach (Sprite blip in blips) {
                this.blips.Add(blip);
            }
        }
    }
}

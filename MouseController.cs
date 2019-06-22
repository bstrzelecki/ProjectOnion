using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnion
{
    class MouseController
    {
        static Vector2 initialPosition;
        public static void HandleDeltaMouse()
        {
            if (Input.IsMouseKeyDown(2))
            {
                Input.cameraOffset -= initialPosition - Input.GetMousePosition();
            }
            initialPosition = Input.GetMousePosition();
        }
        static int initialScroll;
        public static void HandleDeltaScroll()
        {
            int diff = initialScroll - (Input.GetMouseScrollDelta()/ 120);
            initialScroll = Input.GetMouseScrollDelta() / 120;
            World.TileSize -= diff * 6;
            World.Offset += diff * 3;
        }
    }
}

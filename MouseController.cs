using MBBSlib.MonoGame;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
            if (Input.IsMouseKeyDown(0))
            {
                Input.cameraOffset -= initialPosition - Input.GetMousePosition();
            }
            initialPosition = Input.GetMousePosition();
        }
        public static void HandleDeltaScroll()
        {
            //TODO implement scroll delta in MBBSlib
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace Test_Roguelike.Core
{
    public class Colors
    {
        public static RLColor FloorBackground = Swatch.Primary;
        public static RLColor Floor = Swatch.Secondary3Lighter;
        public static RLColor FloorBackgroundFov = Swatch.Primary2;
        public static RLColor FloorFov = Swatch.Primary2Lighter;

        public static RLColor WallBackground = Swatch.Secondary3Lighter;
        public static RLColor Wall = Swatch.Secondary3;
        public static RLColor WallBackgroundFov = Swatch.Primary2Darkest;
        public static RLColor WallFov = Swatch.Primary2Darker;

        public static RLColor Door = Swatch.ComplementDarker;
        public static RLColor DoorBackground = Swatch.Primary;
        public static RLColor DoorBackgroundFov = Swatch.Complement;
        public static RLColor DoorFov = Swatch.ComplementLighter;

        public static RLColor Text = RLColor.White;
        public static RLColor TextHeading = RLColor.White;

        public static RLColor Player = Swatch.Primary;

        public static RLColor GhostColor = RLColor.Black;


    }
}

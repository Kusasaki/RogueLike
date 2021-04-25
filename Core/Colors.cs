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
        public static RLColor Floor = Swatch.Alternate;
        public static RLColor FloorBackgroundFov = Swatch.AlternateLighter;
        public static RLColor FloorFov = Swatch.AlternateLightest;

        public static RLColor WallBackground = Swatch.Primary;
        public static RLColor Wall = Swatch.Complement;
        public static RLColor WallBackgroundFov = Swatch.ComplementLighter;
        public static RLColor WallFov = Swatch.ComplementLightest;

        public static RLColor TextHeading = RLColor.White;
    }
}

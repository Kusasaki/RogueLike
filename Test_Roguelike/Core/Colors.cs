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
        public static RLColor FloorBackground = Swatch.Primary; //couleur des points
        public static RLColor Floor = Swatch.ComplementDarker; //couleur du sol
        public static RLColor FloorBackgroundFov = Swatch.AlternateLighter;
        public static RLColor FloorFov = Swatch.AlternateLightest;

        public static RLColor WallBackground = Swatch.Primary; //couleur du fond derrière le donjon
        public static RLColor Wall = Swatch.ComplementDarkest; //couleur des murs
        public static RLColor WallBackgroundFov = Swatch.ComplementLighter;
        public static RLColor WallFov = Swatch.ComplementLightest;

        public static RLColor TextHeading = RLColor.White;
    }
}

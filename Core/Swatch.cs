using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace Test_Roguelike.Core
{
    class Swatch
    {
        public static RLColor PrimaryLightest = new RLColor(95, 140, 169);
        public static RLColor PrimaryLighter = new RLColor(41, 113, 159);
        public static RLColor Primary = new RLColor(10, 78, 122);
        public static RLColor PrimaryDarker = new RLColor(3, 49, 79);
        public static RLColor PrimaryDarkest = new RLColor(1, 33, 53);

        public static RLColor SecondaryLightest = new RLColor(109, 113, 183);
        public static RLColor SecondaryLighter = new RLColor(52, 62, 173);
        public static RLColor Secondary = new RLColor(21, 27, 133);
        public static RLColor SecondaryDarker = new RLColor(8, 12, 86);
        public static RLColor SecondaryDarkest = new RLColor(2, 5, 58);

        public static RLColor AlternateLightest = new RLColor(255, 224, 136);
        public static RLColor AlternateLighter = new RLColor(249, 198, 52);
        public static RLColor Alternate = new RLColor(191, 142, 4);
        public static RLColor AlternateDarker = new RLColor(124, 92, 0);
        public static RLColor AlternateDarkest = new RLColor(83, 61, 0);

        public static RLColor ComplementLightest = new RLColor(255, 202, 136);
        public static RLColor ComplementLighter = new RLColor(249, 162, 52);
        public static RLColor Complement = new RLColor(191, 108, 4);
        public static RLColor ComplementDarker = new RLColor(124, 69, 0);
        public static RLColor ComplementDarkest = new RLColor(83, 46, 0);
    }
}

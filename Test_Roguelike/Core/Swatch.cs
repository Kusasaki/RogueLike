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
        //dégradé de bleu marine
        public static RLColor PrimaryLightest = new RLColor(95, 140, 169);
        public static RLColor PrimaryLighter = new RLColor(41, 113, 159);
        public static RLColor Primary = new RLColor(10, 78, 122);
        public static RLColor PrimaryDarker = new RLColor(3, 49, 79);
        public static RLColor PrimaryDarkest = new RLColor(1, 33, 53);

        //dégradé de vert olive gris
        public static RLColor Primary2Lightest = new RLColor(136, 157, 128);
        public static RLColor Primary2Lighter = new RLColor(215, 224, 212);
        public static RLColor Primary2 = new RLColor(174, 189, 168);
        public static RLColor Primary2Darker = new RLColor(104, 129, 94);
        public static RLColor Primary2Darkest = new RLColor(70, 94, 62);

        //bleu roi un peu violet
        public static RLColor SecondaryLightest = new RLColor(109, 113, 183);
        public static RLColor SecondaryLighter = new RLColor(52, 62, 173);
        public static RLColor Secondary = new RLColor(21, 27, 133);
        public static RLColor SecondaryDarker = new RLColor(8, 12, 86);
        public static RLColor SecondaryDarkest = new RLColor(2, 5, 58);

        //gris taupe
        public static RLColor Secondary2Lightest = new RLColor(66, 63, 72);
        public static RLColor Secondary2Lighter = new RLColor(127,  123,  134);
        public static RLColor Secondary2 = new RLColor(96, 91,  103);
        public static RLColor Secondary2Darker = new RLColor(28, 27, 31);
        public static RLColor Secondary2Darkest = new RLColor(25, 19, 33);

        //gris olive
        public static RLColor Secondary3Lightest = new RLColor(58, 64, 67);
        public static RLColor Secondary3Lighter = new RLColor(114, 122, 126);
        public static RLColor Secondary3 = new RLColor(85, 92, 96);
        public static RLColor Secondary3Darker = new RLColor(25, 28, 29);
        public static RLColor Secondary3Darkest = new RLColor(18, 26, 31);

        //jaune canari foncé
        public static RLColor AlternateLightest = new RLColor(170, 57, 57);
        public static RLColor AlternateLighter = new RLColor(170, 57, 57);
        public static RLColor Alternate = new RLColor(191, 142, 4);
        public static RLColor AlternateDarker = new RLColor(124, 92, 0);
        public static RLColor AlternateDarkest = new RLColor(83, 61, 0);

        //orange
        public static RLColor ComplementLightest = new RLColor(255, 202, 136);
        public static RLColor ComplementLighter = new RLColor(170, 57, 57);
        public static RLColor Complement = new RLColor(191, 108, 4);
        public static RLColor ComplementDarker = new RLColor(124, 69, 0);
        public static RLColor ComplementDarkest = new RLColor(83, 46, 0);

        public static RLColor DbLight = new RLColor(83, 46, 0);
    }
}

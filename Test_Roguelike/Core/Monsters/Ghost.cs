using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Interfaces;

namespace Test_Roguelike.Core.Monsters
{
    public class Ghost : Monster
    {
        public static Ghost Create()
        {
            Ghost ghost = new Ghost();
            ghost.Name = "Jean Bon Bheur";
            ghost.Health = 15;
            ghost.MaxHealth = 14;
            ghost.PAttack = 0;
            ghost.FAttack = 2;
            ghost.Defense = 5;
            ghost.Resistance = 0;
            ghost.Agility = 5;
            ghost.Awareness = 20;
            ghost.Symbol = 'G';
            ghost.Color = Swatch.PrimaryDarkest;
            return ghost;
        }
    }
}

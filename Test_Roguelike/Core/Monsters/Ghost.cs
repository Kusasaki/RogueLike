using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace Test_Roguelike.Core.Monsters
{
    public class Ghost : Monster
    {
        public static Ghost Create()
        {
            Ghost ghost = new Ghost();
            ghost.Health = 15;
            ghost.MaxHealth = 14;
            ghost.PAttack = 0;
            ghost.FAttack = 2;
            ghost.Defense = 5;
            ghost.Resistance = 0;
            ghost.Agility = 5;
            ghost.Awareness = 20;

            return ghost;
        }
    }
}

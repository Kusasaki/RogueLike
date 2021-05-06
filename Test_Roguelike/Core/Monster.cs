using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Interfaces;
using Test_Roguelike.Systems;
using Test_Roguelike.Core.Behaviours;

namespace Test_Roguelike.Core
{
    public class Monster : Actor
    {
        public int? TurnsAlerted { get; set; }

        //Fait jouer le monstre
        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }

        //Affiche la barre de vie du monstre
        public void DrawStats(RLConsole statConsole, int position)
        {
            int yPosition = 13 + (position * 2);

            statConsole.Print(1, yPosition, Symbol.ToString(), RLColor.White);

            int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            statConsole.SetBackColor(3, yPosition, width, 1, Swatch.Primary);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);

            statConsole.Print(2, yPosition, $": {Name} + {Health}", RLColor.White);
        }
    }
}

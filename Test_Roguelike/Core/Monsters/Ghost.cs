using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Interfaces;
using RogueSharp.DiceNotation;

namespace Test_Roguelike.Core.Monsters
{
    public class Ghost : Monster
    {
        public static Ghost Create(int Level)
        {
            Ghost ghost = new Ghost();
            ghost.Name = "Jean Bon Bheur";
            ghost.MaxHealth = 14 + (int)Math.Ceiling(Level / 2.0) * Dice.Roll("1D4");
            ghost.Health = ghost.MaxHealth;
            ghost.PAttack = 1;
            ghost.FAttack = 2 + (int)Math.Floor(Level / 2.0) * Dice.Roll("1D4");
            ghost.Defense = 2 + (int)Math.Ceiling(Level / 2.0) * Dice.Roll("1D4");
            ghost.Resistance = 0;
            ghost.Agility = 5 + (int)Math.Ceiling(Level/2.0) * Dice.Roll("1D6");
            ghost.Awareness = 20;
            ghost.Symbol = 'F';
            ghost.Color = Swatch.PrimaryDarkest;
            ghost.Speed = 25 - (int)Math.Ceiling(Level / 2.0) * (Dice.Roll("1D2") - 1);
            ghost.Inventory = new List<Item>(9);
            return ghost;
        }
    }
}

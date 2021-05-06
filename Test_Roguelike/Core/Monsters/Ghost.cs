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
            ghost.Name = CreateName();
            ghost.MaxHealth = 14 + Level * Dice.Roll("1D4");
            ghost.Health = ghost.MaxHealth;
            ghost.PAttack = 1;
            ghost.FAttack = 2 + (int)Math.Floor(Level / 2.0) * Dice.Roll("1D4");
            ghost.Defense = 2 + (int)Math.Ceiling(Level / 2.0) * Dice.Roll("1D4");
            ghost.Resistance = 0;
            ghost.Agility = 5 + (int)Math.Ceiling(Level/2.0) * Dice.Roll("1D6");
            ghost.Awareness = 20;
            ghost.Symbol = 'F';
            ghost.Color = Swatch.PrimaryDarkest;
            ghost.Speed = 30 - (int)Math.Ceiling(Level / 2.0) * (Dice.Roll("1D2") - 1);
            ghost.Inventory = new List<Item>(9);
            return ghost;
        }

        private static string CreateName()
        {
            string[] names = new string[] { "De Vinci",
                                            "Maxwell",
                                            "Laplace",
                                            "S. De Beauvoir",
                                            "Mobius",
                                            "Musashi",
                                            "A. Wright",
                                            "B. Wright",
                                            "Schrodinger",
                                            "Fabre",
                                            "Darwin",
                                            "Tereshkova",
                                            "Nightingale",
                                            "P. Guggenheim",
                                            "K. Johnson",
                                            "A. Lovelace",
                                            "M. Curie",
                                            "S. Veil",
                                            "R. Menchu",
                                            "A. Earhart",
                                             };
            Random rnd = new Random();
            return names[rnd.Next(0, names.Length)];
        }
    }
}

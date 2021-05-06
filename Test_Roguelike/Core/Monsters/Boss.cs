using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Roguelike.Interfaces;
using Test_Roguelike.Core.Items;
using RogueSharp.DiceNotation;

namespace Test_Roguelike.Core.Monsters
{
    public class Boss : Monster
    {
        public static Boss Create(int Level)
        {
            Boss boss = new Boss();
            boss.MaxHealth = 20 + 2 * Level + 2 * Level * (Dice.Roll("1D4"));
            boss.Health = boss.MaxHealth;
            boss.PAttack = Level + (int)(Level / 1.5) * Dice.Roll("1D3");
            boss.Defense = (int)Math.Floor(Level / 2.0) * Dice.Roll("1D4");
            boss.Resistance = 0;
            boss.Agility = (int)Math.Ceiling(Level / 2.0) * Dice.Roll("1D6");
            boss.Awareness = 20;
            boss.Symbol = 'B';
            boss.Color = Swatch.PrimaryDarkest;
            boss.Speed = 20 - (int)Math.Ceiling(Level / 2.0) * (Dice.Roll("1D2"));
            boss.Inventory = new List<Item>(9);

            switch(Level)
            {
                case 1:
                    boss.Name = "Cleopatre";
                    boss.Inventory.Add(new Weapon("Couteau aiguise", 4, 10));
                    boss.Inventory.Add(new Potion(10, 7, 5, 7, 0, 5));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 2:
                    boss.Name = "Kumaragupta";
                    boss.Inventory.Add(new Weapon("Hachoir tranchant", 5, 15));
                    boss.Inventory.Add(new Potion(12, 6, 10, 3, 2, 0));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 3:
                    boss.Name = "Moctezuma";
                    boss.Inventory.Add(new Weapon("Lance aiguisee", 7, 30));
                    boss.Inventory.Add(new Potion(14, 6, 6, 14, 2, 0));
                    boss.Inventory.Add(new Potion("Agility", Dice.Roll("1D5")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 4:
                    boss.Name = "Catherine II";
                    boss.Inventory.Add(new Weapon("Fleuret de Combat", 9, 40));
                    boss.Inventory.Add(new Potion(25, 0, 10, 10, 2, Dice.Roll("1D10")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 5:
                    boss.Name = "Douglas MacArthur";
                    boss.Inventory.Add(new Key(10));
                    break;
            }
            
            return boss;
        }
    }
}

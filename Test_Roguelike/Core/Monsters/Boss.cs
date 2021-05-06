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
            boss.MaxHealth = 20 + 2 * Level * (Dice.Roll("1D4") + 1);
            boss.Health = boss.MaxHealth;
            boss.PAttack = 4 + (int)Math.Floor(Level / 2.0) * Dice.Roll("1D6");
            boss.Defense = 2 + (int)Math.Floor(Level / 2.0) * Dice.Roll("1D4");
            boss.Resistance = 0;
            boss.Agility = 5 + (int)Math.Ceiling(Level / 2.0) * Dice.Roll("1D6");
            boss.Awareness = 20;
            boss.Symbol = 'B';
            boss.Color = Swatch.PrimaryDarkest;
            boss.Speed = 20 - (int)Math.Ceiling(Level / 2.0) * (Dice.Roll("1D2"));
            boss.Inventory = new List<Item>(9);

            switch(Level)
            {
                case 1:
                    boss.Name = "Cleopatre";
                    boss.Inventory.Add(new Weapon("Couteau aiguise", 3, 10));
                    boss.Inventory.Add(new Potion(5, 2, 1, 3, 0, 5));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 2:
                    boss.Name = "Kumaragupta";
                    boss.Inventory.Add(new Weapon("Hachoir tranchant", 5, 15));
                    boss.Inventory.Add(new Potion(12, 6, 0, 3, 4, 0));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 3:
                    boss.Name = "Moctezuma";
                    boss.Inventory.Add(new Weapon("Lance aiguisee", 7, 30));
                    boss.Inventory.Add(new Potion(0, 6, 4, 0, 3, 0));
                    boss.Inventory.Add(new Potion("Agility", Dice.Roll("1D5")));
                    boss.Inventory.Add(new Joke(Dice.Roll("1D4")));
                    break;

                case 4:
                    boss.Name = "Catherine II";
                    boss.Inventory.Add(new Weapon("Fleuret de Combat", 9, 40));
                    boss.Inventory.Add(new Potion(10, 0, 5, 5, 6, Dice.Roll("1D10")));
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

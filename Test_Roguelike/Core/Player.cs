using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Interfaces;
using Test_Roguelike.Core.Items;

namespace Test_Roguelike.Core
{
    public class Player : Actor
    {
        public bool IsAttacking { get; set; }
        public Actor Target { get; set; }
        public bool IsDead { get; set; }

        public Player()
        {
            PAttack = 2;
            FAttack = 0;
            Health = 50;
            MaxHealth = 50;
            Awareness = 12;
            Name = "Lilia";
            Color = Colors.Player;
            Symbol = 'O';
            Agility = 30;
            Speed = 2;
            X = 10;
            Y = 10;
            Inventory = new List<Item>();
            Inventory.Add(new Joke(3));
            Inventory.Add(new Joke(2));
            Weapon = new Weapon("Cure Dent", 1, 1);
            IsDead = false;
        }

        public bool GetKey(int Level)
        {
            if (Inventory.OfType<Key>().Any(k => k.Level == Game._mapLevel))
                return true;
            else
                return false;
        }

        public void Heal(int value)
        {
            Health += value;
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public void Consume(Potion potion)
        {
            Heal(potion.BoostHealth);
            
            MaxHealth += potion.BoostMaxHealth;
            Health += potion.BoostMaxHealth;
            
            Defense += potion.BoostDefense;
            if (Defense > 45)
                Defense = 45;

            Resistance += potion.BoostResistence;
            if (Resistance > 50)
                Resistance = 50;
            
            Speed -= potion.BoostSpeed;
            if (Speed <= 5)
                Speed = 5;

            Agility += potion.BoostAgility;
            if (Agility > 40)
                Agility = 40;
        }

        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Nom:  {Name}", Colors.Text);
            statConsole.Print(1, 2, $"Points de vie:  {Health}/{MaxHealth}", Colors.Text);

            statConsole.Print(1, 3, $"Attaque physique:  {PAttack}", Colors.Text);

            statConsole.Print(1, 4, $"Defense: {Defense}", Colors.Text);


            statConsole.Print(1, 5, $"Agilite: {Agility}", Colors.Text);
        }
    }
}

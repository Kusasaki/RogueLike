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
            PAttack = 3;
            FAttack = 0;
            Health = 75;
            MaxHealth = 75;
            Awareness = 12;
            Name = "Lilia";
            Color = Colors.Player;
            Symbol = 'O';
            Agility = 30;
            Speed = 12;
            X = 10;
            Y = 10;
            Inventory = new List<Item>();
            Inventory.Add(new Joke(3));
            Inventory.Add(new Joke(2));
            Weapon = new Weapon("Cure Dent", 1, 1);
            IsDead = false;
        }

        //Verifie si le joueur possede la cle du niveau en parametre
        public bool GetKey(int Level)
        {
            if (Inventory.OfType<Key>().Any(k => k.Level == Level))
                return true;
            else
                return false;
        }

        //Soigne le joueur de tant
        public void Heal(int value)
        {
            Health += value;
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        //Gere la consommation des potions avec un systeme de cap( on ne pas aller au dela d'une certaine valeur
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
            if (Speed <= 4)
                Speed = 4;

            Agility += potion.BoostAgility;
            if (Agility > 40)
                Agility = 40;
        }

        //Affiche les statistiques du joueur
        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Nom:  {Name}", Colors.Text);
            statConsole.Print(1, 2, $"Points de vie:  {Health}/{MaxHealth}", Colors.Text);

            statConsole.Print(1, 4, $"Attaque physique: {PAttack + Weapon.PAttackBoost}", Colors.Text);

            statConsole.Print(1, 6, $"Defense: {Defense}", Colors.Text);
            statConsole.Print(1, 7, $"Resistance: {Resistance}", Colors.Text);

            statConsole.Print(1, 9, $"Agilite: {Agility}", Colors.Text);
            statConsole.Print(1, 10, $"Vitesse: {Speed}", Colors.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Interfaces;

namespace Test_Roguelike.Core
{
    public class Player : Actor
    {
        public Player()
        {
            PAttack = 2;
            FAttack = 0;
            Awareness = 5;
            Health = 50;
            MaxHealth = 50;
            Awareness = 15;
            Name = "Rogue";
            Color = Colors.Player;
            Symbol = 'O';
            Agility = 30;
            Speed = 2;
            X = 10;
            Y = 10;
        }
        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Nom:    {Name}       X:  {X}   Y:  {Y}", Colors.Text);
            statConsole.Print(1, 2, $"Points de vie:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 3, $"Attaque physique:  {PAttack}", Colors.Text);
            statConsole.Print(1, 4, $"Defense: {Defense}", Colors.Text);
            statConsole.Print(1, 5, $"Agilite: {Agility}", Colors.Text);
        }
    }
}

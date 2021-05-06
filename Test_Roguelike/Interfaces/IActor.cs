using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Core;
using Test_Roguelike.Core.Items;


namespace Test_Roguelike.Interfaces
{
    //Permet de factoriser le code, car tous les acteurs (monstres et joueur) hériteront des propriétés et des accesseurs
    public interface IActor
    {
        string Name { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        int PAttack { get; set; }
        int FAttack { get; set; }
        int Defense { get; set; }
        int Resistance { get; set; }
        int Awareness { get; set; }
        int Speed { get; set; }
        Weapon Weapon { get; set; }
        List<Item> Inventory { get; set; }
    }
}

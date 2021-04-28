using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace Test_Roguelike.Interfaces
{
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
        
    }
}

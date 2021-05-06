using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RLNET;
using Test_Roguelike.Core;
using Test_Roguelike.Systems;

namespace Test_Roguelike.Interfaces
{
    //Permet de factoriser le code pour Boss et Ghost qui hériteront de act
    public interface IBehavior
    {
        bool Act(Monster monster, CommandSystem commandSystem);
    }
}

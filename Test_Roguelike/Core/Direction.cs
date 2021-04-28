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
    // Direction values correspond to numpad numbers
    public enum Direction
    {
        None = 0,
        Down = 2,
        Left = 4,
        Center = 5,
        Right = 6,
        Up = 8,
    }
}
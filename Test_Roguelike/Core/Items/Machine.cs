using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using Test_Roguelike.Core;

namespace Test_Roguelike.Core.Items
{
    public class Machine : Item
    {
        public bool Destroyed { get; set; }
        
        public Machine()
        {
            Name = "Machine";
            Symbol = 'M';
            Color = Colors.Player;
        }
    }
}

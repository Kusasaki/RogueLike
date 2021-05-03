using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Core.Items
{
    public class Key : Item
    {
        public int Level { get; }

        public Key(int level)
        {
            Level = level;
            Symbol = 'K';
            Pickable = true;
            Color = RLNET.RLColor.Black;
        }

        public override string ToString()
        {
            return " cle du niveau : " + Level;
        }
    }
}

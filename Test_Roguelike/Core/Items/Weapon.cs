using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Core.Items
{
    public class Weapon: Item
    {
        public int PAttackBoost { get; private set; }
        public int RiposteChance { get; private set; }

        public Weapon(string name, int dmg, int chance)
        {
            Name = name;
            PAttackBoost = dmg;
            RiposteChance = chance;
            Symbol = 'A';
        }

        public override string ToString()
        {
            return " arme : " + this.Name;
        }
    }
}

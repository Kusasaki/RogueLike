using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Core.Items
{
    public class Joke : Item
    {
        public int Damage { get; }
        public string Description { get; }
        public string Category { get; }

        public Joke(int dmg, string desc, string category)
        {
            Damage = dmg;
            Category = category;
            Description = desc;
            Symbol = 'J';
        }

        public string JokeDescGenerator(string category)
        {
            //TODO
            return "la";
        }

        public override string ToString()
        {
            return Description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Core.Items
{
    public class Potion : Item
    {
        public int BoostHealth { get; }
        public int BoostMaxHealth { get; }
        public int BoostDefense { get; }
        public int BoostResistence { get; }
        public int BoostSpeed { get; }
        public int BoostAgility { get; }

        public Potion(int Health, int MaxHealth, int Def, int Res, int Speed, int Agility)
        {
            BoostHealth = Health;
            BoostMaxHealth = MaxHealth;
            BoostDefense = Def;
            BoostResistence = Res;
            BoostSpeed = Speed;
            BoostAgility = Agility;
            Symbol = 'P';
        }

        public Potion(string type, int value)
        {
            Symbol = 'P';
            switch (type)
            {
                case "Health":
                    BoostHealth = value;
                    break;
                
                case "MaxHealth":
                    BoostMaxHealth = value;
                    break;
                
                case "Defense":
                    BoostDefense = value;
                    break;
                
                case "Resistence":
                    BoostResistence = value;
                    break;
                
                case "Speed":
                    BoostSpeed = value;
                    break;
                
                case "Agility":
                    BoostAgility = value;
                    break;
            }                      
        }

        public override string ToString()
        {
            string ch = "Potion ";
            if (BoostHealth != 0)
                ch = "de Sante " + BoostHealth;
            if (BoostMaxHealth != 0)
                ch = "de Vitalite " + BoostMaxHealth;
            if (BoostDefense != 0)
                ch = "de Defense " + BoostDefense;
            if (BoostResistence != 0)
                ch = "de Resistance " + BoostResistence;
            if (BoostSpeed != 0)
                ch = "de Vitesse " + BoostSpeed;
            if (BoostAgility != 0)
                ch = "d'Agilite " + BoostAgility;

            return base.ToString();
        }
    }
}

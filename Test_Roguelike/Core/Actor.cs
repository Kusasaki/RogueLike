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
    public class Actor : IActor, IDrawable, IScheduleable
    {
        //IActor
        private int _pAttack;
        private int _fAttack;
        private int _awareness;
        private int _defense;
        private int _resistance;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _agility;
        private int _speed;

        public int PAttack
        {
            get
            {
                return _pAttack;
            }
            set
            {
                _pAttack = value;
            }
        }

        public int FAttack
        {
            get
            {
                return _fAttack;
            }
            set
            {
                _fAttack = value;
            }
        }

        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }

        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }

        public int Resistance
        {
            get
            {
                return _resistance;
            }
            set
            {
                _resistance = value;
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Agility
        {
            get
            {
                return _agility;
            }
            set
            {
                _agility = value;
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        // IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.Floor, ' ');
            }
        }

        // IScheduleable
        public int Time
        {
            get
            {
                return Speed;
            }
        }
    }

}
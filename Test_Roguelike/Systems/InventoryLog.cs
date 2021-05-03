using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;


namespace Test_Roguelike.Systems
{
    public class InventoryLog
    {
        public void Draw(RLConsole console)
        {
            console.Clear();
            if (Game.Player.GetKey(Game._mapLevel))
                console.Print(1, 2, "Vous avez la clé de l'ascenceur", RLColor.White);

            console.Print(1, 1, Game.Player.Weapon.Name, RLColor.White);
        }
    }
}

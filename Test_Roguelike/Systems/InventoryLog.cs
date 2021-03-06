using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Core.Items;


namespace Test_Roguelike.Systems
{
    public class InventoryLog
    {
        //Affiche les informations sur l'inventaire du joueur
        public void Draw(RLConsole console)
        {
            console.Clear();
            if (Game.Player.GetKey(Game._mapLevel))
                console.Print(1, 3, "Vous avez la cle de l'ascenceur", RLColor.White);

            console.Print(1, 1, Game.Player.Weapon.Name, RLColor.White);
            console.Print(1, 5, "Vous avez " + Game.Player.Inventory.OfType<Joke>().Count() + " blagues", RLColor.White);

            if (Game.Player.GetKey(10))
                console.Print(1, 7, "Vous avez la cle de la machine", RLColor.White);
        }
    }
}

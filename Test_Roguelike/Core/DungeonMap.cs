using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Systems;
using Test_Roguelike.Core.Items;

namespace Test_Roguelike.Core
{
    //Map est une classe de RogueSharp
    public class DungeonMap : Map
    {
        public List<Rectangle> Rooms;
        public Stairs StairsUp { get; set; }
        public Stairs StairsDown { get; set; }
        private List<Monster> _monsters;
        public List<Door> Doors { get; set; }
        public List<Item> Items { get; protected set; }

        public DungeonMap()
        {
            Game.SchedulingSystem.Clear();
            Rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
            Doors = new List<Door>();
            Items = new List<Item>();
        }

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            mapConsole.Clear();
            //On dessine la carte
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
            //On dessine les portes
            foreach (Door door in Doors)
            {
                door.Draw(mapConsole, this);
            }
            //On dessine les items
            foreach (Item item in Items)
            {
                item.Draw(mapConsole, this);
            }
            //On dessine l'ascenseur
            StairsUp.Draw(mapConsole, this);
            if (StairsDown != null)
            {
                StairsDown.Draw(mapConsole, this);
            }
            int i = 0;
            //On dessine les monstres
            foreach (Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);
                if (IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            //concerne les murs
            //Si un mur n'a pas encore été dans le champ de vision, il est juste entièrement noir, sans symbole
            if (!cell.IsExplored)
            {
                return;
            }

            // Map niveau
            if (IsInFov(cell.X, cell.Y))
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.Floor, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }


            }
        }

        //Met a jour le champ de vision du joueur selon se position quand il se deplace ou qu'un objet rentre dans son champ de vision
        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;
            ComputeFov(player.X, player.Y, player.Awareness, true);
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        //Fait en sorte qu'un acteur puisse se deplacer sur une case et qu'on ne puisse pas le traversser
        public bool SetActorPosition(Actor actor, int x, int y)
        {
            if (GetCell(x, y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);
                actor.X = x;
                actor.Y = y;
                SetIsWalkable(actor.X, actor.Y, false);
                //S'il le joueur est sur une porte, il l'ouvre
                OpenDoor(actor, x, y);
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        //Pour factoriser le code
        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = (Cell)GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        //Permet de savoir si une porte existe a cette position
        public Door GetDoor(int x, int y)
        {
            return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
        }

        //Met la porte en position ouverte, soigne le joueur -> Temps de repos et permet une meilleure survie et promeut l'exploration des salles
        private void OpenDoor(Actor actor, int x, int y)
        {
            Door door = GetDoor(x, y);
            if (door != null && !door.IsOpen)
            {
                door.IsOpen = true;
                var cell = GetCell(x, y);
                SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);

                Game.Player.Heal(3);
                
                Game.MessageLog.Add($"{actor.Name} opened a door");
            }
        }

        //Verifie si le joueur a la cle du niveau et l'autorise a changer de niveau
        public bool CanMoveDownToNextLevel()
        {
            Player player = Game.Player;
            if (StairsDown != null && player.GetKey(Game._mapLevel))
            {
                Game.MessageLog.Add("Niveau suivant");
                player.Inventory.Remove(player.Inventory.Find(k => k is Key key && key.Level == Game._mapLevel));
                return StairsDown.X == player.X && StairsDown.Y == player.Y;
            }
            else
            {
                Game.MessageLog.Add("Vous ne pouvez pas prendre l'ascenceur, il vous manque la cle ou vous n'etes pas au bon endorit");
                return false;
            }
        }

        //Ajoute le joueur sur la carte et dans l'emploi du temps
        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();

            Game.SchedulingSystem.Add(player);
        }

        //Permet le retrait d'un item de la carte
        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        //Permet de connaitre l'item sur une certaine case
        public Item GetItemAt(int x, int y)
        {
            return Items.FirstOrDefault(i => i.X == x && i.Y == y);
        }

        //Ajoute le monstre sur la carte et dans l'emploi du temps
        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            // Fait en sorte qu'on ne soit pas sur la meme case que le monstre
            SetIsWalkable(monster.X, monster.Y, false);
            Game.SchedulingSystem.Add(monster);
        }

        //Eneleve un monstre de la carte et de l'edt
        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            // After removing the monster from the map, make sure the cell is walkable again
            SetIsWalkable(monster.X, monster.Y, true);
            Game.SchedulingSystem.Remove(monster);
        }

        //Renvoie le monstre sur la case x,y
        public Monster GetMonsterAt(int x, int y)
        {
            return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
        }


        // Renvoie une case sur laquelle on peut marcher dans la salle
        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }

            // Si on ne trouve pas, on renvoit ce point
            return new Point(0,0);
        }

        //Est ce qu'il y au moins une case sur laquelle on peut marcher
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
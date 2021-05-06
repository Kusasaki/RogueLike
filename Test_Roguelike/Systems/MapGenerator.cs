using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RogueSharp.DiceNotation;
using Test_Roguelike.Core;
using Test_Roguelike.Core.Items;
using Test_Roguelike.Core.Monsters;

namespace Test_Roguelike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly List<int[]> coordinates;
        private readonly List<int[]> doors;
        private readonly int _level;
        private readonly static int _maxlevel = 5;

        private readonly DungeonMap _map;

        // Gestion de la création des niveaux
        public MapGenerator(int width, int height, int mapLevel)
        {
            _width = width;
            _height = height;
            _level = mapLevel;
            _map = new DungeonMap();

            //Listes des coordonnees des salles pour la creation
            coordinates = new List<int[]>();

            //Salle du départ du joueur avec l'ascenseur
            coordinates.Add(new int[4] { 40, 14, 40, 6 });

            coordinates.Add(new int[4] { 34, 14, 6, 6 });
            coordinates.Add(new int[4] { 34, 10, 6, 20 });
            coordinates.Add(new int[4] { 34, 10, 6, 30 });
            coordinates.Add(new int[4] { 34, 12, 6, 40 });

            coordinates.Add(new int[4] { 40, 32, 40, 20 });

            coordinates.Add(new int[4] { 34, 14, 80, 6 });
            coordinates.Add(new int[4] { 34, 10, 80, 20 });
            coordinates.Add(new int[4] { 34, 10, 80, 30 });
            coordinates.Add(new int[4] { 34, 12, 80, 40 });

            //Liste des portes du niveau avec les coordonnees de la case
            doors = new List<int[]>();
            
            doors.Add(new int[2] { 14, 20 });
            doors.Add(new int[2] { 14, 30 });
            doors.Add(new int[2] { 14, 40 });
            doors.Add(new int[2] { 40, 35 });

            doors.Add(new int[2] { 60, 20 });

            doors.Add(new int[2] { 94, 20 });
            doors.Add(new int[2] { 94, 30 });
            doors.Add(new int[2] { 94, 40 });
            doors.Add(new int[2] { 80, 35 });
        }

        // Crée la carte 
        public DungeonMap CreateMap()
        {
            // Initialise les cases
            _map.Initialize(_width, _height);

            // Place les salles selon les coordonnees données dans la liste
            foreach (int[] coords in coordinates)
            { 
                int roomWidth = coords[0];
                int roomHeight = coords[1];
                int roomXPosition = coords[2];
                int roomYPosition = coords[3];

                // Les salles sont représentées par des rectangles en structure de données
                var newRoom = new Rectangle(roomXPosition, roomYPosition,
                  roomWidth, roomHeight);

                // Verifie si les salles de la carte ne se chevauchent pas
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }
            // Cree la salle et leui rajoute les portes
            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
                CreateDoors(room);
            }
            // Petite blague bonus dans le niveau -> On vous la laisse au cas ou
            var joke = new Joke(1);
            joke.X = 8;
            joke.Y = 8;
            _map.Items.Add(joke);
            
            //Instancie l'ascenseur, le joueur, les items et les monstres
            CreateStairs();
            PlacePlayer();
            if(_level == 5)
                PlaceMachine();
            PlaceBoss();
            PlaceMonsters();
            PlacePotions();
            return _map;
        }

        //Place le joueur au centre de la premiere salle
        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        private void CreateDoors(Rectangle room)
        {
            // place une porte dans la salle avec les informations sur la salle
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            //Regarde si une porte doit être placee a cet endroit et s'il n'y en a pas deja 
            foreach (Cell cell in _map.GetAllCells())
            {
                int[] tabTest = new int[2] { cell.X, cell.Y };
                if (doors.Any(s => s.SequenceEqual(tabTest)) && _map.GetDoor(cell.X, cell.Y) == null)
                {
                    // Un porte fermee bloque la vue
                    _map.SetCellProperties(cell.X, cell.Y, false, true);
                    _map.Doors.Add(new Door
                    {
                        X = cell.X,
                        Y = cell.Y,
                        IsOpen = false
                    });
                }
            }
        }

        // fait en sorte qu'on puisse marcher, explore et voir à travers les cases interieures de la salle 
        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }

        //Genere l'ascenseur montant à chaque niveau sauf pour le dernier
        private void CreateStairs()
        {
            _map.StairsUp = new Stairs
            {
                X = _map.Rooms.First().Center.X - 1,
                Y = _map.Rooms.First().Center.Y,
                IsUp = true
            };
            if(_level < _maxlevel)
                _map.StairsDown = new Stairs
                {
                    X = _map.Rooms.First().Center.X + 1,
                    Y = _map.Rooms.First().Center.Y,
                    IsUp = false
                };
        }

        //Genere les monstres
        private void PlaceMonsters()
        {
            //bool pour indiquer si la cle de l'etage a ete donnee à un monstre
            bool keyCreated = false;
            int j = 0;
            foreach (var room in _map.Rooms)
            {
                // Chaque salle a une chance sur ici 7 de contenir des monstres
                if (Dice.Roll("1D10") < 7)
                {
                    // Cree entre un et deux monstres
                    var numberOfMonsters = Dice.Roll("1D2");
                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        // Trouve une case sur laquelle on peut marcher (pour eviter de creer un monstre dans un mur)
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        
                        if (randomRoomLocation != null)
                        {
                            var monster = Ghost.Create(_level);
                            //Creation de la cle du niveau pour un seul monstre sur l'etage -> difficulte acccrue
                            if ((!keyCreated && j != 0 && Dice.Roll("1D10") < 4) || (!keyCreated && j < (_map.Rooms.Count - 4)))
                            {
                                monster.Inventory.Add(new Key(_level));
                                keyCreated = true;
                            }   
                            monster.X = randomRoomLocation.X;
                            monster.Y = randomRoomLocation.Y;

                            //Les monstres ont quand meme quelques blagues sur eux, preuve de leur passe d'humain ...
                            if (Dice.Roll("1D10") < 9)
                                monster.Inventory.Add(new Joke(Dice.Roll("1D4")));
                            _map.AddMonster(monster);
                        }
                    }
                }
                j++;
            }
        }
        // Place le boss au centre d'une salle qui n'est pas la premiere Salle
        private void PlaceBoss()
        {
            Boss _levelBoss = Boss.Create(_level);
            Random rnd = new Random();
            var room = _map.Rooms[rnd.Next(1, _map.Rooms.Count)];
            _levelBoss.X = room.Center.X;
            _levelBoss.Y = room.Center.Y;
            _map.AddMonster(_levelBoss);
        }

        //Place la machine du dernier etage
        private void PlaceMachine()
        {
            Machine machine = new Machine();
            Random rnd = new Random();
            var room = _map.Rooms[rnd.Next(1, _map.Rooms.Count)];
            machine.X = room.Center.X - 1;
            machine.Y = room.Center.Y;
            _map.Items.Add(machine);
        }

        //Place les potions de soin a travers le niveau
        private void PlacePotions()
        {
            foreach (var room in _map.Rooms)
            {
                //Sysyteme de generation similaire aux fantomes
                if (Dice.Roll("1D30") < 10)
                {
                    // Generate between 1 and 4 monsters
                    var numberOfItems = Dice.Roll("1D2");
                    for (int i = 0; i < numberOfItems; i++)
                    {
                        
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        
                        if (randomRoomLocation != null)
                        {
                            // Deux categories de potions peuvent être générées : les fortes et les petites
                            var option = Dice.Roll("1D2");
                            Potion potion;

                            switch (option)
                            {
                                case 1:
                                    potion = new Potion("Health", Dice.Roll("1D4"));
                                    potion.X = randomRoomLocation.X;
                                    potion.Y = randomRoomLocation.Y;
                                    _map.Items.Add(potion);
                                    break;
                                case 2:
                                    potion = new Potion("Health", Dice.Roll("1D8"));
                                    potion.X = randomRoomLocation.X;
                                    potion.Y = randomRoomLocation.Y;
                                    _map.Items.Add(potion);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

}

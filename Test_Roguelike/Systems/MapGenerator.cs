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

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        // as well as the sizes and maximum number of rooms
        public MapGenerator(int width, int height, int mapLevel)
        {
            _width = width;
            _height = height;
            _level = mapLevel;
            _map = new DungeonMap();
            coordinates = new List<int[]>();

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

        // Generate a new map that places rooms randomly
        public DungeonMap CreateMap()
        {
            // Set the properties of all cells to false
            _map.Initialize(_width, _height);

            // Try to place as many rooms as the specified maxRooms
            // Note: Only using decrementing loop because of WordPress formatting
            foreach (int[] coords in coordinates)
            {
                // Determine the size and position of the room randomly
                int roomWidth = coords[0];
                int roomHeight = coords[1];
                int roomXPosition = coords[2];
                int roomYPosition = coords[3];

                // All of our rooms can be represented as Rectangles
                var newRoom = new Rectangle(roomXPosition, roomYPosition,
                  roomWidth, roomHeight);

                // Check to see if the room rectangle intersects with any other rooms
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                // As long as it doesn't intersect add it to the list of rooms
                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }
            // Iterate through each room that we wanted placed 
            // call CreateRoom to make it
            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
                CreateDoors(room);
            }
            var joke = new Joke(1);
            joke.X = 8;
            joke.Y = 8;
            _map.Items.Add(joke);
            CreateStairs();
            PlacePlayer();
            PlaceBoss();
            PlaceMonsters();
            PlaceItems();
            return _map;
        }

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
            // The the boundries of the room
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            // Go through each of the rooms border cells and look for locations to place doors.
            foreach (Cell cell in _map.GetAllCells())
            {
                int[] tabTest = new int[2] { cell.X, cell.Y };
                if (doors.Any(s => s.SequenceEqual(tabTest)) && _map.GetDoor(cell.X, cell.Y) == null)
                {
                    // A door must block field-of-view when it is closed.
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

        // Given a rectangular area on the map
        // set the cell properties for that area to true
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


        private void PlaceMonsters()
        {
            bool keyCreated = false;
            int j = 0;
            foreach (var room in _map.Rooms)
            {
                // Each room has a 70% chance of having monsters
                if (Dice.Roll("1D10") < 7)
                {
                    // Generate between 1 and 4 monsters
                    var numberOfMonsters = Dice.Roll("1D3");
                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        // Find a random walkable location in the room to place the monster
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        // It's possible that the room doesn't have space to place a monster
                        // In that case skip creating the monster
                        if (randomRoomLocation != null)
                        {
                            // Temporarily hard code this monster to be created at level 1
                            var monster = Ghost.Create(_level);
                            if ((!keyCreated && j != 0 && Dice.Roll("1D10") < 2) || (!keyCreated && j == (_map.Rooms.Count - 1)))
                            {
                                monster.Inventory.Add(new Key(_level));
                                keyCreated = true;
                            }   
                            monster.X = randomRoomLocation.X;
                            monster.Y = randomRoomLocation.Y;
                            if (Dice.Roll("1D10") < 9)
                                monster.Inventory.Add(new Joke(Dice.Roll("1D4")));
                            _map.AddMonster(monster);
                        }
                    }
                }
                j++;
            }
        }

        private void PlaceBoss()
        {
            Boss _levelBoss = Boss.Create(_level);
            Random rnd = new Random();
            var room = _map.Rooms[rnd.Next(1, _map.Rooms.Count)];
            _levelBoss.X = room.Center.X - 1;
            _levelBoss.Y = room.Center.Y;
            _map.AddMonster(_levelBoss);
        }

        private void PlaceItems()
        {
            foreach (var room in _map.Rooms)
            {
                // Each room has a 70% chance of having monsters
                if (Dice.Roll("1D20") < 5)
                {
                    // Generate between 1 and 4 monsters
                    var numberOfItems = Dice.Roll("1D2");
                    for (int i = 0; i < numberOfItems; i++)
                    {
                        // Find a random walkable location in the room to place the monster
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        // It's possible that the room doesn't have space to place a monster
                        // In that case skip creating the monster
                        if (randomRoomLocation != null)
                        {
                            // Temporarily hard code this monster to be created at level 1
                            var option = Dice.Roll("1D2");
                            Potion potion;

                            switch (option)
                            {
                                case 1:
                                    potion = new Potion("Health", Dice.Roll("1D3"));
                                    potion.X = randomRoomLocation.X;
                                    potion.Y = randomRoomLocation.Y;
                                    _map.Items.Add(potion);
                                    break;
                                case 2:
                                    potion = new Potion("Health", Dice.Roll("1D6"));
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

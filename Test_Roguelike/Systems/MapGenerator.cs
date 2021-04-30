using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Core;
using Test_Roguelike.Core.Monsters;

namespace Test_Roguelike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly List<int[]> coordinates;
        private readonly List<int> doorsX;
        private readonly List<int> doorsY;

        private readonly DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        // as well as the sizes and maximum number of rooms
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
            coordinates = new List<int[]>();
            
            coordinates.Add(new int[4] { 34, 14, 6, 6 });
            coordinates.Add(new int[4] { 34, 10, 6, 20 });
            coordinates.Add(new int[4] { 34, 10, 6, 30 });
            coordinates.Add(new int[4] { 34, 12, 6, 40 });

            coordinates.Add(new int[4] { 40, 14, 40, 6 });
            coordinates.Add(new int[4] { 40, 32, 40, 20 });

            coordinates.Add(new int[4] { 34, 14, 80, 6 });
            coordinates.Add(new int[4] { 34, 10, 80, 20 });
            coordinates.Add(new int[4] { 34, 10, 80, 30 });
            coordinates.Add(new int[4] { 34, 12, 80, 40 });


            doorsX = new List<int>();
            doorsY = new List<int>();
            doorsX.Add(14); doorsY.Add(20);
            doorsX.Add(14); doorsY.Add(30);
            doorsX.Add(14); doorsY.Add(40);
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

            return _map;
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
                if (doorsX.Contains(cell.X) && doorsY.Contains(cell.Y) && _map.GetDoor(cell.X, cell.Y) == null)
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


        /*private void PlaceMonsters()
        {
            foreach (Cell cell in _map.GetAllCells())
            {
                Random rnd = new Random();
                int r = rnd.Next(1, 10);
                if (r < 7)
                {
                    // Generate between 1 and 4 monsters
                    var numberOfMonsters = r;
                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        // Find a random walkable location in the room to place the monster
                        Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                        // It's possible that the room doesn't have space to place a monster
                        // In that case skip creating the monster
                        if (randomRoomLocation != null)
                        {
                            // Temporarily hard code this monster to be created at level 1
                            var monster = Ghost.Create(1);
                            monster.X = randomRoomLocation.X;
                            monster.Y = randomRoomLocation.Y;
                            _map.AddMonster(monster);
                        }
                    }
                }
            }
        }*/
    }

}

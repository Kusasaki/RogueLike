using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RogueSharp.Random;
using Test_Roguelike.Core;
using Test_Roguelike.Core.Items;
using Test_Roguelike.Systems;

namespace Test_Roguelike
{
    public static class Game
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 150;
        private static readonly int _screenHeight = 80;
        public static RLRootConsole _rootConsole { get; private set; }

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 120;
        private static readonly int _mapHeight = 60;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 150;
        private static readonly int _messageHeight = 15;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 30;
        private static readonly int _statHeight = 65;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth =  120;
        private static readonly int _inventoryHeight = 5;
        private static RLConsole _inventoryConsole;
        
        public static DungeonMap DungeonMap { get; private set; }
        public static int _mapLevel = 1;

        public static Player Player { get; set; }

        private static bool _renderRequired = true;

        public static CommandSystem CommandSystem { get; private set; }

        public static MessageLog MessageLog { get; private set; }
        public static InventoryLog InventoryLog { get; private set; }

        public static IRandom Random { get; private set; }

        public static SchedulingSystem SchedulingSystem { get; private set; }

        public static void Main()
        {
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            // The title will appear at the top of the console window 
            // also include the seed used to generate the level
            string consoleTitle = $"RoguePépéMémé - Level 1 ";

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";

            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            
            // Create a new MessageLog and print the random seed used to generate the level
            MessageLog = new MessageLog();
            MessageLog.Add("The rogue arrives on level 1");

            InventoryLog = new InventoryLog();

            SchedulingSystem = new SchedulingSystem();

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, _mapLevel);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            CommandSystem = new CommandSystem();

            // Set background color and text for each console so that we can verify they are in the correct positions
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);

            _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.Primary);
            _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.Primary);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;

            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;

            // Begin RLNET's game loop
            _rootConsole.Run();
        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (Game.Player.IsDead)
            {
                Player = null;
                MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 1);
                DungeonMap = mapGenerator.CreateMap();
                MessageLog = new MessageLog();
                CommandSystem = new CommandSystem();
                _rootConsole.Title = $"RoguePépéMémé - Level 1";
                didPlayerAct = true;
            }
            else if (Player.Inventory.OfType<Machine>().Any())
            {
                MessageLog.Add(" ");
                MessageLog.Add("Vous avez detruit la machine de la mort qui TUE !");
                MessageLog.Add("Le regne des fantomes trop serieux est termine");
                MessageLog.Add("Place a la fête et a la boutade !");
                MessageLog.Add("Vous avez gagne !!! BRAVA");
                _renderRequired = true;
            }
            else if (CommandSystem.IsPlayerTurn)
            {
                if (Game.Player.IsAttacking)
                {
                    _renderRequired = true;
                }

                if (keyPress != null)
                {
                    if (Player.IsAttacking)
                    {
                        if (keyPress.Key == RLKey.Number1)
                        {
                            Game.CommandSystem.Attack(Game.Player, Game.Player.Target, 1);
                            didPlayerAct = true;
                        }
                        else if (keyPress.Key == RLKey.Number2)
                        {
                            Game.CommandSystem.Attack(Game.Player, Game.Player.Target, 2);
                            didPlayerAct = true;
                        }
                    }
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                    else if (keyPress.Key == RLKey.Period)
                    {
                        if (DungeonMap.CanMoveDownToNextLevel())
                        {
                            
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, ++_mapLevel);
                            DungeonMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _rootConsole.Title = $"UChronia - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                }

                if (didPlayerAct)
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
                
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Don't bother redrawing all of the consoles if nothing has changed.
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                

                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);

                // New code after Player.Draw()
                Player.DrawStats(_statConsole);

                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
                

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();
                MessageLog.Draw(_messageConsole);
                InventoryLog.Draw(_inventoryConsole);

                _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.Primary);
                _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, RLColor.Gray);
                
                _renderRequired = false;
            }
        }
    }
}

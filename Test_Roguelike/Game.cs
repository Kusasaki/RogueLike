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
        private static readonly int _screenHeight = 90;
        public static RLRootConsole _rootConsole { get; private set; }

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 120;
        private static readonly int _mapHeight = 60;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 120;
        private static readonly int _messageHeight = 15;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 30;
        private static readonly int _statHeight = 90;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth =  120;
        private static readonly int _inventoryHeight = 15;
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
            string consoleTitle = $"UChronia - Level 1 ";

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
            MessageLog.Add("Bienvenue dans la TOUR DES FANTOMES SURPUISSANTS, elue meilleure chambre d'hote 2015 !");

            InventoryLog = new InventoryLog();

            SchedulingSystem = new SchedulingSystem();

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, _mapLevel);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();

            CommandSystem = new CommandSystem();

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

            //Gestion de l'action du joueur 
            if (Game.Player.IsDead) //Si le joueur meurt relance une nouvelle partie
            {
                Player = null;
                MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 1);
                DungeonMap = mapGenerator.CreateMap();
                MessageLog = new MessageLog();
                CommandSystem = new CommandSystem();
                _rootConsole.Title = $"UChronia - Level 1";
                didPlayerAct = true;
            }
            else if (Player.Inventory.OfType<Machine>().Any()) //Si le joueur a detruit la machine mesage de fin de partie
            {
                MessageLog.Add(" ");
                MessageLog.Add("Vous avez detruit la machine de la mort qui TUE !");
                MessageLog.Add("Le regne des fantomes trop serieux est termine");
                MessageLog.Add("Place a la fete et a la boutade !");
                MessageLog.Add("Vous avez gagne !!! BRAVA");
                _renderRequired = true;
            }
            else if (CommandSystem.IsPlayerTurn)
            {
                if (Player.IsAttacking) //Affichage de l'attaque
                {
                    _renderRequired = true;
                }

                if (keyPress != null)
                {
                    if (Player.IsAttacking) //Gestion de l'interface d'attaque
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
                    if (keyPress.Key == RLKey.Up)//Gestion du mouvement
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
                    else if (keyPress.Key == RLKey.Period) //Gestion du changement de niveua
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

                if (didPlayerAct) //Au tour des monstres !
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
                
            }
            else // Tour des monstres !
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Si les consoles ont changé, on update
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                
                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);

                Player.DrawStats(_statConsole);

                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);
                
                _rootConsole.Draw();

                MessageLog.Draw(_messageConsole);
                InventoryLog.Draw(_inventoryConsole);

                _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Swatch.ComplementDarker);
                _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.Primary2Darker);
                _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.PrimaryLighter);
                _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.Primary2Darker);

                _renderRequired = false;
            }
        }
    }
}

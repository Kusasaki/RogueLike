using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Core;

namespace Test_Roguelike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }

        // Generate a new map that is a simple open floor with walls around the outside
        public DungeonMap CreateMap()
        {
            // Initialize every cell in the map by
            // setting walkable, transparency, and explored to true
            _map.Initialize(_width, _height);
            foreach (Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            
            //construction du niveau : murs
            for (int i = 0; i < _width - 1; i = i + 40)
            {
                foreach (Cell cell in _map.GetCellsInColumns(i, _width - 1))
                {
                    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
                }
            }
            foreach (Cell cell in _map.GetCellsInRows(20,_height-1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            foreach (Cell cell in _map.GetCellsInRows(30, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            foreach (Cell cell in _map.GetCellsInRows(40, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            //construction du niveau : portes à factoriser
            foreach (Cell cell in _map.GetCellsInColumns(22))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(23))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(24))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(59))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(60))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(61))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(97))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(98))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(99))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInSquare(40,35,4))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInSquare(80, 35, 4))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInSquare(60, 40, 19))
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetBorderCellsInSquare(60, 35, 4))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            //mer en haut
            for (int i = 0; i < 7; i++)
            {
                foreach (Cell cell in _map.GetCellsInRows(i, _height - 1))
                {
                    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
                }
            }
            //mer en bas
            for (int i = _height - 7; i < _height - 1; i++)
            {
                foreach (Cell cell in _map.GetCellsInRows(i, _height - 1))
                {
                    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
                }
            }
            //mer à gauche
            for (int i = 0; i < 7; i++)
            {
                foreach (Cell cell in _map.GetCellsInColumns(i, _width - 1))
                {
                    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
                }
            }
            //mer à droite
            for (int i = _width - 7; i < _width - 1; i++)
            {
                foreach (Cell cell in _map.GetCellsInColumns(i, _width - 1))
                {
                    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
                }
            }

            return _map;
        }
    }

}

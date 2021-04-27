﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace Test_Roguelike.Core
{
    // Our custom DungeonMap class extends the base RogueSharp Map class
    public class DungeonMap : Map
    {
        // The Draw method will be called each time the map is updated
        // It will render all of the symbols/colors for each cell to the map sub console
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
        {
            // When we haven't explored a cell yet, we don't want to draw anything
            if (!cell.IsExplored)
            {
                return;
            }

            // Map niveau
            if (IsInFov(cell.X, cell.Y))
            {
                // Choose the symbol to draw based on if the cell is walkable or not
                // '.' for floor and '#' for walls
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '~');
                }
            }
            // When a cell is outside of the field of view draw it with darker colors
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.Floor, '.');
                }
                else
                { 
                    console.Set(cell.X, cell.Y, RLColor.Cyan, Colors.WallBackground, '~');
                }
                if ((!cell.IsWalkable) && (((cell.X == 6)&&(cell.Y>6)&&(cell.Y<Height-7))||((cell.Y==6)&&(cell.X>5)&&(cell.X<Width-7))))
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.Floor, '#');
                }
                if ((!cell.IsWalkable) && (((cell.X == Width-7) && (cell.Y >5) && (cell.Y < Height - 7)) || ((cell.Y == Height-7) && (cell.X >5) && (cell.X < Width - 6))))
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.Floor, '#');
                }
            }
        }
    }
}
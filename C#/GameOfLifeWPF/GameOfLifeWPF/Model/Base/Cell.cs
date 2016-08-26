using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLifeWPF.Model.Base
{
    public abstract class Cell : Button
    {
        public bool IsAlive { get; set; }
        public Point Coordinates { get; set; }
        public List<Cell> Neighbours { get; set; }
        public int Age { get; protected set; }

        public Cell()
        {
            Background = Brushes.Bisque;
            Age = 0;
        }

        public Cell(Cell cell)
        {
            if (cell != this && cell != null)
            {
                CellInitialization(cell);
            }
        }

        private void CellInitialization(Cell cell)
        {
            Coordinates = cell.Coordinates;
            IsAlive = cell.IsAlive;
            Neighbours = cell.Neighbours;
            Name = cell.Name;
            Width = cell.Width;
            Height = Height;
            Click += Game.Cell_Click;
            ToolTip = cell.ToolTip;
            Age = cell.Age;
        }

        public virtual bool DetermineIfCellSurvived()
        {
            int aliveNeighboursCount = Neighbours.Where((neighbour) => neighbour.IsAlive).Count();
            bool survived = aliveNeighboursCount == 3 || (IsAlive && aliveNeighboursCount == 2);
            Age = survived ? Age+1 : 0;
            return survived;
        }

        public virtual void EmitCellState() //TODO To be removed and better designed
        {
        }
    }
}

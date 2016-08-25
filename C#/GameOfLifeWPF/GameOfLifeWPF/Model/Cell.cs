using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLifeWPF.Model
{
    public class Cell : Button
    {
        public bool IsAlive { get; set; }
        public Point Coordinates { get; set; }
        public List<Cell> Neighbours { get; set; }

        public void Survive()
        {
            while (true)
            {
                PerformStep();
            }
        }

        public void PerformStep()
        {
            IsAlive = DetermineIfCellSurvived();
            EmitCellState();
            Thread.Sleep(100);
        }

        private bool DetermineIfCellSurvived()
        {
            int aliveNeighboursCount = Neighbours.Where((neighbour) => neighbour.IsAlive).Count();
            return aliveNeighboursCount == 3 || (IsAlive && aliveNeighboursCount == 2);
        }

        public void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = IsAlive ? Brushes.Purple : Brushes.Bisque);
        }
    }
}

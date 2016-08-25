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
        private List<Cell> _neighbours;

        public Cell(List<Cell> neighbours)
        {
            if(neighbours.Count == 7)   // 7 - becouse every cell should have only 7 neighbours
            {
                _neighbours = neighbours;
            }
            else
            {
                throw new Exception("Inconsistent playground.");
            }
        }

        public void Survive(ref MainWindow win)
        {
            while (true)
            {
                IsAlive = DetermineIfCellSurvived();
                Thread.Sleep(100);
                EmitCellState();
            }
        }

        private bool DetermineIfCellSurvived()
        {
            throw new NotImplementedException();
        }

        public void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = IsAlive ? Brushes.Purple : Brushes.Bisque);
        }
    }
}

using GameOfLifeWPF.Model.Base;
using System.Linq;
using System.Windows.Media;

namespace GameOfLifeWPF.Model
{
    class VirusCell : Cell
    {
        public VirusCell(Cell cell) : base(cell)
        {
            if(!(cell is VirusCell) && cell != this && cell != null)
            {
                IsAlive = true;
            }
        }

        public override bool DetermineIfCellSurvived()
        {
            int aliveNeighboursCount = Neighbours.Where((neighbour) => neighbour.IsAlive).Count();
            return aliveNeighboursCount == 3 || (IsAlive && aliveNeighboursCount == 2);
        }

        public override void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = Brushes.Green);
        }
    }
}

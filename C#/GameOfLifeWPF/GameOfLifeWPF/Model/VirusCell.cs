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
            bool survived = Age < 10;
            Age = survived ? Age + 1 : 0;
            return survived;
        }

        public override void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = Brushes.ForestGreen);
        }
    }
}

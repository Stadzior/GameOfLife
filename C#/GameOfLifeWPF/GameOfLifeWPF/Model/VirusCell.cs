using GameOfLifeWPF.Model.Base;
using System.Windows.Media;

namespace GameOfLifeWPF.Model
{
    class VirusCell : Cell
    {
        private bool _isAlive;
        public override bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                _isAlive = value;
                Dispatcher.Invoke(() => Background = value ? Brushes.ForestGreen : Brushes.Bisque);
            }
        }

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

    }
}

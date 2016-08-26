using GameOfLifeWPF.Model.Base;
using System.Windows.Media;

namespace GameOfLifeWPF.Model
{
    public class RegularCell : Cell
    {
        public RegularCell() : base()
        {
        }
        public RegularCell(Cell cell) : base(cell)
        {
        }

        public override void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = IsAlive ? Brushes.Purple : Brushes.Bisque);
        }
    }
}

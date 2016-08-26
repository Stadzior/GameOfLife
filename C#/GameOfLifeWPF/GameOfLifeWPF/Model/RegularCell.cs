using GameOfLifeWPF.Model.Base;
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
    public class RegularCell : Cell
    {
        public RegularCell() : base()
        {
        }
        public RegularCell(Cell cell) : base(cell)
        {
        }

        public override bool DetermineIfCellSurvived()
        {
            return base.DetermineIfCellSurvived() && !Neighbours.Any((neighbour)=>neighbour is VirusCell);
        }
        public override void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = IsAlive ? Brushes.Purple : Brushes.Bisque);
        }
    }
}

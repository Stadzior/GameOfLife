using GameOfLifeWPF.Model.Base;
using System.Collections.Generic;
using System.Windows;
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

        public RegularCell(Point coordinates, string name, double width, double height, object toolTip, bool isAlive = true, int age = 0, RoutedEventHandler cellClickHandler = null) :
            base(coordinates, name, width, height, toolTip, isAlive, age, cellClickHandler)
        {
        }

        public override void EmitCellState()
        {
            Dispatcher.Invoke(() => Background = IsAlive ? Brushes.Purple : Brushes.Bisque);
        }
    }
}

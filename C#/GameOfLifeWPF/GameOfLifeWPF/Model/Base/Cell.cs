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

        public Cell(Cell cell): this(cell.Coordinates, cell.Name, cell.Width, cell.Height, cell.ToolTip, cell.IsAlive, cell.Age)
        {
            Neighbours = cell.Neighbours; //TODO Fix OnClick Event Handler
        }

        public Cell(Point coordinates, string name,double width, double height,object toolTip, bool isAlive = true, int age = 0, RoutedEventHandler cellClickHandler = null) : this()
        {
            Coordinates = coordinates;
            IsAlive = isAlive;
            Name = name;
            Width = width;
            Height = height;
            ToolTip = toolTip;
            Age = age;
            if(cellClickHandler != null) Click += cellClickHandler;
        }

        public virtual bool DetermineIfCellSurvived()
        {
            int aliveNeighboursCount = Neighbours.Where((neighbour) => neighbour.IsAlive && !(neighbour is VirusCell)).Count();
            bool survived = aliveNeighboursCount == 3 || (IsAlive && aliveNeighboursCount == 2);
            Age = survived ? Age+1 : 0;
            return survived;
        }

        public virtual void EmitCellState() //TODO To be removed and better designed
        {
        }
    }
}

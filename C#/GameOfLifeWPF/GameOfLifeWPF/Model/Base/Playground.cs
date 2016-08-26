using GameOfLifeWPF.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLifeWPF.Model
{
    public class Playground
    {
        public MainWindow MainWindow { private get; set; }
        public Panel OuterPanel { private get; set; }
        public Panel InnerPanel { private get; set; }
        public List<Cell> Cells { get; private set; }

        public Playground(RoutedEventHandler cellClickHandler, int playgroundSize, int cellSize)
        {
            PopulatePlayground(cellClickHandler, playgroundSize, cellSize);
            LinkNeighbours();
        }

        private void PopulatePlayground(RoutedEventHandler cellClickHandler, int playgroundSize, int cellSize)
        {
            Cells = new List<Cell>();
            for (double i = 0; i < playgroundSize; i++)
            {
                for (double j = 0; j < playgroundSize; j++)
                {
                    string name = "cell_" + i.ToString() + "_" + j.ToString();
                    string toolTip = i.ToString() + "," + j.ToString();
                    Cell cell = new RegularCell(new Point(i, j), name, cellSize, cellSize, toolTip, cellClickHandler: cellClickHandler);
                    Cells.Add(cell);
                }
            }
        }

        private void LinkNeighbours()
        {
            foreach (RegularCell cell in Cells)
            {
                cell.Neighbours = Cells.Where((x) => (
                (x.Coordinates.X > cell.Coordinates.X - 2 && x.Coordinates.X < cell.Coordinates.X + 2) &&
                (x.Coordinates.Y > cell.Coordinates.Y - 2 && x.Coordinates.Y < cell.Coordinates.Y + 2) &&
                x != cell)).ToList();
            }
        }

        public void RefreshUI(bool gameIsRunning)
        {
            OuterPanel.Background = gameIsRunning ? Brushes.LightSeaGreen : Brushes.DarkRed;
            MainWindow.RefreshToolBar();
        }

    }
}

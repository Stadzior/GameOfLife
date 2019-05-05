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
        public Panel PanelContainer { get; set; }
        public IList<Cell> Cells { get; set; }

        public Playground(RoutedEventHandler cellClickHandler, int playgroundSize, int cellSize)
        {
            Cells = new List<Cell>();
            PopulatePlayground(cellClickHandler, playgroundSize, cellSize);
            LinkNeighbours();
        }

        private void PopulatePlayground(RoutedEventHandler cellClickHandler, int playgroundSize, int cellSize)
        {
            for (double i = 0; i < playgroundSize; i++)
            {
                for (double j = 0; j < playgroundSize; j++)
                {
                    string name = "cell_" + i.ToString() + "_" + j.ToString();
                    string toolTip = i.ToString() + "," + j.ToString();
                    Cell cell = new RegularCell(new Point(i, j), name, cellSize, cellSize, toolTip,isAlive: false, cellClickHandler: cellClickHandler);
                    Cells.Add(cell);
                }
            }
        }

        private void LinkNeighbours()
        {
            foreach (RegularCell cell in Cells)
            {
                cell.Neighbours = Cells.Where((x) => (
                x.Coordinates.X > cell.Coordinates.X - 2 && x.Coordinates.X < cell.Coordinates.X + 2 &&
                x.Coordinates.Y > cell.Coordinates.Y - 2 && x.Coordinates.Y < cell.Coordinates.Y + 2 &&
                x != cell)).ToList();
            }
        }

        internal void ReplaceCell<T>(Cell sourceCell, T targetCell) where T : Cell
        {
            int index = Cells.IndexOf(sourceCell);
            Cells.RemoveAt(index);
            Cells.Insert(index, targetCell);

            ReplaceInNeighbourhood(sourceCell, targetCell);
        }

        private static void ReplaceInNeighbourhood<T>(Cell sourceCell, T targetCell) where T : Cell
        {
            foreach (Cell neighbour in targetCell.Neighbours)
            {
                neighbour.Neighbours.Remove(sourceCell);
                neighbour.Neighbours.Add(targetCell);
            }
        }
    }
}

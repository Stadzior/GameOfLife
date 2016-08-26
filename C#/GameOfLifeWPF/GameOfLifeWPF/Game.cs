using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameOfLifeWPF
{
    public static class Game
    {
        public static bool GameIsRunning { get; set; } = false;
        private static List<Cell> _cells;
        private static Thread _gameThread;

        public static List<Cell> InitializePlayground(int playgroundSize, int cellSize)
        {
            CreatePlayground(playgroundSize, cellSize);
            BondCellsWithNeighbours();
            return _cells;
        }

        private static void CreatePlayground(int playgroundSize, int cellSize)
        {
            _cells = new List<Cell>();
            for (double i = 0; i < playgroundSize; i++)
            {
                for (double j = 0; j < playgroundSize; j++)
                {
                    Cell cell = new RegularCell();
                    cell.Width = cellSize;
                    cell.Height = cellSize;
                    cell.Click += Cell_Click;
                    cell.Coordinates = new Point(i, j);
                    cell.ToolTip = i.ToString() + "," + j.ToString();
                    cell.Name = "cell_" + i.ToString() + "_" + j.ToString();
                    _cells.Add(cell);
                }
            }
        }

        private static void BondCellsWithNeighbours()
        {
            foreach (RegularCell cell in _cells)
            {
                cell.Neighbours = _cells.Where((x) => (
                (x.Coordinates.X > cell.Coordinates.X - 2 && x.Coordinates.X < cell.Coordinates.X + 2) &&
                (x.Coordinates.Y > cell.Coordinates.Y - 2 && x.Coordinates.Y < cell.Coordinates.Y + 2) &&
                x != cell)).ToList();
            }
        }

        public static void PerformStep()
        {
            Dictionary<Cell, bool> futureStates = new Dictionary<Cell, bool>();
            for (int i = 0; i < _cells.Count; i++)
            {
                Cell cell = _cells[i];
                if (cell is VirusCell && !cell.IsAlive)
                {
                    Application.Current.Dispatcher.Invoke(() => MutateCellTo<RegularCell>(cell));
                }
                else
                {
                    if(!(cell is VirusCell) && cell.IsAlive && cell.Neighbours.Any((neighbour)=>neighbour is VirusCell))
                    {
                        Application.Current.Dispatcher.Invoke(() => MutateCellTo<VirusCell>(cell));
                    }
                }
                futureStates.Add(cell, cell.DetermineIfCellSurvived());
            }

            foreach (KeyValuePair<Cell, bool> pair in futureStates)
            {
                pair.Key.IsAlive = pair.Value;
                pair.Key.EmitCellState();
            }
        }

        public static void ClearPlayground()
        {
            foreach (Cell cell in _cells)
            {
                cell.IsAlive = false;
                cell.EmitCellState();
            }
        }

        public static void RandomizePlayground()
        {
            foreach (Cell cell in _cells)
            {
                Thread.Sleep(1);
                cell.IsAlive = new Random((int)DateTime.Now.Ticks).Next(2) == 1;
                cell.EmitCellState();
            }
        }

        public static void PauseGame()
        {
            GameIsRunning = false;
            _gameThread.Abort();
        }

        public static void ResumeGame()
        {
            GameIsRunning = true;
            _gameThread = new Thread(delegate () {
                while (GameIsRunning)
                {
                    PerformStep();
                    Thread.Sleep(10);
                }
            });
            _gameThread.SetApartmentState(ApartmentState.STA);
            _gameThread.Start();
        }

        public static void Cell_Click(object sender, RoutedEventArgs e)
        {
            Cell cell = ((Cell)sender);
            if (cell.IsAlive)
            {
                if (cell is VirusCell)
                {
                    cell.IsAlive = false;
                    cell = MutateCellTo<RegularCell>(cell);
                }
                else
                {
                    cell = MutateCellTo<VirusCell>(cell);
                }
            }
            else
            {
                cell.IsAlive = true;
                cell.EmitCellState();
            }
        }

        private static T MutateCellTo<T>(Cell cell) where T : Cell
        {
            T mutant = (T)Activator.CreateInstance(typeof(T), new object[] { cell });
            _cells[_cells.FindIndex((x) => x.Coordinates == cell.Coordinates)] = mutant;

            WrapPanel wrapPanelPlayground = (WrapPanel)cell.Parent;
            int cellIndexWithinWrapPanel = wrapPanelPlayground.Children.IndexOf(cell);

            wrapPanelPlayground.Children.RemoveAt(cellIndexWithinWrapPanel);
            wrapPanelPlayground.Children.Insert(cellIndexWithinWrapPanel, mutant);

            foreach (Cell neighbour in mutant.Neighbours)
            {
                neighbour.Neighbours.Remove(cell);
                neighbour.Neighbours.Add(mutant);
            }
            mutant.EmitCellState();
            return mutant;
        }
    }
}

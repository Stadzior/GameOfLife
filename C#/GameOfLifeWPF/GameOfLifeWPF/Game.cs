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
using System.Windows.Media;
using System.Windows.Threading;

namespace GameOfLifeWPF
{
    public class Game
    {
        public bool GameIsRunning { get; private set; } = false;
        private Thread _gameThread;
        public Playground LinkedPlayground { get; private set; }

        public List<Cell> InitializePlayground(int playgroundSize, int cellSize)
        {
            LinkedPlayground = new Playground(Cell_Click,playgroundSize, cellSize);
            return LinkedPlayground.Cells;
        }

        public void PerformStep()
        {
            Dictionary<Cell, bool> futureStates = new Dictionary<Cell, bool>();
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = LinkedPlayground.Cells[i];
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

        public void Reset()
        {
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = LinkedPlayground.Cells[i];
                cell.IsAlive = false;
                if(!(cell is RegularCell)) MutateCellTo<RegularCell>(cell);
                cell.EmitCellState();
            }
        }

        public void Randomize()
        {
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = LinkedPlayground.Cells[i];
                Thread.Sleep(1);
                cell.IsAlive = new Random((int)DateTime.Now.Ticks).Next(2) == 1;
                if (!(cell is RegularCell)) MutateCellTo<RegularCell>(cell);
                cell.EmitCellState();
            }
        }

        public void Pause()
        {
            GameIsRunning = false;
            _gameThread.Abort();
            LinkedPlayground.RefreshUI(GameIsRunning);
        }

        public void Resume()
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
            LinkedPlayground.RefreshUI(GameIsRunning);
        }

        public void Cell_Click(object sender, RoutedEventArgs e)
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

        private T MutateCellTo<T>(Cell cell) where T : Cell
        {
            T mutant = (T)Activator.CreateInstance(typeof(T), new object[] { cell });
            mutant.Click += Cell_Click;

            LinkedPlayground.Cells[LinkedPlayground.Cells.FindIndex((x) => x.Coordinates == cell.Coordinates)] = mutant;

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

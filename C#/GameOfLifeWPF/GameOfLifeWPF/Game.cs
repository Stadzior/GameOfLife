using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.Base;
using GameOfLifeWPF.View;
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
        private bool _gameIsRunning;
        private IRefreshable _view;
        public bool GameIsRunning
        {
            get { return _gameIsRunning; }
            set
            {
                _gameIsRunning = value;
                if(_view != null) _view.Refresh();
            }
        }
        private Thread _gameThread;
        public Playground LinkedPlayground { get; private set; }

        public Game(CellCollection cells, int playgroundSize, IRefreshable view = null)
        {
            //TODO Adjusting to screen login.
            int cellSize = 10;
            LinkedPlayground = new Playground(cells, Cell_Click, playgroundSize, cellSize);
            if (view != null) _view = view;
        }

        public void PerformStep()
        {
            Dictionary<Cell, bool> futureStates = new Dictionary<Cell, bool>();
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = (Cell)LinkedPlayground.Cells[i];
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
            }
        }

        public void Reset()
        {
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = (Cell)LinkedPlayground.Cells[i];
                if(!(cell is RegularCell)) MutateCellTo<RegularCell>(cell);
                cell.IsAlive = false;
            }
        }

        public void Randomize()
        {
            for (int i = 0; i < LinkedPlayground.Cells.Count; i++)
            {
                Cell cell = (Cell)LinkedPlayground.Cells[i];
                Thread.Sleep(1);
                if (!(cell is RegularCell)) MutateCellTo<RegularCell>(cell);
                cell.IsAlive = new Random((int)DateTime.Now.Ticks).Next(2) == 1;
            }
        }

        public void Pause()
        {
            GameIsRunning = false;
            _gameThread.Abort();
        }

        public void Resume()
        {
            GameIsRunning = true;
            _gameThread = new Thread(delegate () {
                while (GameIsRunning)
                {
                    PerformStep();
                }
            });
            _gameThread.SetApartmentState(ApartmentState.STA);
            _gameThread.Start();
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
            }
        }

        private T MutateCellTo<T>(Cell cell) where T : Cell
        {
            T mutant = (T)Activator.CreateInstance(typeof(T), new object[] { cell });
            mutant.Click += Cell_Click;

            LinkedPlayground.ReplaceCell(cell, mutant);
            
            return mutant;
        }
    }
}

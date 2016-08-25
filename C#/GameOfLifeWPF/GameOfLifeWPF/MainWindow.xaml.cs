using GameOfLifeWPF.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Linq;

namespace GameOfLifeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _gameIsRunning;
        private List<Thread> _survivalThreads;
        private List<Cell> _cells;
        private Thread _gameThread;
        public MainWindow()
        {
            _gameIsRunning = false;
            _survivalThreads = new List<Thread>();
            InitializeComponent();
            InitializePlayground();
        }
        private void InitializePlayground()
        {
            int playgroundSize = 50;
            int cellSize = 10;
            Background = Brushes.DarkRed;
            wrapPanelPlayground.Height = cellSize * playgroundSize;
            wrapPanelPlayground.Width = cellSize * playgroundSize;
            _cells = new List<Cell>();
            CreatePlayground(playgroundSize,cellSize);
            BondCellsWithNeighbours();
        }

        private void CreatePlayground(int playgroundSize,int cellSize)
        {
            for (double i = 0; i < playgroundSize; i ++)
            {
                for (double j = 0; j < playgroundSize; j ++)
                {
                    Cell cell = new Cell();
                    cell.Background = Brushes.Bisque;
                    cell.Width = cellSize;
                    cell.Height = cellSize;
                    cell.Click += Cell_Click;
                    cell.Coordinates = new Point(i, j);
                    cell.ToolTip = i.ToString() + "," + j.ToString();
                    _cells.Add(cell);
                    wrapPanelPlayground.Children.Add(cell);
                }
            }
        }

        private void BondCellsWithNeighbours()
        {           
            foreach (Cell cell in _cells)
            {
                cell.Neighbours = _cells.Where((x) => (
                (x.Coordinates.X > cell.Coordinates.X - 2 && x.Coordinates.X < cell.Coordinates.X + 2) &&
                (x.Coordinates.Y > cell.Coordinates.Y - 2 && x.Coordinates.Y < cell.Coordinates.Y + 2) &&
                x != cell)).ToList();
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Cell cell = ((Cell)sender);
            if (cell.IsAlive)
            {
                if(cell is ZombieCell)
                {
                    cell.IsAlive = false;
                }
                else
                {
                    TransformToZombie(cell);
                }
            }
            else
            {
                cell.IsAlive = true;
            }

            cell.EmitCellState();
        }

        private void TransformToZombie(Cell cell)
        {
            throw new NotImplementedException();
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.RightShift:
                    {
                        if (_gameIsRunning)
                        {
                            PauseGame();
                        }
                        else
                        {
                            
                            _gameThread = new Thread(()=>ResumeGame());
                            _gameThread.Start();
                            Background = Brushes.ForestGreen;
                        }
                        break;
                    }

                case Key.R:
                    RandomizePlayground();
                    break;

                case Key.C:
                    ClearPlayground();
                    break;

                case Key.S:
                    PerformStep();
                    break;
            }

        }

        private void PerformStep()
        {
            Dictionary<Cell, bool> futureStates = new Dictionary<Cell, bool>();
            foreach (Cell cell in _cells)
            {
                futureStates.Add(cell, cell.DetermineIfCellSurvived());
            }
            foreach (KeyValuePair<Cell,bool> pair in futureStates)
            {
                pair.Key.IsAlive = pair.Value;
                pair.Key.EmitCellState();
            }
        }

        private void ClearPlayground()
        {
            foreach (Cell cell in _cells)
            {
                cell.IsAlive = false;
                cell.EmitCellState();
            }
        }

        private void RandomizePlayground()
        {
            foreach (Cell cell in _cells)
            {
                Thread.Sleep(1);
                cell.IsAlive = new Random((int)DateTime.Now.Ticks).Next(2) == 1;
                cell.EmitCellState();
            }
        }

        private void PauseGame()
        {
            _gameIsRunning = false;
            Background = Brushes.DarkRed;
            _gameThread.Abort();
        }

        private void ResumeGame()
        {
            _gameIsRunning = true;
            while (_gameIsRunning)
            {
                PerformStep();
                Thread.Sleep(10);
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PauseGame();
        }
    }
}

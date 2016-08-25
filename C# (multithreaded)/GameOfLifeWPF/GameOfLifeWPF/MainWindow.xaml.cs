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

        public MainWindow()
        {
            _gameIsRunning = false;
            _survivalThreads = new List<Thread>();
            InitializeComponent();
            InitializePlayground();
        }
        private void InitializePlayground()
        {
            int playgroundSize = 10;
            _cells = new List<Cell>();
            CreatePlayground(playgroundSize);
            BondCellsWithNeighbours();
        }

        private void CreatePlayground(int playgroundSize)
        {
            for (double i = 0; i < playgroundSize; i ++)
            {
                for (double j = 0; j < playgroundSize; j ++)
                {
                    Cell cell = new Cell();
                    cell.Background = Brushes.Bisque;
                    cell.Width = wrapPanelPlayground.Width / playgroundSize;
                    cell.Height = wrapPanelPlayground.Height / playgroundSize;
                    cell.Click += ChangeCellState;
                    cell.Coordinates = new Point(i, j);
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

        private void ChangeCellState(object sender, RoutedEventArgs e)
        {
            Cell cell = ((Cell)sender);
            cell.IsAlive = !cell.IsAlive;
            cell.EmitCellState();
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
              if(e.Key == Key.RightShift)
            {
                if (_gameIsRunning)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
                _gameIsRunning = !_gameIsRunning;
            }
        }

        private void PauseGame()
        {
            foreach (Thread thread in _survivalThreads)
            {
                thread.Abort();
            }
            _survivalThreads.Clear();
        }

        private void ResumeGame()
        {
            foreach (Cell cell in wrapPanelPlayground.Children)
            {
                Thread survivalThread = new Thread(() => cell.Survive(ref mainWindow));
                _survivalThreads.Add(survivalThread);
                survivalThread.Start();
            }
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PauseGame();
        }
    }
}

using GameOfLifeWPF.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace GameOfLifeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _gameIsRunning;
        private List<Thread> _survivalThreads;

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
            for (double i = 0; i < wrapPanelPlayground.Width; i+=(wrapPanelPlayground.Width / playgroundSize))
            {
                for (double j = 0; j < wrapPanelPlayground.Height; j+=(wrapPanelPlayground.Height / playgroundSize))
                {
                    Cell cell = new Cell(GatherNeighbours(i,j));
                    cell.Background = Brushes.Bisque;
                    cell.Width = wrapPanelPlayground.Width / playgroundSize;
                    cell.Height = wrapPanelPlayground.Height / playgroundSize;
                    cell.Click += ChangeCellState;
                    wrapPanelPlayground.Children.Add(cell);
                }
            }
        }

        private object GatherNeighbours(double i, double j)
        {
            throw new NotImplementedException();
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

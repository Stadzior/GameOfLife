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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    BaseCell cell = new RegularCell();
                    cell.Background = Brushes.Bisque;
                    cell.Width = wrapPanelPlayground.Width / playgroundSize;
                    cell.Height = wrapPanelPlayground.Height / playgroundSize;
                    
                    cell.Click += ResurrectCell;
                    wrapPanelPlayground.Children.Add(cell);
                }
            }
        }

        private void ResurrectCell(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Background = Brushes.Purple;
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
            foreach (BaseCell cell in wrapPanelPlayground.Children)
            {
                Thread survivalThread = new Thread(() => cell.Survive());
                _survivalThreads.Add(survivalThread);
                survivalThread.Start();
            }
        }
    }
}

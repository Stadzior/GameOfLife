using GameOfLifeWPF.Model;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Linq;
using GameOfLifeWPF.Model.Base;

namespace GameOfLifeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            int cellSize = 10;
            int playgroundSize = 50;
            Background = Brushes.DarkRed;
            wrapPanelPlayground.Height = cellSize * playgroundSize;
            wrapPanelPlayground.Width = cellSize * playgroundSize;
            List<Cell> cells = Game.InitializePlayground(playgroundSize, cellSize);
            foreach (Cell cell in cells)
            {
                wrapPanelPlayground.Children.Add(cell);
            }
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.RightShift:
                    {
                        if (Game.GameIsRunning)
                        {
                            Game.PauseGame();
                            Background = Brushes.DarkRed;
                        }
                        else
                        {
                            Game.ResumeGame();
                            Background = Brushes.ForestGreen;
                        }
                        break;
                    }

                case Key.R:
                    Game.RandomizePlayground();
                    break;

                case Key.C:
                    Game.ClearPlayground();
                    break;

                case Key.S:
                    Game.PerformStep();
                    break;
            }

        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Game.PauseGame();
            Background = Brushes.DarkRed;
        }
    }
}

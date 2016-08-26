﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GameOfLifeWPF.Model.Base;
using System.Windows.Controls.Primitives;

namespace GameOfLifeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;
        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            int cellSize = 10;
            int playgroundSize = 50;
            stackPanelMain.Background = Brushes.DarkRed;

            List<Cell> cells = _game.InitializePlayground(playgroundSize, cellSize);
            _game.LinkedPlayground.OuterPanel = stackPanelMain;
            _game.LinkedPlayground.MainWindow = mainWindow;

            foreach (Cell cell in cells)
            {
                wrapPanelPlayground.Children.Add(cell);
            }
            RefreshToolBar();
            Width = wrapPanelPlayground.Width + 20;
            Height = wrapPanelPlayground.Height + toolBarMain.Height + 40;
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.RightShift:
                    {
                        if (_game.GameIsRunning)
                        {
                            btnPause.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                        else
                        {
                            btnResume.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        }
                        break;
                    }

                case Key.R:
                    btnRandomize.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;

                case Key.C:
                    btnClear.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;

                case Key.S:
                    btnStepForward.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
            }

        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _game.Pause();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (!_game.GameIsRunning) _game.Reset();
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            if (!_game.GameIsRunning) _game.Resume();
        }

        private void btnStepBack_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnStepForward_Click(object sender, RoutedEventArgs e)
        {
            if (!_game.GameIsRunning) _game.PerformStep();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_game.GameIsRunning) _game.Pause();
        }

        private void btnRandomize_Click(object sender, RoutedEventArgs e)
        {
            if (!_game.GameIsRunning) _game.Randomize();
        }

        public void RefreshToolBar()
        {
            btnClear.IsEnabled = !_game.GameIsRunning;
            btnStepBack.IsEnabled = false;
            btnStepForward.IsEnabled = !_game.GameIsRunning;
            btnResume.IsEnabled = !_game.GameIsRunning;
            btnPause.IsEnabled = _game.GameIsRunning;
            btnRandomize.IsEnabled = !_game.GameIsRunning;
        }
    }
}

using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            int cellSize = 10;
            int playgroundSize = 50;
            stackPanelMain.Background = Brushes.DarkRed;
            wrapPanelPlayground.Height = cellSize * playgroundSize;
            wrapPanelPlayground.Width = cellSize * playgroundSize;
            List<Cell> cells = Game.InitializePlayground(playgroundSize, cellSize);
            Game.OuterPanel = stackPanelMain;
            Game.MainWindow = this;
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
                        if (Game.GameIsRunning)
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
            Game.PauseGame();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if (!Game.GameIsRunning) Game.ClearPlayground();
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            if (!Game.GameIsRunning) Game.ResumeGame();
        }

        private void btnStepBack_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnStepForward_Click(object sender, RoutedEventArgs e)
        {
            if (!Game.GameIsRunning) Game.PerformStep();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (Game.GameIsRunning) Game.PauseGame();
        }

        private void btnRandomize_Click(object sender, RoutedEventArgs e)
        {
            if (!Game.GameIsRunning) Game.RandomizePlayground();
        }

        public void RefreshToolBar()
        {
            btnClear.IsEnabled = !Game.GameIsRunning;
            btnStepBack.IsEnabled = false;
            btnStepForward.IsEnabled = !Game.GameIsRunning;
            btnResume.IsEnabled = !Game.GameIsRunning;
            btnPause.IsEnabled = Game.GameIsRunning;
            btnRandomize.IsEnabled = !Game.GameIsRunning;
        }
    }
}

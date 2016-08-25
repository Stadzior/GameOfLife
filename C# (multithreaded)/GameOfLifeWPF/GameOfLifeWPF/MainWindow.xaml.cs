using GameOfLifeWPF.Model;
using GameOfLifeWPF.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            InitializePlayground();
        }

        private void InitializePlayground()
        {
            int playgroundSize = 10;
            for (double i = 0; i < wrapPanel.Width; i+=(wrapPanel.Width / playgroundSize))
            {
                for (double j = 0; j < wrapPanel.Height; j+=(wrapPanel.Height / playgroundSize))
                {
                    BaseCell cell = new RegularCell();
                    cell.Background = Brushes.Bisque;
                    cell.Width = wrapPanel.Width / playgroundSize;
                    cell.Height = wrapPanel.Height / playgroundSize;
                    cell.Click += Cell_Click;
                    wrapPanel.Children.Add(cell);
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Background = Brushes.Purple;
        }
    }
}

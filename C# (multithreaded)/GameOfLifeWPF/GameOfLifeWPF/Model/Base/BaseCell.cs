using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLifeWPF.Model.Base
{
    public abstract class BaseCell : Button
    {
        public void Survive()
        {
            //while (true)
            //{
                bool survived = new Random().Next(2) == 1; //TODO Temporary, to be further implemented;
                if (Dispatcher.CheckAccess())
                {
                    Background = survived ? Brushes.Purple : Brushes.Bisque;
                }
                else
                {
                    Dispatcher.Invoke(() => Background = survived ? Brushes.Purple : Brushes.Bisque);
                }
            //}
        }
    }
}

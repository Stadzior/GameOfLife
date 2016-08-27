using GameOfLifeWPF.Model.Base;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;

namespace GameOfLifeWPF
{
    public class CellCollection : UIElementCollection, IEnumerable<Cell>
    {
        public CellCollection(UIElement visualParent, FrameworkElement logicalParent) : base(visualParent, logicalParent)
        {
        }

        IEnumerator<Cell> IEnumerable<Cell>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        //TODO Casting from List to CellCollection
        public void FromList(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}

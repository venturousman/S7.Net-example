using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HmiExample.Helpers
{
    public static class CommonHelpers
    {
        /// <summary>
        /// Check if bit at pos position is 1 or 0
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pos">position of bit need to check, must be greater than 0</param>
        /// <returns>true / false</returns>
        public static bool IsBitSet(byte n, int pos)
        {
            if (pos == 0) return false;
            return ((n >> (pos - 1)) & 1) != 0;

            //example:
            //        3210 <- standard pos
            //        4321 <- pos
            //n = 6 = 0110
            //pos 0 -> n >> -1 ???
            //pos 1 -> n >> 0 = 0110 & 0001 = 0000
            //pos 2 -> n >> 1 = 0011 & 0001 = 0001
            //pos 3 -> n >> 2 = 0001 & 0001 = 0001
            //pos 4 -> n >> 3 = 0000 & 0001 = 0000
        }

        /// <summary>
        /// http://www.java2s.com/Code/CSharp/Windows-Presentation-Foundation/Findchildren.htm
        /// http://www.java2s.com/Code/CSharp/Windows-Presentation-Foundation/FindControlWithTag.htm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(parent);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                        if (child != null && child is T)
                        {
                            yield return (T)child;
                        }

                        var children = FindVisualChildren<T>(child);
                        foreach (T childOfChild in children)
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static T FindControlWithTag<T>(this DependencyObject parent, string tag) where T : UIElement
        {
            if (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                        if (typeof(FrameworkElement).IsAssignableFrom(child.GetType()) &&
                            ((FrameworkElement)child).Tag != null && (((FrameworkElement)child).Tag.ToString() == tag))
                        {
                            return child as T;
                        }
                        var item = FindControlWithTag<T>(child, tag);
                        if (item != null) return item as T;
                    }
                }
            }

            return null;
        }

        public static DataGridCell GetCell(DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = GetRow(grid, row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    grid.ScrollIntoView(rowContainer, grid.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        public static DataGridRow GetRow(DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static double GetWidth(int pix)
        {
            return (pix - 12 + 5) / 7d + 1;
        }

        public static double GetHeight(int pix)
        {
            return pix * 72 / 96d;
        }
    }
}

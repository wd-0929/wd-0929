using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WheelTest
{
    public static class Utility
    {
        //FocusManager.IsFocusScope="False"-点击调
        public static string CompletionUri(this string uriString)
        {
            if (!string.IsNullOrWhiteSpace(uriString) && uriString.Length > 3 && uriString.Substring(0, 2) == "//")
                return "http:" + uriString;
            else
                return uriString;
        }
        public static T[] GetSelectedItems<T>(this ListBox listBox)
        {
            if (listBox.IsSelected())
            {
                if (listBox.Items.Count == listBox.SelectedItems.Count)
                {
                    return listBox.Items.OfType<T>().ToArray();
                }
                else
                {
                    int[] indexs = new int[listBox.SelectedItems.Count];
                    for (int i = 0; i < listBox.SelectedItems.Count; i++)
                    {
                        indexs[i] = listBox.Items.IndexOf(listBox.SelectedItems[i]);
                    }
                    return indexs.OrderBy(tmp => tmp).Select(index => (T)listBox.Items[index]).ToArray();
                }
            }
            else
            {
                return new T[0];
            }
        }
        public static bool IsSelected(this ListBox listBox)
        {
            return (listBox.IsInitialized || listBox.ItemsSource is System.Windows.Data.ListCollectionView) && listBox.SelectedItems.Count > 0;
        }
    }
}

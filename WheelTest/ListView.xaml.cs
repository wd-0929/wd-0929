using JSON.GTA.ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>   
    /// ListView.xaml 的交互逻辑
    /// </summary>
    public partial class ListView : Window
    {
        public ListView()
        {
            InitializeComponent();
            ExeterItems = new string[] { "11111", "22222", "333333" }.ToList();
        }


        public List<string> ExeterItems
        {
            get { return (List<string>)GetValue(ExeterItemsProperty); }
            set { SetValue(ExeterItemsProperty, value); }
        }

        public static readonly DependencyProperty ExeterItemsProperty =
            DependencyProperty.Register(nameof(ExeterItems), typeof(List<string>), typeof(ListView), new PropertyMetadata(null));

            

        public static T FindParentOfType<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return null;

            // 如果当前父控件就是目标类型，则直接返回
            if (parent is T typedParent)
                return typedParent;

            // 否则继续向上查找
            return FindParentOfType<T>(parent);
        }

        private void SelectAll1_Click(object sender, RoutedEventArgs e)
        {

        }
        AdornerLayer mAdornerLayer;
        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as TextBlock)))))) as ListBoxItem;

            DragDropAdorner adorner = new DragDropAdorner(listBoxItem, new Point(0, 0));
            mAdornerLayer = AdornerLayer.GetAdornerLayer(mTopLevelGrid);
            mAdornerLayer.Add(adorner);
            System.Windows.DragDrop.AddPreviewGiveFeedbackHandler(listBoxItem, aaaa);
            System.Windows.DragDrop.DoDragDrop(listBoxItem, listBoxItem.DataContext, DragDropEffects.Copy);
            mAdornerLayer.Remove(adorner);
        }

        private void aaaa(object sender, GiveFeedbackEventArgs e)
        {
            //_ssss.Text = string.Format("{0}:{1}", System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);
            //mAdornerLayer.Margin =new Thickness(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y,0,0) ;
            //mAdornerLayer.InvalidateArrange();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExeterItems = new string[] { "333", "42", "333331231233" }.ToList();
        }
    }
}

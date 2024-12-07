using JSON.GTA.ControlLibrary;
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
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>
    /// 拖拽操作.xaml 的交互逻辑
    /// </summary>
    public partial class 拖拽操作 : Window
    {
        public 拖拽操作()
        {
            InitializeComponent();
        }
        private void DraggablePanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //System.Windows.Controls.Image draggedItem = sender as System.Windows.Controls.Image;
            //var data = draggedItem.DataContext as object;
            //var listBoxItem = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as Image)))) as ListBoxItem;

            //DragDropAdorner adorner = new DragDropAdorner(listBoxItem, new Point(826, 20));
            //mAdornerLayer = AdornerLayer.GetAdornerLayer(mTopLevelGrid);
            //mAdornerLayer.Add(adorner);
            //System.Windows.DragDrop.AddPreviewGiveFeedbackHandler(listBoxItem, aaaa);
            //System.Windows.DragDrop.DoDragDrop(listBoxItem, draggedItem.DataContext, DragDropEffects.Move);
            //mAdornerLayer.Remove(adorner);
        }

    
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WheelTest.MainWindow;

namespace WheelTest
{
    /// <summary>
    /// DoDragDrop.xaml 的交互逻辑
    /// </summary>
    public partial class DoDragDrop : Window
    {
        private ViewModel viewModel;
        public DoDragDrop()
        {
            InitializeComponent();
            DataContext = viewModel = new ViewModel();
        }
        class ViewModel : INotifyPropertyChanged
        {
            #region INotifyPropertyChanged
            [NonSerialized]
            private PropertyChangedEventHandler _propertyChanged;
            public event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    _propertyChanged += value;
                }
                remove
                {
                    _propertyChanged -= value;
                }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                if (_propertyChanged != null)
                {
                    _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            #endregion INotifyPropertyChanged

            #region Datas Property
            private int[] _datas;
            /// <summary>
            /// 测试数据
            /// </summary>
            public int[] Datas
            {
                get
                {
                    return _datas;
                }
                set
                {
                    if (_datas != value)
                    {
                        _datas = value;
                        OnPropertyChanged(nameof(Datas));
                    }
                }
            }
            #endregion Datas Property

            public ViewModel()
            {
                List<int> dat = new List<int>();
                for (int i = 0; i < 100; i++)
                {
                    dat.Add(i);
                }
                Datas = dat.ToArray();
            }
            #region TextblockY Property
            private double _textblockY;
            /// <summary>
            /// describe
            /// </summary>
            public double TextblockY   
            {
                get
                {
                    return _textblockY;
                }
                set
                {
                    if (_textblockY != value)
                    {
                        _textblockY = value;
                        OnPropertyChanged(nameof(TextblockY));
                    }
                }
            }
            #endregion TextblockY Property

            #region TextblockX Property
            private double _textblockX;
            /// <summary>
            /// describe
            /// </summary>  
            public double TextblockX
            {
                get
                {
                    return _textblockX;
                }
                set
                {
                    if (_textblockX != value)
                    {
                        _textblockX = value;
                        OnPropertyChanged(nameof(TextblockX));
                    }
                }
            }
            #endregion TextblockX Property

            public bool IsDoDragDrop;
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && sender is Border)
            {
                Border draggedItem = sender as Border;
                viewModel.IsDoDragDrop = true;
                ThreadProxy.Execute(() =>
                {
                    while (viewModel.IsDoDragDrop)
                    {
                        Thread.Sleep(1000);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            Point ptLeftUp = new Point(0, 0);

                            ptLeftUp = this.PointToScreen(ptLeftUp);
                            viewModel.TextblockY = System.Windows.Forms.Control.MousePosition.Y;
                            Window window = Window.GetWindow(_ScrollViewer);
                            Point point = _ScrollViewer.TransformToAncestor(window).Transform(new Point(0, 0));
                            viewModel.TextblockX = point.Y+ ptLeftUp.Y;
                            //var index = 0;
                            //if(point.Y<0)
                            //{
                            //    index = -10;
                            //}
                            //else if(point.Y> _ScrollViewer.ActualHeight) 
                            //{
                            //    index = 10;
                            //}
                            //_ScrollViewer.ScrollToVerticalOffset(_ScrollViewer.ContentVerticalOffset + index);
                        }));
                    }
                });
                DragDrop.DoDragDrop(draggedItem, draggedItem.Tag, DragDropEffects.Move);
                viewModel.IsDoDragDrop = false;
            }
        }
    }
}

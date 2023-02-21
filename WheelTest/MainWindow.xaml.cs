using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WheelTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }
        public class ViewModel : INotifyPropertyChanged
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

            #region ItemsSource Property
            private string[] _ItemsSource= {"1","2","3","4","5" };
            /// <summary>
            /// describe
            /// </summary>
            public string[] ItemsSource
            {   
                get
                {
                    return _ItemsSource;
                }
                set
                {
                    if (_ItemsSource != value)
                    {
                        _ItemsSource = value;
                        OnPropertyChanged(nameof(ItemsSource));
                    }
                }
            }
            #endregion ItemsSource Property

            #region Text Property
            private string _text="9";
            /// <summary>
            /// describe
            /// </summary>
            public string Text  
            {
                get
                {
                    return _text;
                }
                set
                {
                    if (_text != value)
                    {
                        _text = value;
                        OnPropertyChanged(nameof(Text));
                    }
                }
            }
            #endregion Text Property


        }

        private void Bd_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            var tag = border.Tag;
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {

        }
    }
}

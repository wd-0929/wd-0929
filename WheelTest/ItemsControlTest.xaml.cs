using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// ItemsControlTest.xaml 的交互逻辑
    /// </summary>
    public partial class ItemsControlTest : Window
    {
        public ItemsControlTest()
        {
            InitializeComponent();

            DataContext = new ViewModel();
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

            public string[] list 
            {
                get
                {
                    return new string[] {"dasdas","dasd","asdas" };
                } 
            }

        }
    }
}

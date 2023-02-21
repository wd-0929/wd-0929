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
using System.Windows.Shapes;

namespace WheelTest
{
    /// <summary>
    /// DynamicImages.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicImages : Window
    {
        public DynamicImages()
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

            #region Datas Property
            private string[] _datas = new string[10];
            /// <summary>
            /// describe
            /// </summary>
            public string[] Datas
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


            #region ImageUrl Property
            private string _imageUrl = "C:\\Users\\1135814967@qq.com\\Desktop\\lQDPJxbnGkiZYhrNA-jNAu6wpbfeAGdMeJ8De7R1joBwAA_750_1000.jpg";
            /// <summary>
            /// describe
            /// </summary>
            public string ImageUrl
            {
                get
                {
                    return _imageUrl;
                }
                set
                {
                    if (_imageUrl != value)
                    {
                        _imageUrl = value;
                        OnPropertyChanged(nameof(ImageUrl));
                    }
                }
            }
            #endregion ImageUrl Property

            public ViewModel()
            {

            }

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            var tag = textBlock.Tag;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _to.Visibility = Visibility.Visible;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WheelTest.Style
{
    public class SerializeTest: INotifyPropertyChanged
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



        #region PreviewImage Property
        private BitmapImage _previewImage;
        /// <summary>
        /// describe
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]

        public BitmapImage PreviewImage
        {
            get
            {
                return _previewImage;
            }
            set
            {
                if (_previewImage != value)
                {
                    _previewImage = value;
                }
            }
        }
        #endregion PreviewImage Property
    }
}

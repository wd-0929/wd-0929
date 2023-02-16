using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WheelTest.Style.Converters
{
    [ValueConversion(typeof(string), typeof(Color))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return System.Windows.Media.ColorConverter.ConvertFromString(value?.ToString());
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is System.Windows.Media.Color)
            {
                System.Windows.Media.Color color = (System.Windows.Media.Color)value;
                return color.ToString();
            }
            return null;
        }
    }
}

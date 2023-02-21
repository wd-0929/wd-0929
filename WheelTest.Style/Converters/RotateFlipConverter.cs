using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WheelTest.Style.Converters
{

    public class RotateFlipConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is RotateFlipType)
            {
                RotateFlipType type = (RotateFlipType)value;
                switch (type)
                {
                    case RotateFlipType.RotateNoneFlipNone:
                        return "不旋转";
                    case RotateFlipType.Rotate90FlipNone:
                        return "90度顺时针旋转";
                    case RotateFlipType.Rotate180FlipNone:
                        return "180度顺时针旋转";
                    case RotateFlipType.Rotate270FlipNone:
                        return "270度顺时针旋转";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

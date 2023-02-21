using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace WheelTest.Style.Converters
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.GetType().IsEnum && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                if (string.IsNullOrWhiteSpace(parameter?.ToString()))
                {
                    return DisplayHelper.GetDisplayName((Enum)value);
                }
                else
                {
                    if (Regex.IsMatch(parameter?.ToString(), "\\{[^\\}]+\\}"))
                    {
                        return string.Format(parameter.ToString(), DisplayHelper.GetDisplayName((Enum)value));
                    }
                    else
                    {
                        return DisplayHelper.GetDisplayName((Enum)value);
                    }
                }
            }
            else if (parameter != null)
                return parameter;
            else return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return Enum.ToObject(value.GetType(), value);
            }
            catch
            {
                return null;
            }
        }
    }
}

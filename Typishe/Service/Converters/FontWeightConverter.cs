using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
using System.Windows;

namespace Typishe.Service
{
    public class FontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var weightValue = (int) parameter.ToString().Parse();
            return FontWeight.FromOpenTypeWeight(weightValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

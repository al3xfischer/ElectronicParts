using ElectronicParts.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ElectronicParts.Converter
{
    class ConnectionToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int inputLeft = (int)values[0];
            int inputTop = (int)values[1];
            int outputLeft = (int)values[2];
            int outputTop = (int)values[3];

            int left = 0;

            if (inputLeft - outputLeft < 0)
            {
                left = outputLeft + (inputLeft - outputLeft) - 10;
            }
            else
            {
                left = inputLeft + (outputLeft - inputLeft) - 10;
            }

            return new Thickness(left, outputTop + (inputTop - outputTop) / 2 - 20, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

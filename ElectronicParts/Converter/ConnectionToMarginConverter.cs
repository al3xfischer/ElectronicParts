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
            PinViewModel outputPinVM = values[0] as PinViewModel;

            PinViewModel inputPinVM = values[1] as PinViewModel;

            return new Thickness(outputPinVM.Left + (inputPinVM.Left - outputPinVM.Left) / 2 - 10, outputPinVM.Top + (inputPinVM.Top - outputPinVM.Top) / 2 - 20, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

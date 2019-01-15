using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicParts.Views.Converter
{
    public class PinToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return 20;
            }

            int.TryParse(value.ToString(), out int height);
            return height * 20 == 0 ? 20 : height * 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

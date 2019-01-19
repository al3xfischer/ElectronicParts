using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ElectronicParts.Converter
{
    public class GridSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var gridSize = (int)value;
                return $"-{gridSize} -{gridSize} {gridSize} {gridSize}";
            }
            catch (InvalidCastException)
            {
                return "-10 -10 10 10";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

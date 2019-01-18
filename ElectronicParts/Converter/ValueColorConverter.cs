using ElectronicParts.DI;
using ElectronicParts.Services.Interfaces;
using Shared;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicParts.Converter
{
    public class ValueColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)ColorConverter.ConvertFromString("black");
            var black = new SolidColorBrush(color);

            if (value is null)
            {
                return black;
            }

            var type = value
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValueGeneric<>))
                ?.GetGenericArguments()[0];

            if (type is null)
            {
                return black;
            }

            var conf = Container.Resolve<IConfigurationService>().Configuration;
            var colorName = string.Empty;
            switch (type)
            {
                case var obj when type == typeof(bool):
                    colorName = conf.BoolRules.First(r => r.Value == (value as IValueGeneric<bool>).Current).Color;
                    break;
                case var obj when type == typeof(int):
                    colorName = conf.IntRules.First(r => r.Value == (value as IValueGeneric<int>).Current).Color;
                    break;
                case var obj when type == typeof(string):
                    colorName = conf.StringRules.First(r => r.Value == (value as IValueGeneric<string>).Current).Color;
                    break;
                default:
                    colorName = "black";
                    break;
            }

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorName));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

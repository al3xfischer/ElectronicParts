// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="BoolToColorConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the BoolToColorConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Represents the <see cref="BoolToColorConverter"/> class of the ElectronicParts program.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a color.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            SolidColorBrush color = Brushes.Black;

            if (boolValue)
            {
                color = Brushes.LightGreen;
            }

            return color;
        }

        /// <summary>
        /// Converts a color to a boolean.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted boolean.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

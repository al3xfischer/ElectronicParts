// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="GridSizeConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the GridSizeConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Represents the GridSizeConverter class of the ElectronicParts program.
    /// </summary>
    public class GridSizeConverter : IValueConverter
    {
        /// <summary>
        /// Converts an integer to a string describing the cell size.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>A string describing the cell size.</returns>
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

        /// <summary>
        /// Converts a string describing the cell size to an integer. 
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted integer.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

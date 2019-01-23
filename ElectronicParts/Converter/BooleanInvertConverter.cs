// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="BooleanInvertConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the BooleanInvertConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// A boolean inverter.
    /// </summary>
    public class BooleanInvertConverter : IValueConverter
    {
        /// <summary>
        /// Inverts a given boolean.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The inverted boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// Inverts a given boolean.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The inverted boolean.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

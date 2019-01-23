// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="PinToHeightConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PinToHeightConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Represents the PinToHeightConverter class of the ElectronicParts program.
    /// </summary>
    public class PinToHeightConverter : IValueConverter
    {
        /// <summary>
        /// Converts the amount of pins in a node to its height.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The height of the node.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return 20;
            }

            int.TryParse(value.ToString(), out int height);
            return height * 20 == 0 ? 20 : height * 20;
        }

        /// <summary>
        /// Converts the height of a node to the amount of pins.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The amount of pins.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

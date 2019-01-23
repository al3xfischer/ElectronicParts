// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="IntToPointMulitConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IntToPointMulitConverter.cs class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts integers to a <see cref="Point"/> instance.
    /// </summary>
    public class IntToPointMulitConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts integers to a <see cref="Point"/> instance..
        /// </summary>
        /// <param name="values">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted point.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return new Point((int)values[0], (int)values[1]);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a point to its integer values.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetTypes">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted integers.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

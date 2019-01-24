// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="ConnectionToMarginConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ConnectionToMarginConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Represents the ConnectionToMarginConverter class of the ElectronicParts program.
    /// </summary>
    public class ConnectionToMarginConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts connection value to margins.
        /// </summary>
        /// <param name="values">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted margins.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //    var leftPoint = (Point)values[0];
            //    var rightPoint = (Point)values[1];
            //    //var targetPoint = new Point();

            //    //targetPoint.X = leftPoint.X > rightPoint.X ? leftPoint.X - rightPoint.X : rightPoint.X - leftPoint.X;
            //    //targetPoint.Y = leftPoint.Y > rightPoint.Y ? leftPoint.Y - rightPoint.Y : rightPoint.Y - leftPoint.Y;
            //    //targetPoint.X += Math.Min(leftPoint.X,rightPoint.X);
            //    //targetPoint.Y += Math.Min(leftPoint.Y,rightPoint.Y);
            //    //targetPoint.X += leftPoint.X;
            //    //targetPoint.Y += leftPoint.Y;
            //    //leftPoint.X += 60;
            //    //leftPoint.Y -= 60;
            //    //leftPoint.X += 20;
            //    //leftPoint.Y += 20;
            //    //return leftPoint;

            //    var t = new Thickness(leftPoint.X + leftPoint.X / 4, leftPoint.Y + leftPoint.Y / 4, 0, 0);
            //    return t;

            int inputLeft = (int)values[0];
            int inputTop = (int)values[1];
            int outputLeft = (int)values[2];
            int outputTop = (int)values[3];
            Point bottomPoint = (Point)values[4];
            Point topPoint = (Point)values[5];


            int left = 0;

            if (inputLeft - outputLeft < 0)
            {
                left = outputLeft + (inputLeft - outputLeft) - 10;
            }
            else
            {
                left = inputLeft + (outputLeft - inputLeft) - 10;
            }

            //return new Thickness(left, outputTop + ((inputTop - outputTop) / 2) - 20, 0, 0);
            return new Thickness(bottomPoint.X, bottomPoint.Y - 20, 0, 0);
        }

        /// <summary>
        /// Converts a margin to connection values.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetTypes">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted connection values.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

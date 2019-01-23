// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="BitmapToImageSourceConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the BitmapToImageSourceConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Converter
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents the BitmapToImageSourceConverter class of the ElectronicParts program.
    /// </summary>
    public class BitmapToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// Converts a Bitmap to a BitmapImage.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted BitmapImage.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream();
            (value as Bitmap).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        /// <summary>
        /// Converts a BitmapImage to a Bitmap.
        /// </summary>
        /// <param name="value">The value which is converted.</param>
        /// <param name="targetType">The type of the value that is required.</param>
        /// <param name="parameter">A optional parameter used during the conversion.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The converted Bitmap.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

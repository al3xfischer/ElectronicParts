// ***********************************************************************
// Assembly         : ElectronicParts.ViewModels
// Author           : 
// ***********************************************************************
// <copyright file="Extensions.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Extensions class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace System
{
    using ElectronicParts.ViewModels;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Includes extensions for the ElectronicParts program.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns a given IEnumerable as observable collection.
        /// </summary>
        /// <typeparam name="TValue">The generic type of the IEnumerable.</typeparam>
        /// <param name="values">The IEnumerable which will be converted.</param>
        /// <returns>The given IEnumerable as observable collection.</returns>
        public static ObservableCollection<TValue> ToObservableCollection<TValue>(this IEnumerable<TValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new ObservableCollection<TValue>(values);
        }        

        public static int RoundTo(this int input, int roundTo)
        {
            return (int)Math.Round((double)input / roundTo) * roundTo;
        }

        public static long RoundTo(this long input, int roundTo)
        {
            return input.RoundTo(roundTo);
        }

        public static double RoundTo(this double input, int roundTo)
        {
            return Math.Round(input / roundTo) * roundTo;
        }

        public static int FloorTo(this int input, int floorTo)
        {
            return (int)Math.Floor((double)input / floorTo) * floorTo;
        }

        public static long FloorTo(this long input, int floorTo)
        {
            return input.FloorTo(floorTo);
        }

        public static double FloorTo(this double input, int floorTo)
        {
            return Math.Floor(input / floorTo) * floorTo;
        }

        public static int CeilingTo(this int input, int ceilingTo)
        {
            return (int)Math.Ceiling((double)input / ceilingTo) * ceilingTo;
        }

        public static long CeilingTo(this long input, int ceilingTo)
        {
            return input.CeilingTo(ceilingTo);
        }

        public static double CeilingTo(this double input, int ceilingTo)
        {
            return Math.Ceiling(input / ceilingTo) * ceilingTo;
        }
    }
}

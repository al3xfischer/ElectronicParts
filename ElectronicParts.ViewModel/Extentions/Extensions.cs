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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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

        /// <summary>
        /// Rounds an <see cref="int"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to be rounded.</param>
        /// <param name="roundTo">The <see cref="int"/> the input gets rounded to.</param>
        /// <returns>A rounded <see cref="int"/>.</returns>
        public static int RoundTo(this int input, int roundTo)
        {
            return (int)((long)input).RoundTo(roundTo);
        }

        /// <summary>
        /// Rounds a <see cref="long"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to be rounded.</param>
        /// <param name="roundTo">The <see cref="int"/> the input gets rounded to.</param>
        /// <returns>A rounded <see cref="long"/>.</returns>
        public static long RoundTo(this long input, int roundTo)
        {
            return (long)Math.Round((double)input / roundTo) * roundTo;
        }

        /// <summary>
        /// Rounds a <see cref="double"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be rounded.</param>
        /// <param name="roundTo">The <see cref="int"/> the input gets rounded to.</param>
        /// <returns>A rounded <see cref="double"/>.</returns>
        public static double RoundTo(this double input, int roundTo)
        {
            return ((long)input).RoundTo(roundTo);
        }

        /// <summary>
        /// Floors an <see cref="int"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to be floored.</param>
        /// <param name="floorTo">The <see cref="int"/> the input gets floored to.</param>
        /// <returns>A floored <see cref="int"/>.</returns>
        public static int FloorTo(this int input, int floorTo)
        {
            return (int)((long)input).FloorTo(floorTo);
        }

        /// <summary>
        /// Floors a <see cref="long"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to be floored.</param>
        /// <param name="floorTo">The <see cref="int"/> the input gets floored to.</param>
        /// <returns>A floored <see cref="long"/>.</returns>
        public static long FloorTo(this long input, int floorTo)
        {
            return (long)Math.Floor((double)input / floorTo) * floorTo;
        }

        /// <summary>
        /// Floors a <see cref="double"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be floored.</param>
        /// <param name="floorTo">The <see cref="int"/> the input gets floored to.</param>
        /// <returns>A floored <see cref="double"/>.</returns>
        public static double FloorTo(this double input, int floorTo)
        {
            return ((long)input).FloorTo(floorTo);
        }

        /// <summary>
        /// Ceils an <see cref="int"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to be ceiled.</param>
        /// <param name="ceilingTo">The <see cref="int"/> the input gets ceiled to.</param>
        /// <returns>A ceiled <see cref="int"/>.</returns>
        public static int CeilingTo(this int input, int ceilingTo)
        {
            return (int)((long)input).CeilingTo(ceilingTo);
        }

        /// <summary>
        /// Ceils a <see cref="long"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to be ceiled.</param>
        /// <param name="ceilingTo">The <see cref="int"/> the input gets ceiled to.</param>
        /// <returns>A ceiled <see cref="long"/>.</returns>
        public static long CeilingTo(this long input, int ceilingTo)
        {
            return (long)Math.Ceiling((double)input / ceilingTo) * ceilingTo;
        }

        /// <summary>
        /// Ceils a <see cref="double"/> to the next given <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be ceiled.</param>
        /// <param name="ceilingTo">The <see cref="int"/> the input gets ceiled to.</param>
        /// <returns>A ceiled <see cref="double"/>.</returns>
        public static double CeilingTo(this double input, int ceilingTo)
        {
            return ((long)input).CeilingTo(ceilingTo);
        }
    }
}

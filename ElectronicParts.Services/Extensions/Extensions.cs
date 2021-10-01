// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="Extensions.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Extensions class of the ElectronicParts.Services project</summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace ElectronicParts.Services.Extensions
{
    /// <summary>
    ///     Represents the <see cref="Extensions" /> class of the ElectronicParts.Services application.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Returns the index of a given object within an IEnumerable. Using the default EqualityComparer.
        /// </summary>
        /// <typeparam name="T">Represents the type of the value.</typeparam>
        /// <param name="input">The input enumerable.</param>
        /// <param name="value">The value to get the index of.</param>
        /// <returns>The index of the value within the enumerable.</returns>
        public static int IndexOf<T>(this IEnumerable<T> input, T value)
        {
            return input.IndexOf(value, null);
        }

        /// <summary>
        ///     Returns the index of a given object within an IEnumerable. Using the default EqualityComparer.
        /// </summary>
        /// <typeparam name="T">Represents the type of the value.</typeparam>
        /// <param name="input">The input enumerable.</param>
        /// <param name="value">The value to get the index of.</param>
        /// <param name="comparer">The comparer used for searching the value in the enumerable.</param>
        /// <returns>The index of the value within the enumerable.</returns>
        public static int IndexOf<T>(this IEnumerable<T> input, T value, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var found = input
                .Select((a, i) => new {a, i})
                .FirstOrDefault(x => comparer.Equals(x.a, value));
            return found == null ? -1 : found.i;
        }
    }
}
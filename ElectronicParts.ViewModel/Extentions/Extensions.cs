using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
    public static class Extensions
    {
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

        public static int FloorTo(this int input, int floorTo)
        {
            return (int)Math.Floor((double)input / floorTo) * floorTo;
        }

        public static long FloorTo(this long input, int floorTo)
        {
            return input.FloorTo(floorTo);
        }

        public static int CeilingTo(this int input, int ceilingTo)
        {
            return (int)Math.Ceiling((double)input / ceilingTo) * ceilingTo;
        }

        public static long CeilingTo(this long input, int ceilingTo)
        {
            return input.CeilingTo(ceilingTo);
        }
    }
}

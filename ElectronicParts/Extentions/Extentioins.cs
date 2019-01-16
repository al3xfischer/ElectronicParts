using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
    public static class Extentioins
    {
        public static ObservableCollection<TValue> ToObservableCollection<TValue>(this IEnumerable<TValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new ObservableCollection<TValue>(values);
        }
    }
}

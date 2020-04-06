using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.CoreExtensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<U> Apply<T, U>(this IEnumerable<T> enumerable, Func<T, U> function)
        {
            List<U> appliedEnumerable = new List<U>();
            foreach (T item in enumerable)
                appliedEnumerable.Add(function(item));

            return appliedEnumerable as IEnumerable<U>;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace LORModingBase.CustomExtensions
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class IEnumerable
    {
        /// <summary>
        /// Safely foreach IEnumberable object
        /// </summary>
        public static void ForEachSafe<T>(this IEnumerable<T> source, Action<T> foreachCallBack)
        {
            foreach (T eachObject in source ?? Enumerable.Empty<T>())
                foreachCallBack(eachObject);
        }
    }
}

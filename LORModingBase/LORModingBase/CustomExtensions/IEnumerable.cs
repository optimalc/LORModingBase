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

        /// <summary>
        /// Find string for given string
        /// </summary>
        public static List<string> FindAll_Contains(this string[] source, string strToSearch, bool ignoreCase = false)
        {
            List<string> findList = new List<string>();
            foreach (string eachStr in source)
            {
                if (!ignoreCase && eachStr.Contains(strToSearch))
                    findList.Add(eachStr);
                if (ignoreCase && eachStr.ToLower().Contains(strToSearch.ToLower()))
                    findList.Add(eachStr);
            }
            return findList;
        }

        /// <summary>
        /// Key value for each for dictionary if not null
        /// </summary>
        public static void ForEachKeyValuePairSafe<A, B>(this Dictionary<A, B> dicToForEach, Action<A, B> dicForEachAction)
        {
            if (dicToForEach == null) return;
            foreach (KeyValuePair<A, B> eachPair in dicToForEach)
                dicForEachAction(eachPair.Key, eachPair.Value);
        }

        /// <summary>
        /// Action for one item if not null or empty
        /// </summary>
        public static void ActionOneItemSafe<A>(this List<A> listToAction, Action<A> action)
        {
            if (listToAction != null && listToAction.Count > 0)
                action(listToAction[0]);
        }
    
        /// <summary>
        /// Get unique list
        /// </summary>
        /// <param name="listToUnique"></param>
        /// <returns></returns>
        public static List<string> GetUniqueList(this List<string> listToUnique)
        {
            return new HashSet<string>(listToUnique).ToList();
        }
    }
}

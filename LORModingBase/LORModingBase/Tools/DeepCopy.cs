using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Deep copy tools
    /// </summary>
    public static class DeepCopy
    {
        /// <summary>
        /// Deep copy class
        /// </summary>
        /// <typeparam name="T">Class to copy(Must be [Serializable]</typeparam>
        /// <param name="obj">Object to copy</param>
        /// <returns>Copied cobject</returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}

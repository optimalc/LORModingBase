using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for math
    /// </summary>
    class MathTools
    {
        public static Random _random = new Random();

        /// <summary>
        /// Generates a random number within a range
        /// <para> :Parameters: </para>
        /// <para> min - Min number </para>
        /// <para> max - Max number </para>
        /// <para> :Example: </para>
        /// <para> GetRandomNumber(10, 50) </para>
        /// <para> // Get randomber in between 10 and 50 </para>
        /// </summary>
        /// <param name="min">Min number</param>
        /// <param name="max">Max number</param>
        /// <returns>Randomized number</returns>
        public static int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}

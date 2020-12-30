using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DS
{
    /// <summary>
    /// Game infos data structure
    /// </summary>
    class GameInfo
    {
        /// <summary>
        /// Resists info doc
        /// </summary>
        public static List<string> resistInfo_Doc = new List<string>() { "취약", "약점", "보통", "견딤", "내성", "면역" };

        /// <summary>
        /// Resist info code
        /// </summary>
        public static List<string> resistInfo_Code = new List<string>() { "Vulnerable", "Weak", "Normal", "Endure", "Resist", "Immune" };
    }
}

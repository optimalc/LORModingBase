using System.Collections.Generic;

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

        /// <summary>
        /// Resist info dictionary reverse
        /// </summary>
        public static Dictionary<string, string> resistInfo_Dic_Rev = new Dictionary<string, string>()
        {
            {"취약", "Vulnerable" },
            {"약점", "Weak" },
            {"보통", "Normal" },
            {"견딤", "Endure" },
            {"내성", "Resist" },
            {"면역", "Immune" }
        };

        /// <summary>
        /// Resist info dictionary
        /// </summary>
        public static Dictionary<string, string> resistInfo_Dic = new Dictionary<string, string>()
        {
            {"Vulnerable", "취약" },
            {"Weak", "약점" },
            {"Normal", "보통" },
            {"Endure", "견딤" },
            {"Resist", "내성" },
            {"Immune", "면역" }
        };

        /// <summary>
        /// Chpater info doc
        /// </summary>
        public static Dictionary<string, string> chapter_Dic = new Dictionary<string, string>() {
            {"1", "뜬소문" },
            {"2", "도시 괴담" },
            {"3", "도시 전설" },
            {"4", "도시 질병" },
            {"5", "도시 악몽" },
            {"6", "도시의 별" },
            {"7", "챕터 7" }
        };
    }
}

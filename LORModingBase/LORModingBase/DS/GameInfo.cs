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

        /// <summary>
        /// Chpater info doc
        /// </summary>
        public static Dictionary<string, string> chapter_Doc = new Dictionary<string, string>() {
            {"1", "뜬소문" },
            {"2", "도시 괴담" },
            {"3", "도시 전설" },
            {"4", "도시 질병" },
            {"5", "도시 악몽" },
            {"6", "도시의 별" },
            {"7", "챕터 7" }
        };
    }

    /// <summary>
    /// Stage infos data structure
    /// </summary>
    class StageInfo
    {
        /// <summary>
        /// Stage ID
        /// </summary>
        public string stageID = "";

        /// <summary>
        /// Stage discription
        /// </summary>
        public string stageDoc = "";

        /// <summary>
        /// Stage chapter
        /// </summary>
        public string Chapter = "";  
    }

    /// <summary>
    /// Passive infos data structure
    /// </summary>
    class PassiveInfo
    {
        /// <summary>
        /// Passive uniqe ID
        /// </summary>
        public string passiveID = "";

        /// <summary>
        /// Passive name
        /// </summary>
        public string passiveName = "";

        /// <summary>
        /// Passive description
        /// </summary>
        public string passiveDes = "";
    }

    /// <summary>
    /// Critical page data structure
    /// </summary>
    class CriticalPageInfo
    {
        public string rarity = "";
        public string bookID = "";
        public string name = "";

        public string chapter = "";
        public string episode = "";

        public string HP = "";
        public string breakNum = "";

        public string minSpeedCount = "";
        public string maxSpeedCount = "";

        public string SResist = "";
        public string PResist = "";
        public string HResist = "";

        public string BSResist = "";
        public string BPResist = "";
        public string BHResist = "";
    }
}

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
        /// Resist info dictionary
        /// </summary>
        public static Dictionary<string, string> resistInfo_Dic_Rev = new Dictionary<string, string>()
        {
            {"취약", "Vulnerable" },
            {"약점", "Weak" },
            {"보통", "Normal" },
            {"견딤", "Endure" },
            {"내성", "Endure" },
            {"면역", "Immune" },
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
    public class CriticalPageInfo
    {
        public string rarity = "Common";
        public string bookID = Tools.MathTools.GetRandomNumber(1000000, 9999999).ToString();
        public string name = "";

        public string chapter = "";
        public string episode = "";

        public string HP = "50";
        public string breakNum = "50";

        public string minSpeedCount = "1";
        public string maxSpeedCount = "6";

        public string skinName = "";
        public string iconName = "";

        public string SResist = "보통";
        public string PResist = "보통";
        public string HResist = "보통";

        public string BSResist = "보통";
        public string BPResist = "보통";
        public string BHResist = "보통";

        public List<string> passiveIDs = new List<string>();
    }
}

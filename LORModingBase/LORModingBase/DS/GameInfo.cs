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

    /// <summary>
    /// Icon infos data structure
    /// </summary>
    class DropBookInfo
    {
        /// <summary>
        /// Icon name to use
        /// </summary>
        public string iconName = "";

        /// <summary>
        /// Chapter
        /// </summary>
        public string chapter = "";

        /// <summary>
        /// Icon desciption
        /// </summary>
        public string iconDesc = "";

        /// <summary>
        /// bookID
        /// </summary>
        public string bookID = "";

        /// <summary>
        /// Dropped itmes
        /// </summary>
        public List<string> dropItems = new List<string>();
    }

    /// <summary>
    /// Skin infos data structure
    /// </summary>
    class BookSkinInfo
    {
        /// <summary>
        /// Icon name to use
        /// </summary>
        public string skinName = "";

        /// <summary>
        /// Chapter
        /// </summary>
        public string chapter = "";

        /// <summary>
        /// Icon desciption
        /// </summary>
        public string skinDesc = "";
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
    [Serializable]
    public class CriticalPageInfo
    {
        public string rarity = "Common";
        public string bookID = Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_USER, DS.FilterDatas.CARD_DIV_CUSTOM).ToString();
        public string name = "";

        public string chapter = "";
        public string episode = "";
        public string episodeDes = "";

        public string HP = "50";
        public string breakNum = "50";

        public string minSpeedCount = "1";
        public string maxSpeedCount = "6";

        public string skinName = "";
        public string skinDes = "";
        public string iconName = "";
        public string iconDes = "";

        public string SResist = "Normal";
        public string PResist = "Normal";
        public string HResist = "Normal";

        public string BSResist = "Normal";
        public string BPResist = "Normal";
        public string BHResist = "Normal";

        public List<string> passiveIDs = new List<string>();

        /// <summary>
        /// 책장의 이야기
        /// </summary>
        public string description = "입력된 정보가 없습니다";
        /// <summary>
        /// 책장이 어느 책으로부터 나올지의 리스트
        /// </summary>
        public List<string> dropBooks = new List<string>();

        public string rangeType = "Nomal";

        /// <summary>
        /// 시작시 빛의 개수
        /// </summary>
        public string ENEMY_StartPlayPoint = "";
        /// <summary>
        /// 최대 빛의 개수
        /// </summary>
        public string ENEMY_MaxPlayPoint = "";
        /// <summary>
        /// 최대 감정 레벨
        /// </summary>
        public string ENEMY_EmotionLevel = "";
        /// <summary>
        /// 시작시 가지는 책장의 수
        /// </summary>
        public string ENEMY_AddedStartDraw = "";

        public bool ENEMY_TYPE_CH_FORCE = false;
        public bool USER_TYPE_CH_FORCE = false;

        /// <summary>
        /// 적 전용책장 여부
        /// </summary>
        public bool ENEMY_IS_ENEMY_TYPE = false;
    }
}

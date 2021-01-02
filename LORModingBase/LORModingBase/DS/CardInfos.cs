using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DS
{
    /// <summary>
    /// Card information data structure
    /// </summary>
    [Serializable]
    public class CardInfo
    {
        public string cost = "1";
        public string rarity = "Common";

        public string cardID = Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_SPECIAL, DS.FilterDatas.CARD_DIV_FINAL_STORY).ToString();
        public string name = "";

        public string cardImage = "";
        public string cardScript = "";

        public List<Dice> dices = new List<Dice>();

        /// <summary>
        /// 책장이 어느 책으로부터 나올지의 리스트
        /// </summary>
        public List<string> dropBooks = new List<string>();
        /// <summary>
        /// 원거리 타입
        /// </summary>
        public string rangeType = "Near";

        /// <summary>
        /// 전용카드 여부를 설정합니다
        /// </summary>
        public string option = "";

        public string chapter = "1";
        public string priority = "1";
        public string sortPriority = "1";

        public string priorityScript = "";

        /// <summary>
        /// 감정 수치를 제한합니다
        /// </summary>
        public string EXTRA_EmotionLimit = "";
        /// <summary>
        /// 광역 전투 책장
        /// </summary>
        public string EXTRA_Affection = "";
    }

    /// <summary>
    /// Dice information data structure
    /// </summary>
    [Serializable]
    public class Dice
    {
        public string min = "1";
        public string max = "6";

        public string type = "Atk";
        public string detail = "Slash";
        public string motion = "";

        public string script = "";
        public string actionScript = "";

        public string effectres = "";
    }
}

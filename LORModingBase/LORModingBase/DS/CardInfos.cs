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
    class CardInfo
    {
        public string cost = "1";
        public string rarity = "Common";

        public string bookID = Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_SPECIAL, DS.FilterDatas.CARD_DIV_FINAL_STORY).ToString();
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
        public string rangeType = "Nomal";
    }

    /// <summary>
    /// Dice information data structure
    /// </summary>
    [Serializable]
    class Dice
    {
        public string min = "";
        public string max = "";

        public string type = "";
        public string detail = "";
        public string motion = "";

        public string script = "";
        public string actionScript = "";
    }
}

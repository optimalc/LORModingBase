using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Check datas
    /// </summary>
    class CheckDatas
    {
        /// <summary>
        /// Logging data
        /// </summary>
        public static string LOG_DATA = "";

        /// <summary>
        /// Check datas
        /// </summary>
        public static string CheckAllDatas()
        {
            LOG_DATA = "";
            CriticalCheck.CriticalCheckAll();
            CautionCheck.CautionCheckAll();
            return LOG_DATA;
        }

        /// <summary>
        /// Make ciritical log message
        /// </summary>
        /// <param name="languageID">Localize ID</param>
        /// <param name="args">String format param</param>
        public static void MakeCriticalMessage(string languageID, params object[] args)
        {
            string CRITICAL = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Critical");
            LOG_DATA += $"[-][{CRITICAL}] {String.Format(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, languageID), args)}\n";
        }

        /// <summary>
        /// Make caution message
        /// </summary>
        /// <param name="languageID">Localize ID</param>
        /// <param name="args">String format param</param>
        public static void MakeCautionMessage(string languageID, params object[] args)
        {
            string CAUTION = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Caution");
            LOG_DATA += $"[-][{CAUTION}] {String.Format(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, languageID), args)}\n";
        }
    }

    class CriticalCheck
    {
        public static void CriticalCheckAll()
        {
            KeyPageCheck();
            CardCheck();
            StageCheck();
            EnemyCheck();
            DeckCheck();
            DropBookCheck();
            PassiveCheck();
            CardAbilityCheck();
        }

        public static void KeyPageCheck()
        {
            string CRITICAL = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Critical");
            List<string> KEY_PAGE_ID_LIST = new List<string>();
            DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.ActionXmlDataNodesByPath("Book", (XmlDataNode bookNode) =>
            {
                string BOOK_NODE_ID = bookNode.GetAttributesSafe("ID");
                string BOOK_NODE_NAME = bookNode.GetInnerTextByPath("Name");

                if (string.IsNullOrEmpty(BOOK_NODE_ID))
                    CheckDatas.MakeCriticalMessage("KeyPage_Critical_1", BOOK_NODE_NAME);
                if (string.IsNullOrEmpty(BOOK_NODE_NAME))
                    CheckDatas.MakeCriticalMessage("KeyPage_Critical_2", BOOK_NODE_ID);

                if (!string.IsNullOrEmpty(BOOK_NODE_ID))
                {
                    if (KEY_PAGE_ID_LIST.Contains(BOOK_NODE_ID))
                        CheckDatas.MakeCriticalMessage("KeyPage_Critical_3", BOOK_NODE_ID);
                    KEY_PAGE_ID_LIST.Add(BOOK_NODE_ID);
                }
            });
        }

        public static void CardCheck()
        {
            List<string> CARD_PAGE_ID_LIST = new List<string>();
            DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (XmlDataNode bookNode) =>
            {
                string CARD_NODE_ID = bookNode.GetAttributesSafe("ID");
                string CARD_NODE_NAME = bookNode.GetInnerTextByPath("Name");

                if (string.IsNullOrEmpty(CARD_NODE_ID))
                    CheckDatas.MakeCriticalMessage("Card_Critical_1", CARD_NODE_NAME);
                if (string.IsNullOrEmpty(CARD_NODE_NAME))
                    CheckDatas.MakeCriticalMessage("Card_Critical_2", CARD_NODE_ID);

                if (!string.IsNullOrEmpty(CARD_NODE_ID))
                {
                    if (CARD_PAGE_ID_LIST.Contains(CARD_NODE_ID))
                        CheckDatas.MakeCriticalMessage("Card_Critical_3", CARD_NODE_ID);
                    CARD_PAGE_ID_LIST.Add(CARD_NODE_ID);
                }
            });
        }
    
        public static void StageCheck()
        {
            List<string> STAGE_ID_LIST = new List<string>();
            DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.ActionXmlDataNodesByPath("Stage", (XmlDataNode stageNode) =>
            {
                string STAGE_NODE_ID = stageNode.GetAttributesSafe("id");
                string STAGE_NODE_NAME = stageNode.GetInnerTextByPath("Name");

                if (string.IsNullOrEmpty(STAGE_NODE_ID))
                    CheckDatas.MakeCriticalMessage("Stage_Critical_1", STAGE_NODE_NAME);
                if (string.IsNullOrEmpty(STAGE_NODE_NAME))
                    CheckDatas.MakeCriticalMessage("Stage_Critical_2", STAGE_NODE_ID);

                if (!string.IsNullOrEmpty(STAGE_NODE_ID))
                {
                    if (STAGE_ID_LIST.Contains(STAGE_NODE_ID))
                        CheckDatas.MakeCriticalMessage("Stage_Critical_3", STAGE_NODE_ID);
                    STAGE_ID_LIST.Add(STAGE_NODE_ID);
                }
            });
        }

        public static void EnemyCheck()
        {
            List<string> ENEMY_ID_LIST = new List<string>();
            DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.ActionXmlDataNodesByPath("Enemy", (XmlDataNode enemyNode) =>
            {
                string ENYME_NODE_ID = enemyNode.GetAttributesSafe("ID");
                string ENTME_NODE_NAME_ID = enemyNode.GetInnerTextByPath("NameID");

                if (string.IsNullOrEmpty(ENYME_NODE_ID))
                    CheckDatas.MakeCriticalMessage("Enemy_Critical_1", ENTME_NODE_NAME_ID);
                if (string.IsNullOrEmpty(ENTME_NODE_NAME_ID))
                    CheckDatas.MakeCriticalMessage("Enemy_Critical_2", ENYME_NODE_ID);

                if (!string.IsNullOrEmpty(ENYME_NODE_ID))
                {
                    if (ENEMY_ID_LIST.Contains(ENYME_NODE_ID))
                        CheckDatas.MakeCriticalMessage("Enemy_Critical_3", ENYME_NODE_ID);
                    ENEMY_ID_LIST.Add(ENYME_NODE_ID);
                }
            });
        }

        public static void DeckCheck()
        {
            List<string> DECK_ID_LIST = new List<string>();
            DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.ActionXmlDataNodesByPath("Deck", (XmlDataNode deckNode) =>
            {
                string DECK_NODE_ID = deckNode.GetAttributesSafe("ID");

                if (string.IsNullOrEmpty(DECK_NODE_ID))
                    CheckDatas.MakeCriticalMessage("Deck_Critical_1");

                if (!string.IsNullOrEmpty(DECK_NODE_ID))
                {
                    if (DECK_ID_LIST.Contains(DECK_NODE_ID))
                        CheckDatas.MakeCriticalMessage("Deck_Critical_2", DECK_NODE_ID);
                    DECK_ID_LIST.Add(DECK_NODE_ID);
                }
            });
        }
    
        public static void DropBookCheck()
        {
            List<string> DROP_BOOK_ID_LIST = new List<string>();
            DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.ActionXmlDataNodesByPath("BookUse", (XmlDataNode bookUseNode) =>
            {
                string DROP_BOOK_ID = bookUseNode.GetAttributesSafe("ID");

                if (string.IsNullOrEmpty(DROP_BOOK_ID))
                    CheckDatas.MakeCriticalMessage("DropBook_Critical_1");

                if (!string.IsNullOrEmpty(DROP_BOOK_ID))
                {
                    if (DROP_BOOK_ID_LIST.Contains(DROP_BOOK_ID))
                        CheckDatas.MakeCriticalMessage("DropBook_Critical_2", DROP_BOOK_ID);
                    DROP_BOOK_ID_LIST.Add(DROP_BOOK_ID);
                }
            });
        }
    
        public static void PassiveCheck()
        {
            List<string> PASSIVE_ID_LIST = new List<string>();
            DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.ActionXmlDataNodesByPath("Passive", (XmlDataNode passiveNode) =>
            {
                string PASSIVE_ID = passiveNode.GetAttributesSafe("ID");

                if (string.IsNullOrEmpty(PASSIVE_ID))
                    CheckDatas.MakeCriticalMessage("Passive_Critical_1");

                if (!string.IsNullOrEmpty(PASSIVE_ID))
                {
                    if (PASSIVE_ID_LIST.Contains(PASSIVE_ID))
                        CheckDatas.MakeCriticalMessage("Passive_Critical_2", PASSIVE_ID);
                    PASSIVE_ID_LIST.Add(PASSIVE_ID);
                }
            });
        }

        public static void CardAbilityCheck()
        {
            List<string> CARD_ABILITY_ID_LIST = new List<string>();
            DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (XmlDataNode cardAbilityNode) =>
            {
                string CARD_ABILITY_ID = cardAbilityNode.GetAttributesSafe("ID");

                if (string.IsNullOrEmpty(CARD_ABILITY_ID))
                    CheckDatas.MakeCriticalMessage("CardAbility_Critical_1");

                if (!string.IsNullOrEmpty(CARD_ABILITY_ID))
                {
                    if (CARD_ABILITY_ID_LIST.Contains(CARD_ABILITY_ID))
                        CheckDatas.MakeCriticalMessage("CardAbility_Critical_2", CARD_ABILITY_ID);
                    CARD_ABILITY_ID_LIST.Add(CARD_ABILITY_ID);
                }
            });
        }
    }

    class CautionCheck
    {
        public static void CautionCheckAll()
        {

        }
    }
}

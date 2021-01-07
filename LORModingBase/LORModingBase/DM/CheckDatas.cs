﻿using System;
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
            CheckDatas_Critical();
            CheckDatas_Caution();
            return LOG_DATA;
        }

        /// <summary>
        /// Check critical infos
        /// </summary>
        public static void CheckDatas_Critical()
        {
            #region Key Page Check
            string CRITICAL = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Critical");
            List<string> KEY_PAGE_ID_LIST = new List<string>();
            DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.ActionXmlDataNodesByPath("Book", (XmlDataNode bookNode) =>
            {
                string BOOK_NODE_ID = bookNode.GetAttributesSafe("ID");
                string BOOK_NODE_NAME = bookNode.GetInnerTextByPath("Name");

                if (string.IsNullOrEmpty(BOOK_NODE_ID))
                    MakeCriticalMessage("KeyPage_Critical_1", BOOK_NODE_NAME);
                if (string.IsNullOrEmpty(BOOK_NODE_NAME))
                    MakeCriticalMessage("KeyPage_Critical_2", BOOK_NODE_ID);

                if(!string.IsNullOrEmpty(BOOK_NODE_ID))
                {
                    if(KEY_PAGE_ID_LIST.Contains(BOOK_NODE_ID))
                        MakeCriticalMessage("KeyPage_Critical_3", BOOK_NODE_ID);
                    KEY_PAGE_ID_LIST.Add(BOOK_NODE_ID);
                }
            });
            #endregion

            #region Cards Check
            List<string> CARD_PAGE_ID_LIST = new List<string>();
            DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (XmlDataNode bookNode) =>
            {
                string CARD_NODE_ID = bookNode.GetAttributesSafe("ID");
                string CARD_NODE_NAME = bookNode.GetInnerTextByPath("Name");

                if (string.IsNullOrEmpty(CARD_NODE_ID))
                    MakeCriticalMessage("Card_Critical_1", CARD_NODE_NAME);
                if (string.IsNullOrEmpty(CARD_NODE_NAME))
                    MakeCriticalMessage("Card_Critical_2", CARD_NODE_ID);

                if (!string.IsNullOrEmpty(CARD_NODE_ID))
                {
                    if (CARD_PAGE_ID_LIST.Contains(CARD_NODE_ID))
                        MakeCriticalMessage("Card_Critical_3", CARD_NODE_ID);
                    CARD_PAGE_ID_LIST.Add(CARD_NODE_ID);
                }
            });
            #endregion
        }

        /// <summary>
        /// Check caution infos
        /// </summary>
        public static void CheckDatas_Caution()
        {

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
}

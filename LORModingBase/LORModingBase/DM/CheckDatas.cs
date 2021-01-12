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
            CheckCriticalMessageWithSyntaxString();
            return LOG_DATA;
        }

        public static void CheckCriticalMessageWithSyntaxString()
        {
            #region Critical Page check logic
            CheckDatasBySyntaxCheckStrings(
              DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnCriticalPage%").Split('$')[0],
              "Book$ID", DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode,
                new List<string>() { "Book$ID#BDI", "Book/Name#B" ,
                                    "Book/EquipEffect/HP#BI", "Book/EquipEffect/Break#BI",
                                    "Book/EquipEffect/SpeedMin#BI", "Book/EquipEffect/Speed#BI",
                                    "Book/Chapter#BI", "Book/Episode#BI", "Book/BookIcon#B", "Book/CharacterSkin#B"});
            #endregion
            #region Card Page check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnCards%").Split('$')[0],
                "Card$ID", DM.EditGameData_CardInfos.StaticCard.rootDataNode,
                new List<string>() { "Card$ID#BDI", "Card/Name#B",
                                     "Card/Artwork#B", "Card/Spec$Cost#BI",
                                     "Card/BehaviourList/Behaviour$Min#BI", "Card/BehaviourList/Behaviour$Dice#BI"});
            #endregion

            #region Stage check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnStages%").Split('$')[0],
                "Stage$id", DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode,
                new List<string>() { "Stage$id#BDI", "Stage/Name#B",
                                     "Stage/Wave/Formation#BI", "Stage/Wave/Unit#BI",
                                     "Stage/FloorNum#BI", "Stage/Chapter#BI"});
            #endregion
            #region Enemy check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnEnemy%").Split('$')[0],
                "Enemy$ID", DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode,
                new List<string>() { "Enemy$ID#BDI", "Enemy/NameID#B",
                                     "Enemy/MinHeight#BI", "Enemy/MaxHeight#BI",
                                     "Enemy/BookId#BI", "Enemy/DeckId#BI",
                                     "Enemy/DropTable/DropItem#BI"});
            #endregion
            #region Deck check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnDecks%").Split('$')[0],
                "Deck$ID", DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode,
                new List<string>() { "Deck$ID#BD", "Deck/Card#BI" });
            #endregion

            #region DropBook check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnDropBook%").Split('$')[0],
                "BookUse$ID", DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode,
                new List<string>() { "BookUse$ID#BD", "BookUse/TextId#B",
                                     "BookUse/BookIcon#B", "BookUse/Chapter#BI",
                                      "BookUse/DropItem#BI"});
            #endregion
            #region Passive check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnPassive%").Split('$')[0],
                "Passive$ID", DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode,
                new List<string>() { "Passive$ID#BD", "Passive/Cost#BI" });
            #endregion
            #region Card ability check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnCardAbility%").Split('$')[0],
                "BattleCardAbility$ID", DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode,
                new List<string>() { "BattleCardAbility$ID#BD" });
            #endregion

            #region Buff check logic
            CheckDatasBySyntaxCheckStrings(
                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"%BtnBuff%").Split('$')[0],
                "effectTextList/BattleEffectText$ID", DM.EditGameData_Buff.LocalizedBuff.rootDataNode,
                new List<string>() { "effectTextList/BattleEffectText$ID#BD" });
            #endregion
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

        public static void CheckDatasBySyntaxCheckStrings(string menunName, string referencePath, XmlDataNode rootDataNode, List<string> checkSyntaxStrings, bool isCritical = true)
        {
            string REFERENCE_TEXT = "NO ID";
            if (referencePath.Contains('$'))
                rootDataNode.ActionXmlDataNodesByPath(referencePath.Split('$')[0], (XmlDataNode checkNode) =>
                {
                    REFERENCE_TEXT = checkNode.GetAttributesSafe(referencePath.Split('$').Last());
                });
            else
                REFERENCE_TEXT = rootDataNode.GetInnerTextByPath(referencePath);

            checkSyntaxStrings.ForEach((string checkSyntaxString) =>
            {
                if(checkSyntaxString.Split('#').Count() == 2)
                {
                    string CHECK_PATH = checkSyntaxString.Split('#')[0];
                    string CHECK_FUNC = checkSyntaxString.Split('#')[1];

                    List<string> DUPLICATE_CHECK = new List<string>();
                    rootDataNode.ActionXmlDataNodesByPath(CHECK_PATH.Split('$')[0], (XmlDataNode checkNode) =>
                    {
                        string CHECK_TEXT = "";
                        if (CHECK_PATH.Contains('$'))
                            CHECK_TEXT = checkNode.GetAttributesSafe(CHECK_PATH.Split('$').Last());
                        else
                            CHECK_TEXT = checkNode.innerText;

                        if (string.IsNullOrEmpty(CHECK_TEXT) && CHECK_FUNC.Contains("B"))
                        {
                            if(isCritical)
                                CheckDatas.MakeCriticalMessage("ERROR_MESSAGE_01", REFERENCE_TEXT, menunName, CHECK_PATH);
                            else
                                CheckDatas.MakeCautionMessage("ERROR_MESSAGE_01", REFERENCE_TEXT, menunName, CHECK_PATH);
                        }

                        if (!string.IsNullOrEmpty(CHECK_TEXT) && CHECK_FUNC.Contains("D"))
                        {
                            if (DUPLICATE_CHECK.Contains(CHECK_TEXT))
                            {
                                if (isCritical)
                                    CheckDatas.MakeCriticalMessage("ERROR_MESSAGE_02", REFERENCE_TEXT, menunName, CHECK_PATH);
                                else
                                    CheckDatas.MakeCautionMessage("ERROR_MESSAGE_02", REFERENCE_TEXT, menunName, CHECK_PATH);
                            }
                            DUPLICATE_CHECK.Add(CHECK_TEXT);
                        }

                        if (!string.IsNullOrEmpty(CHECK_TEXT) && CHECK_FUNC.Contains("I"))
                        {
                            if (!Tools.CheckInput.IsIntegerInputed(CHECK_TEXT))
                            {
                                if (isCritical)
                                    CheckDatas.MakeCriticalMessage("ERROR_MESSAGE_03", REFERENCE_TEXT, menunName, CHECK_PATH);
                                else
                                    CheckDatas.MakeCautionMessage("ERROR_MESSAGE_03", REFERENCE_TEXT, menunName, CHECK_PATH);
                            }
                        }

                        if (!string.IsNullOrEmpty(CHECK_TEXT) && CHECK_FUNC.Contains("D"))
                        {
                            if (!Tools.CheckInput.IsDoubleInputed(CHECK_TEXT))
                            {
                                if (isCritical)
                                    CheckDatas.MakeCriticalMessage("ERROR_MESSAGE_04", REFERENCE_TEXT, menunName, CHECK_PATH);
                                else
                                    CheckDatas.MakeCautionMessage("ERROR_MESSAGE_04", REFERENCE_TEXT, menunName, CHECK_PATH);
                            }
                        }
                    });
                }
            });
        }
    }
}

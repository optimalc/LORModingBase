using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Static infos for card datas
    /// </summary>
    partial class GameInfos
    {
        #region Card effects data load
        /// <summary>
        /// Card effect dic - [effectID, localized description]
        /// </summary>
        public static Dictionary<string, List<string>> cardEffectDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// Load localized card effects
        /// </summary>
        public static void LoadData_CardEffect()
        {
            cardEffectDic.Clear();
            string battleCardEffectPath = $"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\BattleCardAbilities\\BattleCardAbilities.txt";
            XmlNodeList battleCardAbilityNodes = Tools.XmlFile.SelectNodeLists(battleCardEffectPath, "//BattleCardAbility");
            foreach (XmlNode battleCardAbilityNode in battleCardAbilityNodes)
            {
                #region ID Check
                if (battleCardAbilityNode.Attributes["ID"] == null
                    || string.IsNullOrEmpty(battleCardAbilityNode.Attributes["ID"].Value))
                    continue;
                #endregion

                List<string> desc = new List<string>();
                #region Make localized description
                XmlNodeList descNodes = battleCardAbilityNode.SelectNodes("Desc");
                foreach (XmlNode descNode in descNodes)
                {
                    if (!string.IsNullOrEmpty(descNode.InnerText))
                        desc.Add(descNode.InnerText);
                }
                #endregion
                cardEffectDic[battleCardAbilityNode.Attributes["ID"].Value] = desc;
            }
        }
        #endregion


        #region Card info data load
        /// <summary>
        /// Localized name dic - [cardID, Localized card name]
        /// </summary>
        public static Dictionary<string, string> gameCardLocalized = new Dictionary<string, string>();

        /// <summary>
        /// Critical page infos in game
        /// </summary>
        public static List<DS.CardInfo> gameCardInfos = new List<DS.CardInfo>();

        /// <summary>
        /// Load card information
        /// </summary>
        public static void LoadData_CardsInfo()
        {
            #region Make localized dictionary list
            gameCardLocalized.Clear();
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\BattlesCards").ToList().ForEach((string battleCardsInfo) =>
            {
                XmlNodeList battleCardsDesc = Tools.XmlFile.SelectNodeLists(battleCardsInfo, "//BattleCardDesc");
                foreach (XmlNode battleCardDesc in battleCardsDesc)
                {
                    #region Check ID and Localized card name empty
                    if (battleCardDesc.Attributes["ID"] == null || battleCardDesc["LocalizedName"] == null
                        || string.IsNullOrEmpty(battleCardDesc["LocalizedName"].InnerText))
                        continue;
                    #endregion
                    gameCardLocalized[battleCardDesc.Attributes["ID"].Value] = battleCardDesc["LocalizedName"].InnerText;
                }
            });
            #endregion
            #region Make card infos
            gameCardInfos.Clear();
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\Card").ToList().ForEach((string cardInfoPath) =>
            {
                XmlNodeList cardNodeList = Tools.XmlFile.SelectNodeLists(cardInfoPath, "//Card");
                foreach (XmlNode cardNode in cardNodeList)
                {
                    #region Check if card ID attribute exist
                    if (cardNode.Attributes["ID"] == null
                        || string.IsNullOrEmpty(cardNode.Attributes["ID"].Value))
                        continue;
                    #endregion

                    DS.CardInfo cardInfo = new DS.CardInfo();
                    #region Get basic infos (id, name, rarity, cardScript, option, chapter)
                    cardInfo.cardID = cardNode.Attributes["ID"].Value;
                    if (gameCardLocalized.ContainsKey(cardInfo.cardID))
                        cardInfo.name = gameCardLocalized[cardInfo.cardID];

                    cardInfo.rarity = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Rarity", "Common");
                    cardInfo.cardScript = GetDescriptionForCard.GetScriptDescription(Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Script"));

                    cardInfo.option = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Option");
                    cardInfo.chapter = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Chapter", "1");
                    #endregion

                    #region Get artwork description
                    string chapterDes = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, "noChapter");
                    if (DS.GameInfo.chapter_Dic.ContainsKey(cardInfo.chapter))
                        chapterDes = DS.GameInfo.chapter_Dic[cardInfo.chapter];

                    string IMAGE_DES = $"{cardInfo.name}:{chapterDes}:{Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Artwork")}";
                    cardInfo.cardImage = IMAGE_DES;
                    #endregion

                    #region Get data from Spec node attribute (rangeType, cost, emotionLimit, affection)
                    cardInfo.rangeType = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "Range", "Near");
                    cardInfo.cost = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "Cost", "1");

                    cardInfo.EXTRA_EmotionLimit = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "EmotionLimit");
                    cardInfo.EXTRA_Affection = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "Affection");
                    #endregion
                    #region Get semi datas (priority, priorityScript, sortPriority)
                    cardInfo.priority = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Priority", "1");
                    cardInfo.priorityScript = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "PriorityScript");
                    cardInfo.sortPriority = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "SortPriority", "1");
                    #endregion

                    #region Get dice lists
                    XmlNodeList behaviourNodes = cardNode.SelectNodes("BehaviourList/Behaviour");
                    foreach (XmlNode behaviourNode in behaviourNodes)
                    {
                        cardInfo.dices.Add(new DS.Dice()
                        {
                            min = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Min"),
                            max = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Dice"),
                            type = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Type"),
                            detail = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Detail"),
                            motion = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Motion"),
                            script = GetDescriptionForCard.GetScriptDescription(Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Script")),
                            actionScript = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "ActionScript"),
                            effectres = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "EffectRes")
                        });
                    }
                    #endregion
                    gameCardInfos.Add(cardInfo);
                }
            });
            #endregion
        }
        #endregion


        #region Dropbooks info data load
        /// <summary>
        /// Drop book dic - [bookID, cardIDListToDrop]
        /// </summary>
        public static Dictionary<string, List<string>> dropBookDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// Load drop book data
        /// </summary>
        public static void LoadData_Dropbooks()
        {
            dropBookDic.Clear();
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\CardDropTable").ToList().ForEach((string dropBookPath) =>
            {
                XmlNodeList dropBookNodes = Tools.XmlFile.SelectNodeLists(dropBookPath, "//DropTable");
                foreach (XmlNode dropBookNode in dropBookNodes)
                {
                    #region Check if drop book table ID is null or empty
                    if (dropBookNode.Attributes["ID"] == null
                             || string.IsNullOrEmpty(dropBookNode.Attributes["ID"].Value))
                        continue;
                    #endregion

                    List<string> dropBookCards = new List<string>();
                    XmlNodeList dropBookCardNodes = dropBookNode.SelectNodes("Card");
                    foreach (XmlNode dropBookCardNode in dropBookCardNodes)
                    {
                        #region Update dorp book cards info
                        if (string.IsNullOrEmpty(dropBookCardNode.InnerText))
                            continue;
                        dropBookCards.Add(dropBookCardNode.InnerText);
                        #endregion
                        #region Update game Cord Info dropbooks info
                        foreach (DS.CardInfo gameCardInfo in gameCardInfos)
                        {
                            if (gameCardInfo.cardID == dropBookCardNode.InnerText)
                            {
                                string BOOK_TO_DROP = dropBookNode.Attributes["ID"].Value;
                                if (!gameCardInfo.dropBooks.Contains(BOOK_TO_DROP))
                                    gameCardInfo.dropBooks.Add(GetDescriptionForBook.GetBookChapterDescription(BOOK_TO_DROP));
                            }
                        }
                        #endregion
                    }

                    if (dropBookCards.Count <= 0) continue;
                    dropBookDic[dropBookNode.Attributes["ID"].Value] = dropBookCards;
                }
            });
        }
        #endregion


        /// <summary>
        /// Get description for card
        /// </summary>
        public class GetDescriptionForCard
        {
            /// <summary>
            /// Get artwork description
            /// </summary>
            /// <param name="artworkName">Artwork name to use</param>
            /// <returns>DES</returns>
            public static string GetArtworkDescription(string artworkName)
            {
                if (!string.IsNullOrEmpty(artworkName))
                {
                    DS.CardInfo foundCardInfo = DM.GameInfos.gameCardInfos.Find((DS.CardInfo cardInfo) =>
                    {
                        return cardInfo.cardImage.Split(':').Last() == artworkName;
                    });
                    if (foundCardInfo != null)
                        return $"{foundCardInfo.name}:{foundCardInfo.cardImage}";
                    else
                        return $"커스텀 이미지:{artworkName}";
                }
                else
                    return "";
            }

            /// <summary>
            /// Get card/dice script description
            /// </summary>
            /// <param name="scriptName">Script name to use</param>
            /// <returns></returns>
            public static string GetScriptDescription(string scriptName)
            {
                if (!string.IsNullOrEmpty(scriptName))
                {
                    if (cardEffectDic.ContainsKey(scriptName))
                        return $"{String.Join(" ", cardEffectDic[scriptName].ToArray())}:{scriptName}";
                    else
                        return $"커스텀 효과:{scriptName}";
                }
                else
                    return "";
            }
        }
    }
}

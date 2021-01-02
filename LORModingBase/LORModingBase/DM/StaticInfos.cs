using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game episode data managerment
    /// </summary>
    class StaticInfos
    {
        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadData_CardsInfo();

            LoadDatas_PassiveInfo();
            LoadDatas_StageInfo();
            LoadDatas_SkinAndBookIconInfo();
        }

        #region Load datas for critical page
        /// <summary>
        /// Loaded stage infos
        /// </summary>
        public static List<DS.StageInfo> stageInfos = new List<DS.StageInfo>();

        /// <summary>
        /// Loaded passive infos
        /// </summary>
        public static Dictionary<string, List<DS.PassiveInfo>> passiveInfos = new Dictionary<string, List<DS.PassiveInfo>>();
        public static List<string> passiveList = new List<string>();

        /// <summary>
        /// Book skin infos
        /// </summary>
        public static List<DS.BookSkinInfo> bookSkinInfos = new List<DS.BookSkinInfo>();
        /// <summary>
        /// Critical page infos in game
        /// </summary>
        public static List<DS.CriticalPageInfo> gameCriticalPageInfos = new List<DS.CriticalPageInfo>();

        /// <summary>
        /// Book icon infos
        /// </summary>
        public static List<DS.DropBookInfo> dropBookInfos = new List<DS.DropBookInfo>();


        /// <summary>
        /// Load stage info datas
        /// </summary>
        public static void LoadDatas_StageInfo()
        {
            stageInfos.Clear();
            XmlNodeList stageNodeList = Tools.XmlFile.SelectNodeLists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\StageInfo\\StageInfo.txt",
                "//Stage");

            foreach (XmlNode stageNode in stageNodeList)
            {
                if (stageNode.Attributes["id"] == null
                    || stageNode["Chapter"] == null
                    || stageNode["Name"] == null)
                    continue;

                stageInfos.Add(new DS.StageInfo()
                {
                    stageID = stageNode.Attributes["id"].Value,
                    Chapter = stageNode["Chapter"].InnerText,
                    stageDoc = stageNode["Name"].InnerText
                });
            }
        }

        /// <summary>
        /// Load skin and book icon datas
        /// </summary>
        public static void LoadDatas_SkinAndBookIconInfo()
        {
            #region Load book icon infos
            dropBookInfos.Clear();
            string dropBookInfoPath = $"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\etc\\KR_Dropbook.txt";
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\DropBook").ToList().ForEach((string dropBookPath) =>
            {
                XmlNodeList bookUseNodeList = Tools.XmlFile.SelectNodeLists(dropBookPath, "//BookUse");
                foreach (XmlNode bookUseNode in bookUseNodeList)
                {
                    if (bookUseNode.Attributes["ID"] == null || bookUseNode["TextId"] == null || bookUseNode["BookIcon"] == null || bookUseNode["Chapter"] == null)
                        continue;
                    if (string.IsNullOrEmpty(bookUseNode["BookIcon"].InnerText))
                        continue;

                    XmlNode dropBookInfoNode = Tools.XmlFile.SelectSingleNode(dropBookInfoPath, $"//text[@id='{bookUseNode["TextId"].InnerText}']");
                    if (dropBookInfoNode == null)
                        continue;

                    XmlNodeList dropItems = bookUseNode.SelectNodes("DropItem");
                    List<string> dropItemList = new List<string>();
                    foreach (XmlNode dropItem in dropItems)
                    {
                        if (string.IsNullOrEmpty(dropItem.InnerText)) continue;
                        dropItemList.Add(dropItem.InnerText);
                    }

                    dropBookInfos.Add(new DS.DropBookInfo()
                    {
                        iconName = bookUseNode["BookIcon"].InnerText,
                        iconDesc = dropBookInfoNode.InnerText,
                        chapter = bookUseNode["Chapter"].InnerText,
                        bookID = bookUseNode.Attributes["ID"].Value,
                        dropItems = dropItemList
                    });
                }
            });
            #endregion
            #region Load skin icon infos
            bookSkinInfos.Clear();
            gameCriticalPageInfos.Clear();

            XmlNode booksDesNode = Tools.XmlFile.SelectSingleNode($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\Books\\_Books.txt", "//bookDescList");
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\EquipPage").ToList().ForEach((string eqPath) =>
            {
                XmlNodeList bookNodeList = Tools.XmlFile.SelectNodeLists(eqPath, "//Book");

                foreach (XmlNode bookNode in bookNodeList)
                {
                    #region Add skin infos
                    if (bookNode["CharacterSkin"] == null)
                        continue;
                    if (string.IsNullOrEmpty(bookNode["CharacterSkin"].InnerText))
                        continue;

                    bookSkinInfos.Add(new DS.BookSkinInfo()
                    {
                        skinDesc = (bookNode["Name"] == null) ? "" : bookNode["Name"].InnerText,
                        skinName = bookNode["CharacterSkin"].InnerText,
                        chapter = (bookNode["Chapter"] == null) ? "" : bookNode["Chapter"].InnerText
                    });
                    #endregion
                    #region Add equiment load infos
                    DS.CriticalPageInfo criticalPageInfo = new DS.CriticalPageInfo();
                    criticalPageInfo.bookID = (bookNode.Attributes["ID"] == null) ? "" : bookNode.Attributes["ID"].Value;
                    if (string.IsNullOrEmpty(criticalPageInfo.bookID))
                        continue;

                    criticalPageInfo.name = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Name");
                    criticalPageInfo.rangeType = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "RangeType", "Nomal");
                    criticalPageInfo.rarity = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Rarity");

                    criticalPageInfo.chapter = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Chapter");
                    criticalPageInfo.episode = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Episode");
                    criticalPageInfo.episodeDes = GetDescription.GetEpisodeDescription(criticalPageInfo.episode, criticalPageInfo.chapter);

                    criticalPageInfo.iconName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "BookIcon");
                    criticalPageInfo.iconDes = GetDescription.GetIconDescription(criticalPageInfo.iconName);

                    criticalPageInfo.skinName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "CharacterSkin");
                    criticalPageInfo.skinDes = GetDescription.GetSkinDescription(criticalPageInfo.skinName);

                    XmlNode equipEffectNode = bookNode.SelectSingleNode("EquipEffect");
                    criticalPageInfo.HP = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "HP");
                    criticalPageInfo.breakNum = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "Break");

                    criticalPageInfo.minSpeedCount = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "SpeedMin");
                    criticalPageInfo.maxSpeedCount = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "Speed");

                    criticalPageInfo.SResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "SResist");
                    criticalPageInfo.PResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "PResist");
                    criticalPageInfo.HResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "HResist");

                    criticalPageInfo.BSResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "SBResist");
                    criticalPageInfo.BPResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "PBResist");
                    criticalPageInfo.BHResist = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "HBResist");

                    #region 적 관련 정보 불러오기
                    criticalPageInfo.ENEMY_StartPlayPoint = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "StartPlayPoint");
                    criticalPageInfo.ENEMY_MaxPlayPoint = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "MaxPlayPoint");
                    criticalPageInfo.ENEMY_EmotionLevel = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "EmotionLevel");
                    criticalPageInfo.ENEMY_AddedStartDraw = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "AddedStartDraw");
                    #endregion
                    #region 전용책장 관련 정보 불러오기
                    XmlNodeList onlyCardNodes = equipEffectNode.SelectNodes("OnlyCard");
                    foreach (XmlNode onlyCardNode in onlyCardNodes)
                    {
                        if (!string.IsNullOrEmpty(onlyCardNode.InnerText))
                            criticalPageInfo.onlyCards.Add(DM.StaticInfos.GetDescription.GetUniqueCardDescription(onlyCardNode.InnerText));
                    }
                    #endregion

                    XmlNodeList passiveNodes = equipEffectNode.SelectNodes("Passive");
                    foreach (XmlNode passiveNode in passiveNodes)
                    {
                        if (string.IsNullOrEmpty(passiveNode.InnerText))
                            continue;

                        criticalPageInfo.passiveIDs.Add(GetDescription.GetPassiveDescription(passiveNode.InnerText));
                    }

                    #region Get description of ciritical page
                    string BOOK_ID_DESC = criticalPageInfo.bookID;
                    int BOOK_ID = Convert.ToInt32(criticalPageInfo.bookID);
                    if (BOOK_ID > DS.FilterDatas.CRITICAL_PAGE_DIV_LIBRARION && BOOK_ID < DS.FilterDatas.CRITICAL_PAGE_DIV_ENEMY)
                        BOOK_ID_DESC = (BOOK_ID + 100000).ToString();

                    XmlNode bookDescNode = booksDesNode.SelectSingleNode($"//BookDesc[@BookID='{BOOK_ID_DESC}']");
                    if (bookDescNode != null)
                    {
                        XmlNodeList descNodes = bookDescNode.SelectNodes("TextList/Desc");
                        if (descNodes.Count > 0)
                            criticalPageInfo.description = "";

                        for (int descNodeIndex = 0; descNodeIndex < descNodes.Count; descNodeIndex++)
                        {
                            if (descNodeIndex > 0)
                                criticalPageInfo.description += "\r\n\r\n";
                            criticalPageInfo.description += descNodes[descNodeIndex].InnerText;
                        }
                        criticalPageInfo.description = criticalPageInfo.description.Replace(". ", ". \n");
                    }
                    #endregion
                    #region Get dropbook info of ciritical page
                    dropBookInfos.ForEach((DS.DropBookInfo dropBookInfo) =>
                    {
                        if (dropBookInfo.dropItems.Contains(criticalPageInfo.bookID))
                            criticalPageInfo.dropBooks.Add($"{dropBookInfo.iconDesc}:{dropBookInfo.bookID}");
                    });
                    #endregion

                    gameCriticalPageInfos.Add(criticalPageInfo);
                    #endregion
                }
            });
            #endregion

        }

        /// <summary>
        /// Load passive info datas
        /// </summary>
        public static void LoadDatas_PassiveInfo()
        {
            passiveInfos.Clear();
            passiveList.Clear();

            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\PassiveDesc").ToList().ForEach((string pvPath) =>
            {
                XmlNodeList passiveDescNodeList = Tools.XmlFile.SelectNodeLists(pvPath, "//PassiveDesc");

                List<DS.PassiveInfo> passives = new List<DS.PassiveInfo>();
                foreach (XmlNode passiveDescNode in passiveDescNodeList)
                {
                    if (passiveDescNode.Attributes["ID"] == null
                        || passiveDescNode["Name"] == null
                        || passiveDescNode["Desc"] == null)
                        continue;

                    passives.Add(new DS.PassiveInfo()
                    {
                        passiveID = passiveDescNode.Attributes["ID"].Value,
                        passiveName = passiveDescNode["Name"].InnerText,
                        passiveDes = passiveDescNode["Desc"].InnerText
                    });
                    passiveList.Add($"{passiveDescNode["Name"].InnerText}:{passiveDescNode["Desc"].InnerText}:{passiveDescNode.Attributes["ID"].Value}");
                }
                string PATH_TO_USE = pvPath.Split('\\').Last().Split('.')[0];
                if (passives.Count > 0) passiveInfos[PATH_TO_USE] = passives;
            });
        }

        /// <summary>
        /// Get correct description
        /// </summary>
        public class GetDescription
        {
            /// <summary>
            /// Get episode description for given ID
            /// </summary>
            /// <param name="episodeID">Episode ID to use</param>
            /// <param name="chapterToUse">Chapter If not found</param>
            /// <returns>DES</returns>
            public static string GetEpisodeDescription(string episodeID, string chapterToUse)
            {
                if (!string.IsNullOrEmpty(episodeID))
                {
                    DS.StageInfo foundStageInfo = DM.StaticInfos.stageInfos.Find((DS.StageInfo stageInfo) =>
                    {
                        return stageInfo.stageID == episodeID;
                    });
                    if (foundStageInfo != null)
                        return $"{DS.GameInfo.chapter_Dic[foundStageInfo.Chapter]} / {foundStageInfo.stageDoc}:{foundStageInfo.stageID}";
                    else
                        return $"{DS.GameInfo.chapter_Dic[chapterToUse]} / 커스텀 스테이지:{episodeID}";
                }
                else
                    return "";
            }

            /// <summary>
            /// Get skin description for given Name
            /// </summary>
            /// <param name="skinName">Skin name to use</param>
            /// <returns>DES</returns>
            public static string GetSkinDescription(string skinName)
            {
                if (!string.IsNullOrEmpty(skinName))
                {
                    DS.BookSkinInfo foundSkinInfo = DM.StaticInfos.bookSkinInfos.Find((DS.BookSkinInfo skinInfo) =>
                    {
                        return skinInfo.skinName == skinName;
                    });
                    if (foundSkinInfo != null)
                        return $"{foundSkinInfo.skinDesc}:{foundSkinInfo.skinName}";
                    else
                        return $"커스텀 스킨:{skinName}";
                }
                else
                    return "";
            }

            /// <summary>
            /// Get icon description for given name
            /// </summary>
            /// <param name="iconName">Icon name to use</param>
            /// <returns>DES</returns>
            public static string GetIconDescription(string iconName)
            {
                if (!string.IsNullOrEmpty(iconName))
                {
                    DS.DropBookInfo foundDropBookInfo = DM.StaticInfos.dropBookInfos.Find((DS.DropBookInfo dropInfo) =>
                    {
                        return dropInfo.iconName == iconName;
                    });
                    if (foundDropBookInfo != null)
                        return $"{foundDropBookInfo.iconDesc}:{foundDropBookInfo.iconName}";
                    else
                        return $"커스텀 아이콘:{iconName}";
                }
                else
                    return "";
            }

            /// <summary>
            /// Get passive description
            /// </summary>
            /// <param name="passiveID">Passive ID to use</param>
            /// <returns>DES</returns>
            public static string GetPassiveDescription(string passiveID)
            {
                string foundPassiveDesc = DM.StaticInfos.passiveList.Find((string passiveDesc) =>
                {
                    return passiveDesc.Split(':').Last() == passiveID;
                });
                if (!string.IsNullOrEmpty(foundPassiveDesc))
                    return foundPassiveDesc;
                else
                    return $"커스텀:커스텀 패시브:{passiveID}";
            }
        
            /// <summary>
            /// Get unique card description
            /// </summary>
            /// <param name="cardID">Card id to use</param>
            /// <returns>DES</returns>
            public static string GetUniqueCardDescription(string cardID)
            {
                DS.CardInfo foundCardInfo = DM.StaticInfos.gameCardInfos.Find((DS.CardInfo cardInfo) =>
                {
                    return cardInfo.cardID == cardID;
                });
                if (foundCardInfo != null)
                    return $"{foundCardInfo.name}:{foundCardInfo.cardID}";
                else
                    return $"커스텀 카드:{cardID}";
            }
        }
        #endregion
        #region Load datas for card
        /// <summary>
        /// Critical page infos in game
        /// </summary>
        public static List<DS.CardInfo> gameCardInfos = new List<DS.CardInfo>();
        /// <summary>
        /// Localized name for each card ID
        /// </summary>
        public static Dictionary<string, string> gameCardLocalized = new Dictionary<string, string>();

        public static void LoadData_CardsInfo()
        {
            #region Make localized dictionary list
            gameCardLocalized.Clear();
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\BattlesCards").ToList().ForEach((string battleCardsInfo) =>
            {
                XmlNodeList battleCardsDesc = Tools.XmlFile.SelectNodeLists(battleCardsInfo, "//BattleCardDesc");
                foreach (XmlNode battleCardDesc in battleCardsDesc)
                {
                    if (battleCardDesc.Attributes["ID"] == null || battleCardDesc["LocalizedName"] == null)
                        continue;
                    if (string.IsNullOrEmpty(battleCardDesc["LocalizedName"].InnerText))
                        continue;

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
                    if (cardNode.Attributes["ID"] == null)
                        continue;
                    if (string.IsNullOrEmpty(cardNode.Attributes["ID"].Value))
                        continue;


                    DS.CardInfo cardInfo = new DS.CardInfo();
                    cardInfo.cardID = cardNode.Attributes["ID"].Value;
                    if(gameCardLocalized.ContainsKey(cardInfo.cardID))
                        cardInfo.name = gameCardLocalized[cardInfo.cardID];

                    cardInfo.rarity = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Rarity", "Common");
                    cardInfo.cardScript = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Script");

                    cardInfo.option = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Option");
                    cardInfo.cardImage = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Artwork");

                    cardInfo.rangeType = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "Range", "Near");
                    cardInfo.cost = Tools.XmlFile.GetAttributeSafeWithXPath.ToString(cardNode, "Spec", "Cost", "1");

                    cardInfo.chapter = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Chapter");
                    cardInfo.priority = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "Priority");
                    cardInfo.sortPriority = Tools.XmlFile.GetXmlNodeSafe.ToString(cardNode, "SortPriority");

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
                            script = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "Script"),
                            actionScript = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "ActionScript"),
                            effectres = Tools.XmlFile.GetAttributeSafe.ToString(behaviourNode, "EffectRes")
                        });
                    }

                    gameCardInfos.Add(cardInfo);
                }
            }); 
            #endregion
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    partial class GameInfos
    {
        #region Passive info to load
        /// <summary>
        /// Loaded passive dic [Passive file path, Passive info list]
        /// </summary>
        public static Dictionary<string, List<DS.PassiveInfo>> passiveInfos = new Dictionary<string, List<DS.PassiveInfo>>();
        /// <summary>
        /// Passive description list ($NAME:$DESC:$ID)
        /// </summary>
        public static List<string> passiveList = new List<string>();

        /// <summary>
        /// Load passive info datas
        /// </summary>
        public static void LoadDatas_PassiveInfo()
        {
            passiveInfos.Clear();
            passiveList.Clear();

            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\PassiveDesc").ToList().ForEach((string pvPath) =>
            {
                XmlNodeList passiveDescNodeList = Tools.XmlFile.SelectNodeLists(pvPath, "//PassiveDesc");

                List<DS.PassiveInfo> passives = new List<DS.PassiveInfo>();
                foreach (XmlNode passiveDescNode in passiveDescNodeList)
                {
                    #region Check each passive datas
                    if (passiveDescNode.Attributes["ID"] == null
                        || passiveDescNode["Name"] == null
                        || passiveDescNode["Desc"] == null)
                        continue;
                    #endregion
                    #region Add passive datas
                    passives.Add(new DS.PassiveInfo()
                    {
                        passiveID = passiveDescNode.Attributes["ID"].Value,
                        passiveName = passiveDescNode["Name"].InnerText,
                        passiveDes = passiveDescNode["Desc"].InnerText
                    });
                    passiveList.Add($"{passiveDescNode["Name"].InnerText}:{passiveDescNode["Desc"].InnerText}:{passiveDescNode.Attributes["ID"].Value}");
                    #endregion
                }
                string PATH_TO_USE = pvPath.Split('\\').Last().Split('.')[0];
                if (passives.Count > 0) passiveInfos[PATH_TO_USE] = passives;
            });
        }
        #endregion


        #region Stage info to load
        /// <summary>
        /// Loaded stage infos
        /// </summary>
        public static List<DS.StageInfo> stageInfos = new List<DS.StageInfo>();

        /// <summary>
        /// Load stage info datas
        /// </summary>
        public static void LoadDatas_StageInfo()
        {
            stageInfos.Clear();
            XmlNodeList stageNodeList = Tools.XmlFile.SelectNodeLists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\StageInfo\\StageInfo.txt",
                "//Stage");
            XmlNode stageNameNode = Tools.XmlFile.SelectSingleNode($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\StageName\\_StageName.txt",
              "//CharactersNameRoot");

            foreach (XmlNode stageNode in stageNodeList)
            {
                #region Check if stage id, chapter exists
                if (stageNode.Attributes["id"] == null
                   || stageNode["Chapter"] == null
                   || string.IsNullOrEmpty(stageNode.Attributes["id"].Value))
                    continue;
                #endregion
                #region Get localized stage name and add stage info to list
                XmlNode stageLocalizedNode = stageNameNode.SelectSingleNode($"Name[@ID='{stageNode.Attributes["id"].Value}']");
                if (stageLocalizedNode != null
                    && !string.IsNullOrEmpty(stageLocalizedNode.InnerText))
                {
                    stageInfos.Add(new DS.StageInfo()
                    {
                        stageID = stageNode.Attributes["id"].Value,
                        Chapter = stageNode["Chapter"].InnerText,
                        stageDoc = stageLocalizedNode.InnerText
                    });
                }
                #endregion
            }
        }
        #endregion


        #region Load dropbook, Skin, CriticalPageInfos
        /// <summary>
        /// Book icon infos
        /// </summary>
        public static List<DS.DropBookInfo> dropBookInfos = new List<DS.DropBookInfo>();

        /// <summary>
        /// Book skin infos
        /// </summary>
        public static List<DS.BookSkinInfo> bookSkinInfos = new List<DS.BookSkinInfo>();
        /// <summary>
        /// Critical page infos in game
        /// </summary>
        public static List<DS.CriticalPageInfo> gameCriticalPageInfos = new List<DS.CriticalPageInfo>();

        /// <summary>
        /// Load skin and book icon datas
        /// </summary>
        public static void LoadDatas_SkinAndBookIconInfo()
        {
            #region Load drop book infos
            dropBookInfos.Clear();
            string dropBookInfoPath = $"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\etc\\{DM.Config.config.localizeOption.ToUpper()}_Dropbook.txt";
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\DropBook").ToList().ForEach((string dropBookPath) =>
            {
                XmlNodeList bookUseNodeList = Tools.XmlFile.SelectNodeLists(dropBookPath, "//BookUse");
                foreach (XmlNode bookUseNode in bookUseNodeList)
                {
                    #region Check DropBookNode (ID, TextId, BookIcon, Chapter)
                    if (bookUseNode.Attributes["ID"] == null
                                   || bookUseNode["TextId"] == null
                                   || bookUseNode["BookIcon"] == null
                                   || bookUseNode["Chapter"] == null
                                   || string.IsNullOrEmpty(bookUseNode["BookIcon"].InnerText))
                        continue;
                    #endregion
                    XmlNode dropBookInfoNode = Tools.XmlFile.SelectSingleNode(dropBookInfoPath, $"//text[@id='{bookUseNode["TextId"].InnerText}']");
                    if (dropBookInfoNode == null)
                        continue;

                    #region Make drop item list -> dropItemList
                    List<string> dropItemList = new List<string>();
                    XmlNodeList dropItems = bookUseNode.SelectNodes("DropItem");
                    foreach (XmlNode dropItem in dropItems)
                    {
                        if (string.IsNullOrEmpty(dropItem.InnerText)) continue;
                        dropItemList.Add(dropItem.InnerText);
                    }
                    #endregion
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
            #region Load critical page infos. Plus, load skin icon infos
            bookSkinInfos.Clear();
            gameCriticalPageInfos.Clear();

            XmlNode booksDesNode = Tools.XmlFile.SelectSingleNode($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\{DM.Config.config.localizeOption}\\Books\\_Books.txt", "//bookDescList");
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\EquipPage").ToList().ForEach((string eqPath) =>
            {
                XmlNodeList bookNodeList = Tools.XmlFile.SelectNodeLists(eqPath, "//Book");

                foreach (XmlNode bookNode in bookNodeList)
                {
                    if (bookNode["CharacterSkin"] == null || bookNode.Attributes["ID"] == null
                        || bookNode["TextId"] == null || string.IsNullOrEmpty(bookNode["CharacterSkin"].InnerText))
                        continue;

                    #region Get localized bookName -> localizedBookDesc["BookName"].InnerText
                    XmlNode localizedBookDesc = booksDesNode.SelectSingleNode($"//BookDesc[@BookID='{bookNode["TextId"].InnerText}']");
                    if (localizedBookDesc == null || localizedBookDesc["BookName"] == null)
                        continue;
                    #endregion
                    #region Add skin infos
                    bookSkinInfos.Add(new DS.BookSkinInfo()
                    {
                        skinDesc = localizedBookDesc["BookName"].InnerText,
                        skinName = bookNode["CharacterSkin"].InnerText,
                        chapter = (bookNode["Chapter"] == null) ? "" : bookNode["Chapter"].InnerText
                    });
                    #endregion

                    #region Add Critical book load infos
                    DS.CriticalPageInfo criticalPageInfo = new DS.CriticalPageInfo();
                    #region Load basic info (ID, BookName, RangeType, Rarity)
                    criticalPageInfo.bookID = bookNode.Attributes["ID"].Value;
                    criticalPageInfo.name = localizedBookDesc["BookName"].InnerText;
                    criticalPageInfo.rangeType = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "RangeType", "Nomal");
                    criticalPageInfo.rarity = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Rarity");
                    #endregion

                    #region Load chapterDes, iconDes, skinDes
                    criticalPageInfo.chapter = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Chapter");
                    criticalPageInfo.episode = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Episode");
                    criticalPageInfo.episodeDes = GetDescriptionForBook.GetEpisodeDescription(criticalPageInfo.episode, criticalPageInfo.chapter);

                    criticalPageInfo.iconName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "BookIcon");
                    criticalPageInfo.iconDes = GetDescriptionForBook.GetIconDescription(criticalPageInfo.iconName);

                    criticalPageInfo.skinName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "CharacterSkin");
                    criticalPageInfo.skinDes = GetDescriptionForBook.GetSkinDescription(criticalPageInfo.skinName);
                    #endregion
                    #region Load equipt effect node data
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
                    #endregion

                    #region Load enemy related data
                    criticalPageInfo.ENEMY_StartPlayPoint = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "StartPlayPoint");
                    criticalPageInfo.ENEMY_MaxPlayPoint = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "MaxPlayPoint");
                    criticalPageInfo.ENEMY_EmotionLevel = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "EmotionLevel");
                    criticalPageInfo.ENEMY_AddedStartDraw = Tools.XmlFile.GetXmlNodeSafe.ToString(equipEffectNode, "AddedStartDraw");
                    #endregion
                    #region Load only card infos
                    XmlNodeList onlyCardNodes = equipEffectNode.SelectNodes("OnlyCard");
                    foreach (XmlNode onlyCardNode in onlyCardNodes)
                    {
                        if (!string.IsNullOrEmpty(onlyCardNode.InnerText))
                            criticalPageInfo.onlyCards.Add(DM.GameInfos.GetDescriptionForBook.GetUniqueCardDescription(onlyCardNode.InnerText));
                    }
                    #endregion

                    #region Load passive datas
                    XmlNodeList passiveNodes = equipEffectNode.SelectNodes("Passive");
                    foreach (XmlNode passiveNode in passiveNodes)
                    {
                        if (string.IsNullOrEmpty(passiveNode.InnerText))
                            continue;

                        criticalPageInfo.passiveIDs.Add(GetDescriptionForBook.GetPassiveDescription(passiveNode.InnerText));
                    }
                    #endregion

                    #region Get description of ciritical page
                    if (localizedBookDesc != null)
                    {
                        XmlNodeList descNodes = localizedBookDesc.SelectNodes("TextList/Desc");
                        if (descNodes.Count > 0)
                        {
                            criticalPageInfo.description = "";

                            for (int descNodeIndex = 0; descNodeIndex < descNodes.Count; descNodeIndex++)
                            {
                                if (descNodeIndex > 0)
                                    criticalPageInfo.description += "\r\n\r\n";
                                criticalPageInfo.description += descNodes[descNodeIndex].InnerText;
                            }
                            criticalPageInfo.description = criticalPageInfo.description.Replace(". ", ". \n");

                            if (string.IsNullOrEmpty(criticalPageInfo.description))
                                criticalPageInfo.description = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, "noDescription");
                        }
                        else
                            criticalPageInfo.description = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, "noDescription");
                    }
                    else
                        criticalPageInfo.description = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, "noDescription");
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
        #endregion


        /// <summary>
        /// Get description for book
        /// </summary>
        public class GetDescriptionForBook
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
                    DS.StageInfo foundStageInfo = DM.GameInfos.stageInfos.Find((DS.StageInfo stageInfo) =>
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
                    DS.BookSkinInfo foundSkinInfo = DM.GameInfos.bookSkinInfos.Find((DS.BookSkinInfo skinInfo) =>
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
                    DS.DropBookInfo foundDropBookInfo = DM.GameInfos.dropBookInfos.Find((DS.DropBookInfo dropInfo) =>
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
                string foundPassiveDesc = DM.GameInfos.passiveList.Find((string passiveDesc) =>
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
                DS.CardInfo foundCardInfo = DM.GameInfos.gameCardInfos.Find((DS.CardInfo cardInfo) =>
                {
                    return cardInfo.cardID == cardID;
                });
                if (foundCardInfo != null)
                    return $"{foundCardInfo.name}:{foundCardInfo.cardID}";
                else
                    return $"커스텀 카드:{cardID}";
            }

            /// <summary>
            /// Get book chapter description
            /// </summary>
            /// <param name="bookID">Book id to use</param>
            /// <returns>DES</returns>
            public static string GetBookChapterDescription(string bookID)
            {
                DS.DropBookInfo founDropBookInfo = DM.GameInfos.dropBookInfos.Find((DS.DropBookInfo dropBookInfo) =>
                {
                    return dropBookInfo.bookID == bookID;
                });
                if (founDropBookInfo != null)
                {
                    if (DS.GameInfo.chapter_Dic.ContainsKey(founDropBookInfo.chapter))
                        return $"{DS.GameInfo.chapter_Dic[founDropBookInfo.chapter]}:{founDropBookInfo.bookID}";
                    else
                        return $":{founDropBookInfo.bookID}";
                }
                else
                    return $"커스텀 에피소드:{bookID}";
            }
        }
    }
}

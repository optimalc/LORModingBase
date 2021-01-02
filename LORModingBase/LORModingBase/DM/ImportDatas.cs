using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Import all datas
    /// </summary>
    class ImportDatas
    {
        public static string TARGET_MODE_DIC = "";

        /// <summary>
        /// Import all datas
        /// </summary>
        /// <param name="dicToLoad">Target directory to load</param>
        public static void ImportAllDatas(string dicToLoad)
        {
            MainWindow.criticalPageInfos.Clear();
            TARGET_MODE_DIC = dicToLoad;

            ImportDatas_CriticalPages();
            ImportDatas_CriticalPageDescription();
            ImportDatas_DropBooks();
        }

        /// <summary>
        /// Import ciritical pages info datas
        /// </summary>
        public static void ImportDatas_CriticalPages()
        {
            string EQUIP_PAGE_PATH = $"{TARGET_MODE_DIC}\\StaticInfo\\EquipPage\\EquipPage.txt";
            if (!File.Exists(EQUIP_PAGE_PATH))
                return;

            XmlNodeList bookNodes = Tools.XmlFile.SelectNodeLists(EQUIP_PAGE_PATH, "//Book");
            foreach (XmlNode bookNode in bookNodes)
            {
                DS.CriticalPageInfo criticalPageInfo = new DS.CriticalPageInfo();
                if (bookNode.Attributes["ID"] != null)
                    criticalPageInfo.bookID = bookNode.Attributes["ID"].Value;
                criticalPageInfo.name = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Name");
                criticalPageInfo.rarity = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Rarity");

                #region 책 아이콘 정보 불러오기
                criticalPageInfo.iconName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "BookIcon");
                criticalPageInfo.iconDes = DM.StaticInfos.GetDescription.GetIconDescription(criticalPageInfo.iconName);
                #endregion
                #region 책 에피소드 정보 불러오기
                criticalPageInfo.episode = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Episode");
                criticalPageInfo.chapter = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "Chapter");
                criticalPageInfo.episodeDes = DM.StaticInfos.GetDescription.GetEpisodeDescription(criticalPageInfo.episode, criticalPageInfo.chapter);
                #endregion
                #region 책 스킨 정보 불러오기
                criticalPageInfo.skinName = Tools.XmlFile.GetXmlNodeSafe.ToString(bookNode, "CharacterSkin");
                criticalPageInfo.skinDes = DM.StaticInfos.GetDescription.GetSkinDescription(criticalPageInfo.skinName);
                #endregion

                if (bookNode["RangeType"] != null)
                    criticalPageInfo.rangeType = bookNode["RangeType"].InnerText;

                XmlNode equipEffectNode = bookNode.SelectSingleNode("EquipEffect");
                if(equipEffectNode != null)
                {
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
                        if (!string.IsNullOrEmpty(passiveNode.InnerText))
                            criticalPageInfo.passiveIDs.Add(DM.StaticInfos.GetDescription.GetPassiveDescription(passiveNode.InnerText));
                    }
                }
                MainWindow.criticalPageInfos.Add(criticalPageInfo);
            }
        }

        /// <summary>
        /// Import page description
        /// </summary>
        public static void ImportDatas_CriticalPageDescription()
        {
            string BOOKS_PATH = $"{TARGET_MODE_DIC}\\Localize\\kr\\Books\\_Books.txt";
            if (!File.Exists(BOOKS_PATH))
                return;

            XmlNodeList bookDescNodes = Tools.XmlFile.SelectNodeLists(BOOKS_PATH, "//BookDesc");
            foreach (XmlNode bookDescNode in bookDescNodes)
            {
                if (bookDescNode.Attributes["BookID"] == null)
                    continue;

                DS.CriticalPageInfo foundCriticalPageInfo = MainWindow.criticalPageInfos.Find((DS.CriticalPageInfo criticalPageInfo) =>
                {
                    return criticalPageInfo.bookID == bookDescNode.Attributes["BookID"].Value;
                });
                if (foundCriticalPageInfo == null)
                    continue;

                XmlNodeList descNodes = bookDescNode.SelectNodes("TextList/Desc");
                if (descNodes.Count > 0)
                    foundCriticalPageInfo.description = "";

                for (int descNodeIndex=0; descNodeIndex<descNodes.Count; descNodeIndex++)
                {
                    if (descNodeIndex > 0)
                        foundCriticalPageInfo.description += "\r\n\r\n";
                    foundCriticalPageInfo.description += descNodes[descNodeIndex].InnerText;
                }
            }
        }
        
        /// <summary>
        /// Import drop books datas
        /// </summary>
        public static void ImportDatas_DropBooks()
        {
            string DROP_BOOK_PATH = $"{TARGET_MODE_DIC}\\StaticInfo\\DropBook\\DropBook.txt";
            if (!File.Exists(DROP_BOOK_PATH))
                return;

            XmlNodeList bookUseNodes = Tools.XmlFile.SelectNodeLists(DROP_BOOK_PATH, "//BookUse");
            foreach (XmlNode bookUseNode in bookUseNodes)
            {
                if (bookUseNode.Attributes["ID"] == null)
                    continue;
                if (string.IsNullOrEmpty(bookUseNode.Attributes["ID"].Value))
                    continue;

                XmlNodeList dropItemNodes = bookUseNode.SelectNodes("DropItem");
                foreach (XmlNode dropItemNode in dropItemNodes)
                {
                    if (string.IsNullOrEmpty(dropItemNode.InnerText))
                        continue;

                    DS.CriticalPageInfo foundCriticalPageInfo = MainWindow.criticalPageInfos.Find((DS.CriticalPageInfo criticalPageInfo) =>
                    {
                        return criticalPageInfo.bookID == dropItemNode.InnerText;
                    });
                    if(foundCriticalPageInfo != null)
                    {
                        DS.DropBookInfo foundDropBookInfo = DM.StaticInfos.dropBookInfos.Find((DS.DropBookInfo dropInfo) =>
                        {
                            return dropInfo.bookID == bookUseNode.Attributes["ID"].Value;
                        });
                        foundCriticalPageInfo.dropBooks.Add($"{foundDropBookInfo.iconDesc}:{foundDropBookInfo.bookID}");
                    }
                }
            }
        }
    }
}

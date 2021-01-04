using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Export created datas
    /// </summary>
    class ExportDatas
    {
        public static string MDOE_DIR_TO_USE = "";

        /// <summary>
        /// Export inputed datas
        /// </summary>
        /// <param name="modeDicName"></param>
        public static string ExportAllDatas(string modeDicName)
        {
            if(DM.Config.config.isDirectBaseModeExport)
                MDOE_DIR_TO_USE = $"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\{modeDicName}";
            else
                MDOE_DIR_TO_USE = $"{DS.PROGRAM_PATHS.DIC_EXPORT_DATAS}\\{modeDicName}";

            if (Directory.Exists(MDOE_DIR_TO_USE))
                Directory.Delete(MDOE_DIR_TO_USE, true);

            Directory.CreateDirectory(MDOE_DIR_TO_USE);

            if (MainWindow.criticalPageInfos.Count > 0)
            {
                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\EquipPage");
                ExportDatas_CriticalPages();

                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\Localize\\{DM.Config.config.localizeOption}\\Books");
                ExportDatas_CriticalPageDescription();

                ExportDatas_DropBooks();
            }
            if (MainWindow.cardInfos.Count > 0)
            {
                ExporDatas_MakeArtWork();

                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\Card");
                ExportDatas_CardInfos();

                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\Localize\\{DM.Config.config.localizeOption}\\BattlesCards");
                ExportDatas_CardInfosDescription();

                ExportDatas_DropTables();
            }

            return MDOE_DIR_TO_USE;
        }

        #region Export ciritical page infos
        /// <summary>
        /// Export ciritical pages
        /// </summary>
        public static void ExportDatas_CriticalPages()
        {
            string EQUIP_PAGE_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\EquipPage\\EquipPage.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_EQUIP_PAGE, EQUIP_PAGE_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(EQUIP_PAGE_PATH, "//BookXmlRoot");

            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                XmlElement bookElement = rootNode.OwnerDocument.CreateElement("Book");
                bookElement.SetAttribute("ID", ciriticalInfo.bookID);

                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "Name", ciriticalInfo.name);
                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "TextId", ciriticalInfo.bookID);
                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "BookIcon", ciriticalInfo.iconName);

                if (!string.IsNullOrEmpty(ciriticalInfo.chapter))
                    Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "Chapter", ciriticalInfo.chapter);
                if (!string.IsNullOrEmpty(ciriticalInfo.episode))
                    Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "Episode", ciriticalInfo.episode);

                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "Rarity", ciriticalInfo.rarity);
                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "CharacterSkin", ciriticalInfo.skinName);
                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "SpeedDiceNum", "1");

                if (ciriticalInfo.rangeType == "Range" || ciriticalInfo.rangeType == "Hybrid")
                    Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "RangeType", ciriticalInfo.rangeType);

                XmlElement equipEffectElement = rootNode.OwnerDocument.CreateElement("EquipEffect");

                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "HP", ciriticalInfo.HP);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "Break", ciriticalInfo.breakNum);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "SpeedMin", ciriticalInfo.minSpeedCount);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "Speed", ciriticalInfo.maxSpeedCount);

                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "SResist", ciriticalInfo.SResist);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "PResist", ciriticalInfo.PResist);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "HResist", ciriticalInfo.HResist);

                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "SBResist", ciriticalInfo.BSResist);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "PBResist", ciriticalInfo.BPResist);
                Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "HBResist", ciriticalInfo.BHResist);

                #region Add Enemy Infomation if exists
                if (!string.IsNullOrEmpty(ciriticalInfo.ENEMY_StartPlayPoint))
                    Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "StartPlayPoint", ciriticalInfo.ENEMY_StartPlayPoint);
                if (!string.IsNullOrEmpty(ciriticalInfo.ENEMY_MaxPlayPoint))
                    Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "MaxPlayPoint", ciriticalInfo.ENEMY_MaxPlayPoint);
                if (!string.IsNullOrEmpty(ciriticalInfo.ENEMY_EmotionLevel))
                    Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "EmotionLevel", ciriticalInfo.ENEMY_EmotionLevel);
                if (!string.IsNullOrEmpty(ciriticalInfo.ENEMY_AddedStartDraw))
                    Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "AddedStartDraw", ciriticalInfo.ENEMY_AddedStartDraw);
                #endregion
                #region Add Only Cards List
                foreach (string onlyCard in ciriticalInfo.onlyCards)
                {
                    if (string.IsNullOrEmpty(onlyCard))
                        continue;
                    Tools.XmlFile.AddNewNodeWithInnerText(equipEffectElement, "OnlyCard", onlyCard.Split(':').Last());
                }
                #endregion

                foreach (string passiveName in ciriticalInfo.passiveIDs)
                {
                    XmlElement passiveElement = rootNode.OwnerDocument.CreateElement("Passive");
                    passiveElement.SetAttribute("Level", "10");
                    passiveElement.InnerText = passiveName.Split(':').Last();
                    equipEffectElement.AppendChild(passiveElement);
                }

                bookElement.AppendChild(equipEffectElement);


                rootNode.AppendChild(bookElement);
            }

            rootNode.OwnerDocument.Save(EQUIP_PAGE_PATH);
        }

        /// <summary>
        /// Export critical page description
        /// </summary>
        public static void ExportDatas_CriticalPageDescription()
        {
            string BOOKS_PATH = $"{MDOE_DIR_TO_USE}\\Localize\\{DM.Config.config.localizeOption}\\Books\\_Books.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_BOOKS, BOOKS_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(BOOKS_PATH, "//bookDescList");

            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                XmlElement bookDescElement = rootNode.OwnerDocument.CreateElement("BookDesc");
                bookDescElement.SetAttribute("BookID", ciriticalInfo.bookID);
                Tools.XmlFile.AddNewNodeWithInnerText(bookDescElement, "BookName", ciriticalInfo.name);

                XmlElement textListElement = rootNode.OwnerDocument.CreateElement("TextList");
                foreach (string desPart in ciriticalInfo.description.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None))
                {
                    Tools.XmlFile.AddNewNodeWithInnerText(textListElement, "Desc", desPart.Replace("\r\n", " ").Replace("\r", "").Replace("\n", ""));
                }
                bookDescElement.AppendChild(textListElement);

                XmlElement passiveListElement = rootNode.OwnerDocument.CreateElement("PassiveList");
                foreach (string passiveName in ciriticalInfo.passiveIDs)
                {
                    Tools.XmlFile.AddNewNodeWithInnerText(passiveListElement, "Passive", passiveName.Split(':')[1]);
                }
                bookDescElement.AppendChild(passiveListElement);

                rootNode.AppendChild(bookDescElement);
            }

            rootNode.OwnerDocument.Save(BOOKS_PATH);
        }

        /// <summary>
        /// Export DropBooks
        /// </summary>
        public static void ExportDatas_DropBooks()
        {
            Dictionary<string, List<string>> dropBookDic = new Dictionary<string, List<string>>(); // <DropBook ID, BookID>
            #region Make drop book dic
            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                foreach (string dropBookID in ciriticalInfo.dropBooks)
                {
                    string DROP_BOOK_ID = dropBookID.Split(':').Last();
                    if (!dropBookDic.ContainsKey(DROP_BOOK_ID))
                        dropBookDic[DROP_BOOK_ID] = new List<string>();

                    dropBookDic[DROP_BOOK_ID].Add(ciriticalInfo.bookID);
                }
            }
            #endregion
            if (dropBookDic.Keys.Count <= 0)
                return;

            Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\DropBook");
            string DROP_BOOK_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\DropBook\\DropBook.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_DROP_BOOK, DROP_BOOK_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(DROP_BOOK_PATH, "//BookUseXmlRoot");
            foreach (string dropBookID in dropBookDic.Keys)
            {
                DS.DropBookInfo foundDropBookInfo = DM.StaticInfos.dropBookInfos.Find((DS.DropBookInfo info) =>
                {
                    return info.bookID == dropBookID;
                });
                if (foundDropBookInfo == null) continue;

                string DROP_BOOK_SEARCH_PATH = $"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\DropBook\\DropBook_ch{foundDropBookInfo.chapter}.txt";
                if (!File.Exists(DROP_BOOK_SEARCH_PATH)) continue;

                XmlNode copiedBookUseNode = Tools.XmlFile.SelectSingleNode(DROP_BOOK_SEARCH_PATH, $"//BookUse[@ID='{dropBookID}']");
                if (copiedBookUseNode == null) continue;


                XmlNodeList dropItems = copiedBookUseNode.SelectNodes("DropItem");
                List<string> dropItemList = new List<string>();
                foreach (XmlNode dropItem in dropItems)
                {
                    if (string.IsNullOrEmpty(dropItem.InnerText)) continue;
                    dropItemList.Add(dropItem.InnerText);
                }

                foreach (string bookIDToDrop in dropBookDic[dropBookID])
                {
                    if (dropItemList.Contains(bookIDToDrop))
                        continue;
                    else
                        dropItemList.Add(bookIDToDrop);

                    XmlElement dropItemElement = copiedBookUseNode.OwnerDocument.CreateElement("DropItem");
                    dropItemElement.SetAttribute("Type", "Equip");
                    dropItemElement.InnerText = bookIDToDrop;
                    copiedBookUseNode.AppendChild(dropItemElement);
                }

                rootNode.AppendChild(rootNode.OwnerDocument.ImportNode(copiedBookUseNode, true));
            }
            rootNode.OwnerDocument.Save(DROP_BOOK_PATH);
        }
        #endregion

        #region Export card infos
        public static void ExporDatas_MakeArtWork()
        {
            bool isToDo = false;
            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                if(cardInfo.cardImage.Contains("%IMAGE_PATH%"))
                {
                    isToDo = true;
                    break;
                }
            }

            if (!isToDo) 
                return;

            Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\ArtWork");
            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                if (cardInfo.cardImage.Contains("%IMAGE_PATH%"))
                {
                    File.Copy(cardInfo.cardImage.Split('/')[1], $"{MDOE_DIR_TO_USE}\\ArtWork\\{cardInfo.cardImage.Split('/')[1].Split('\\').Last()}");
                }
            }
        }

        public static void ExportDatas_CardInfos()
        {
            string CARD_INFO_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\Card\\CardInfo.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_CARD_INFO, CARD_INFO_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(CARD_INFO_PATH, "//DiceCardXmlRoot");

            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                XmlElement cardElement = rootNode.OwnerDocument.CreateElement("Card");

                #region Basic information
                cardElement.SetAttribute("ID", cardInfo.cardID);

                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Name", cardInfo.name);
                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Artwork", cardInfo.cardImage.Split(':').Last());
                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Rarity", cardInfo.rarity);

                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Script", cardInfo.cardScript.Split(':').Last());
                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "ScriptDesc", "");

                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Chapter", cardInfo.chapter);
                Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Priority", cardInfo.priority);

                if (!string.IsNullOrEmpty(cardInfo.option))
                    Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "Option", cardInfo.option);
                if (!string.IsNullOrEmpty(cardInfo.priorityScript))
                    Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "PriorityScript", cardInfo.priorityScript);
                if (!string.IsNullOrEmpty(cardInfo.sortPriority))
                    Tools.XmlFile.AddNewNodeWithInnerText(cardElement, "SortPriority", cardInfo.sortPriority);
                #endregion
                #region Spec information
                XmlElement specElement = rootNode.OwnerDocument.CreateElement("Spec");
                specElement.SetAttribute("Range", cardInfo.rangeType);
                specElement.SetAttribute("Cost", cardInfo.cost);

                if(!string.IsNullOrEmpty(cardInfo.EXTRA_EmotionLimit))
                    specElement.SetAttribute("EmotionLimit", cardInfo.EXTRA_EmotionLimit);
                if (!string.IsNullOrEmpty(cardInfo.EXTRA_Affection))
                    specElement.SetAttribute("Affection", cardInfo.EXTRA_Affection);

                cardElement.AppendChild(specElement);
                #endregion
                #region Dices information
                XmlElement behaviourListElement = rootNode.OwnerDocument.CreateElement("BehaviourList");
                foreach(DS.Dice diceInfo in cardInfo.dices)
                {
                    XmlElement behaviourElement = rootNode.OwnerDocument.CreateElement("Behaviour");

                    behaviourElement.SetAttribute("Min", diceInfo.min);
                    behaviourElement.SetAttribute("Dice", diceInfo.max);

                    behaviourElement.SetAttribute("Type", diceInfo.type);
                    behaviourElement.SetAttribute("Detail", diceInfo.detail);

                    if(string.IsNullOrEmpty(diceInfo.motion))
                    {
                        switch (diceInfo.detail)
                        {
                            case "Slash":
                                behaviourElement.SetAttribute("Motion", "H");
                                break;
                            case "Penetrate":
                                behaviourElement.SetAttribute("Motion", "Z");
                                break;
                            case "Hit":
                                behaviourElement.SetAttribute("Motion", "J");
                                break;

                            case "Evasion":
                                behaviourElement.SetAttribute("Motion", "E");
                                break;
                            case "Guard":
                                behaviourElement.SetAttribute("Guard", "G");
                                break;

                            default:
                                behaviourElement.SetAttribute("Motion", "H");
                                break;
                        }
                    }
                    else
                        behaviourElement.SetAttribute("Motion", diceInfo.motion);

                    behaviourElement.SetAttribute("EffectRes", diceInfo.effectres);
                    behaviourElement.SetAttribute("Script", diceInfo.script.Split(':').Last());
                    behaviourElement.SetAttribute("Desc", diceInfo.script.Split(':')[0]);

                    if (!string.IsNullOrEmpty(diceInfo.actionScript))
                        behaviourElement.SetAttribute("ActionScript", diceInfo.actionScript);

                    behaviourListElement.AppendChild(behaviourElement);
                }
                cardElement.AppendChild(behaviourListElement);
                #endregion

                rootNode.AppendChild(cardElement);
            }

            rootNode.OwnerDocument.Save(CARD_INFO_PATH);
        }
        
        public static void ExportDatas_CardInfosDescription()
        {
            string CARDS_PATH = $"{MDOE_DIR_TO_USE}\\Localize\\{DM.Config.config.localizeOption}\\BattlesCards\\BattlesCards.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_BATTLE_CARDS, CARDS_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(CARDS_PATH, "//cardDescList");

            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                XmlElement cardDescElement = rootNode.OwnerDocument.CreateElement("BattleCardDesc");
                cardDescElement.SetAttribute("ID", cardInfo.cardID);
                Tools.XmlFile.AddNewNodeWithInnerText(cardDescElement, "LocalizedName", cardInfo.name);

                rootNode.AppendChild(cardDescElement);
            }

            rootNode.OwnerDocument.Save(CARDS_PATH);
        }

        public static void ExportDatas_DropTables()
        {
            Dictionary<string, List<string>> dropBookDic = new Dictionary<string, List<string>>(); // <DropBook ID, BookID>
            #region Make drop book dic
            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                foreach (string dropBookID in cardInfo.dropBooks)
                {
                    string DROP_BOOK_ID = dropBookID.Split(':').Last();
                    if (!dropBookDic.ContainsKey(DROP_BOOK_ID))
                        dropBookDic[DROP_BOOK_ID] = new List<string>();

                    dropBookDic[DROP_BOOK_ID].Add(cardInfo.cardID);
                }
            }
            #endregion
            if (dropBookDic.Keys.Count <= 0)
                return;


            Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\CardDropTable");
            string DROP_TABLE_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\CardDropTable\\CardDropTable.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_CARD_DROP_TABLE, DROP_TABLE_PATH);


            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(DROP_TABLE_PATH, "//CardDropTableXmlRoot");
            foreach (string dropBookID in dropBookDic.Keys)
            {
                XmlElement dropTableElement = rootNode.OwnerDocument.CreateElement("DropTable");
                dropTableElement.SetAttribute("ID", dropBookID);

                List<string> dropItemList = new List<string>();
                if (DM.StaticInfos.dropBookDic.ContainsKey(dropBookID))
                {
                    foreach(string existedID in DM.StaticInfos.dropBookDic[dropBookID])
                    {
                        Tools.XmlFile.AddNewNodeWithInnerText(dropTableElement, "Card", existedID);
                        dropItemList.Add(existedID);
                    }
                }

                foreach(string idToAdd in dropBookDic[dropBookID])
                {
                    if (dropItemList.Contains(idToAdd))
                        continue;
                    else
                        dropItemList.Add(idToAdd);

                    Tools.XmlFile.AddNewNodeWithInnerText(dropTableElement, "Card", idToAdd);
                }

                rootNode.AppendChild(dropTableElement);
            }
            rootNode.OwnerDocument.Save(DROP_TABLE_PATH);
        }
        #endregion
    }
}

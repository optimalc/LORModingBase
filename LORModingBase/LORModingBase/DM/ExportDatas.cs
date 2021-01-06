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

            return MDOE_DIR_TO_USE;
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


        /// <summary>
        /// Get save path from static path
        /// </summary>
        public static string GetStaticPathToSave(XmlData xmlData, string baseDic)
        {
            if (xmlData == null) return "";
            if (xmlData.currentXmlFilePaths.Count <= 0) return "";
            int FOUND_INDEX = xmlData.currentXmlFilePaths[0].IndexOf("StaticInfo");
            if (FOUND_INDEX < 0) return "";
            return $"{baseDic}\\{xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX)}";
        }

        /// <summary>
        /// Get save path from localize path
        /// </summary>
        public static string GetLocalizePathToSave(XmlData xmlData, string baseDic)
        {
            if (xmlData == null) return "";
            if (xmlData.currentXmlFilePaths.Count <= 0) return "";
            int FOUND_INDEX = xmlData.currentXmlFilePaths[0].IndexOf("Localize");
            if (FOUND_INDEX < 0) return "";
            return $"{baseDic}\\{xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX)}";
        }


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
                if (DM.GameInfos.dropBookDic.ContainsKey(dropBookID))
                {
                    foreach(string existedID in DM.GameInfos.dropBookDic[dropBookID])
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

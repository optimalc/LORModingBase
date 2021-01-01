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
            MDOE_DIR_TO_USE = $"{DS.PATH.DIC_EXPORT_DATAS}\\{modeDicName}";
            if (Directory.Exists(MDOE_DIR_TO_USE))
                Directory.Delete(MDOE_DIR_TO_USE, true);

            Directory.CreateDirectory(MDOE_DIR_TO_USE);

            if (MainWindow.criticalPageInfos.Count > 0)
            {
                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\EquipPage");
                ExportDatas_CriticalPages();

                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\Localize\\kr\\Books");
                ExportDatas_CriticalPageDescription();

                Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\DropBook");
                ExportDatas_DropBooks();
            }

            return MDOE_DIR_TO_USE;
        }

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
                Tools.XmlFile.AddNewNodeWithInnerText(bookElement, "Chapter", ciriticalInfo.chapter);
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

                foreach(string passiveName in ciriticalInfo.passiveIDs)
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
            string BOOKS_PATH = $"{MDOE_DIR_TO_USE}\\Localize\\kr\\Books\\_Books.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_BOOKS, BOOKS_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(BOOKS_PATH, "//bookDescList");

            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                XmlElement bookDescElement = rootNode.OwnerDocument.CreateElement("BookDesc");
                bookDescElement.SetAttribute("BookID", ciriticalInfo.bookID);
                Tools.XmlFile.AddNewNodeWithInnerText(bookDescElement, "BookName", ciriticalInfo.name);

                XmlElement textListElement = rootNode.OwnerDocument.CreateElement("TextList");
                foreach(string desPart in ciriticalInfo.description.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None))
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
            string DROP_BOOK_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\DropBook\\DropBook.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_DROP_BOOK, DROP_BOOK_PATH);

            Dictionary<string, List<string>> dropBookDic = new Dictionary<string, List<string>>(); // <DropBook ID, BookID>
            #region Make drop book dic
            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                foreach(string dropBookID in ciriticalInfo.dropBooks)
                {
                    string DROP_BOOK_ID = dropBookID.Split(':').Last();
                    if (!dropBookDic.ContainsKey(DROP_BOOK_ID))
                        dropBookDic[DROP_BOOK_ID] = new List<string>();

                    dropBookDic[DROP_BOOK_ID].Add(ciriticalInfo.bookID);
                }
            }
            #endregion

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(DROP_BOOK_PATH, "//BookUseXmlRoot");
            foreach(string dropBookID in dropBookDic.Keys)
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
    }
}

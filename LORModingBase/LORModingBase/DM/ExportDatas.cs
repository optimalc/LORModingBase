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
            Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo");

            Directory.CreateDirectory($"{MDOE_DIR_TO_USE}\\StaticInfo\\EquipPage");
            ExportDatas_CriticalPages();

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
    }
}

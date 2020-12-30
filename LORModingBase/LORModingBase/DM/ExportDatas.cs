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

        public static void ExportDatas_CriticalPages()
        {
            string EQUIP_PAGE_PATH = $"{MDOE_DIR_TO_USE}\\StaticInfo\\EquipPage\\EquipPage.txt";
            File.Copy(DS.PATH.RESOURCE_XML_BASE_EQUIP_PAGE, EQUIP_PAGE_PATH);

            XmlNode rootNode = Tools.XmlFile.SelectSingleNode(EQUIP_PAGE_PATH, "//BookXmlRoot");

            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                XmlElement bookElement = rootNode.OwnerDocument.CreateElement("Book");
                bookElement.SetAttribute("ID", ciriticalInfo.bookID);
                rootNode.AppendChild(bookElement);
            }

            rootNode.OwnerDocument.Save(EQUIP_PAGE_PATH);
        }
    }
}

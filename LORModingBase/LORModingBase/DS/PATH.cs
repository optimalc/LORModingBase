using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DS
{
    class PATH
    {
        /// <summary>
        /// Export data directory
        /// </summary>
        public const string DIC_EXPORT_DATAS = ".\\exportedModes";

        /// <summary>
        /// Config file
        /// </summary>
        public const string CONFIG = "./config.json";
        /// <summary>
        /// Version info file
        /// </summary>
        public const string VERSION = "./version.txt";

        /// <summary>
        /// Mode resources
        /// </summary>
        public const string RELATIVE_DIC_LOR_MODE_RESOURCES = "LibraryOfRuina_Data\\Managed\\BaseMod";
        /// <summary>
        /// Mode staticInfo resources
        /// </summary>
        public static string RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO = $"{RELATIVE_DIC_LOR_MODE_RESOURCES}\\StaticInfo";
        /// <summary>
        /// Mode localize resources
        /// </summary>
        public static string RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE = $"{RELATIVE_DIC_LOR_MODE_RESOURCES}\\Localize";

        /// <summary>
        /// Resource dictionary
        /// </summary>
        public static string DIC_RESURCE = "./resources";
        /// <summary>
        /// Xml bases for equip page
        /// </summary>
        public static string RESOURCE_XML_BASE_EQUIP_PAGE = $"{DIC_RESURCE}\\EquipPage.txt";
        /// <summary>
        /// Xml bases for Books info
        /// </summary>
        public static string RESOURCE_XML_BASE_BOOKS = $"{DIC_RESURCE}\\_Books.txt";
        /// <summary>
        /// Xml base for Dropping books info
        /// </summary>
        public static string RESOURCE_XML_BASE_DROP_BOOK = $"{DIC_RESURCE}\\DropBook.txt";
    }
}

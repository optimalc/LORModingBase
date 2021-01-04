﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DS
{
    class PROGRAM_PATHS
    {
        /// <summary>
        /// Export data directory
        /// </summary>
        public const string DIC_EXPORT_DATAS = ".\\exportedModes";

        /// <summary>
        /// Config file
        /// </summary>
        public const string CONFIG = ".\\config.json";

        /// <summary>
        /// Locaize dictionary
        /// </summary>
        public const string LOCAIZE_DIC = ".\\localize";

        /// <summary>
        /// Version info file
        /// </summary>
        public const string VERSION = ".\\version.txt";
    }

    class GAME_PATHS
    {

    }

    class PATH
    {

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

        /// <summary>
        /// Xml bases for Card info
        /// </summary>
        public static string RESOURCE_XML_BASE_CARD_INFO = $"{DIC_RESURCE}\\CardInfo.txt";
        /// <summary>
        /// Xml bases for Battle cards
        /// </summary>
        public static string RESOURCE_XML_BASE_BATTLE_CARDS = $"{DIC_RESURCE}\\_BattleCards.txt";
        /// <summary>
        /// Xml base for Card drop table
        /// </summary>
        public static string RESOURCE_XML_BASE_CARD_DROP_TABLE = $"{DIC_RESURCE}\\CardDropTable.txt";

        public static string RESOURCE_UNITY_ENGINE_DEBUG = $"{DIC_RESURCE}\\UnityEngine.dll";
    }

    class GAME_RESOURCE_PATHS
    {
        /// <summary>
        /// Mode resource directory
        /// </summary>
        public const string RESOURCE_BASE_MODE = "LibraryOfRuina_Data\\Managed\\BaseMod";

        /// <summary>
        /// Static root directory
        /// </summary>
        public string RESOURCE_ROOT_STATIC = "";
        /// <summary>
        /// Localize root directory
        /// </summary>
        public string RESOURCE_ROOT_LOCALIZE = "";

        /// <summary>
        /// EquipPage static data
        /// </summary>
        public string STATIC_EquipPage = "";
        /// <summary>
        /// StageInfo static data
        /// </summary>
        public string STATIC_StageInfo = "";
        /// <summary>
        /// DropBook static data
        /// </summary>
        public string STATIC_DropBook = "";

        /// <summary>
        /// Passive Desc localized data
        /// </summary>
        public string LOCALIZE_PassiveDesc = "";
        /// <summary>
        /// Stage Name localized data
        /// </summary>
        public string LOCALIZE_StageName = "";
        /// <summary>
        /// DropBook localized data
        /// </summary>
        public string LOCALIZE_DropBook = "";
        /// <summary>
        /// Books localized data
        /// </summary>
        public string LOCALIZE_BOOKS = "";
    }
}

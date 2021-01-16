using System;
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

        /// <summary>
        /// Debug test file
        /// </summary>
        public const string DEBUG_TEST = ".\\debugTest.txt";

        /// <summary>
        /// Code block group directory
        /// </summary>
        public const string CODE_BLOCK_GROUP = ".\\codeBlockGroup";

        /// <summary>
        /// Auto generate code block directory
        /// </summary>
        public const string AUTO_GENERATE_CODE_BLOCK = ".\\autoGenerateCodeBlock";

        /// <summary>
        /// Code block preset directory
        /// </summary>
        public const string CODE_BLOCK_PRESET = ".\\codeBlockPreset";
    }

    class PROGRAM_RESOURCE_PATHS
    {
        /// <summary>
        /// Resource dictionary
        /// </summary>
        public static string DIC_RESOURCE = "./resources";
        /// <summary>
        /// Modified Unity engine path 
        /// </summary>
        public static string RESOURCE_UNITY_ENGINE_DEBUG = $"{DIC_RESOURCE}\\UnityEngine.dll";

        public static string RESOURCE_ROOT_CODE_BLOCK_LIST = $"{DIC_RESOURCE}\\codeBlockRootShowList.json";

        public static string RESOURCE_COMPILE_DENPENDENCIES = $"{DIC_RESOURCE}\\compileDependencies.json";

        public static string RESOURCE_USING_GLOBAL = $"{DIC_RESOURCE}\\usingGlobal.json";

        public static string RESOURCE_SKIN_ITEMINFO = $"{DIC_RESOURCE}\\ItemInfo.xml";

        public static string RESOURCE_SKIN_MODEINFO = $"{DIC_RESOURCE}\\ModInfo.Xml";
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
        /// Story effect info directory
        /// </summary>
        public string RESOURCE_ROOT_STORY_EFFECT_INFO = "";
        /// <summary>
        /// Story localize directory
        /// </summary>
        public string RESOURCE_ROOT_STORY_LOCALIZE= "";
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using LORModingBase.CustomExtensions;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game static datas management
    /// </summary>
    partial class GameInfos
    {
        /// <summary>
        /// Static XmlData dictionary
        /// </summary>
        public static Dictionary<string, XmlData> staticInfos = new Dictionary<string, XmlData>();
        /// <summary>
        /// Localized XmlData dictionary
        /// </summary>
        public static Dictionary<string, XmlData> localizeInfos = new Dictionary<string, XmlData>();
        /// <summary>
        /// Stoary XmlData dictionary
        /// </summary>
        public static Dictionary<string, XmlData> storyInfos = new Dictionary<string, XmlData>();

        /// <summary>
        /// Load all static & localize & story datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadForGivenDirectoryRoot(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE, localizeInfos);
            LoadForGivenDirectoryRoot(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC, staticInfos);

            #region Load story dictionary
            storyInfos.Clear();
            storyInfos["EffectInfo"] = new XmlData(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_EFFECT_INFO);
            storyInfos["Localize"] = new XmlData(Directory.GetFiles(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_LOCALIZE)
                .FindAll_Contains(DM.Config.config.localizeOption.ToUpper())); 
            #endregion
        }

        /// <summary>
        /// Load all datas for given directory
        /// </summary>
        public static void LoadForGivenDirectoryRoot(string directoryRootPath, Dictionary<string, XmlData> XmlDataDic)
        {
            XmlDataDic.Clear();
            if (Directory.Exists(directoryRootPath))
            {
                Directory.GetDirectories(directoryRootPath).ForEachSafe((string dicPath) =>
                {
                    string DIC_KEY = dicPath.Split('\\').Last();
                    XmlDataDic[DIC_KEY] = new XmlData(dicPath);
                });
            }
        }
    }
}

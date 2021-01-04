using System.Collections.Generic;
using System.IO;
using System.Linq;
using LORModingBase.CustomExtensions;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game static datas management
    /// </summary>
    partial class StaticInfos
    {
        public static Dictionary<string, XmlData> staticInfos = new Dictionary<string, XmlData>();
        public static Dictionary<string, XmlData> localizeInfos = new Dictionary<string, XmlData>();
        public static Dictionary<string, XmlData> storyInfos = new Dictionary<string, XmlData>();

        /// <summary>
        /// Equip page static data
        /// </summary>
        public static XmlData XmlData_EquipPage = null;
        /// <summary>
        /// Stage info static data
        /// </summary>
        public static XmlData XmlData_StageInfo = null;
        /// <summary>
        /// DropBook static data
        /// </summary>
        public static XmlData XmlData_DropBook = null;

        /// <summary>
        /// Card drop table static data
        /// </summary>
        public static XmlData XmlData_CardDropTable = null;


        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadForGivenDirectoryRoot(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE, localizeInfos);
            LoadForGivenDirectoryRoot(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC, staticInfos);

            storyInfos.Clear();
            storyInfos["EffectInfo"] = new XmlData(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_EFFECT_INFO);
            storyInfos["Localize"] = new XmlData(Directory.GetFiles(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_LOCALIZE)
                .FindAll_Contains(DM.Config.config.localizeOption.ToUpper()));


            #region Load card data -> StaticInfo_Cards.cs
            LoadData_CardEffect();
            LoadData_CardsInfo();
            #endregion
            #region Load book datas -> StaticInfo_Books.cs
            LoadDatas_PassiveInfo();
            LoadDatas_StageInfo();
            LoadDatas_SkinAndBookIconInfo(); 
            #endregion
            LoadData_Dropbooks(); // Card drop book load
        }

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

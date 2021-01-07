using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;

namespace LORModingBase.DM
{
    /// <summary>
    /// Config data management
    /// </summary>
    class Config
    {
        /// <summary>
        /// Loaded config data
        /// </summary>
        public static DS.Config config = new DS.Config();

        public static DS.GAME_RESOURCE_PATHS GAME_RESOURCE_PATHS = new DS.GAME_RESOURCE_PATHS();

        /// <summary>
        /// Current working directory
        /// </summary>
        public static string CurrentWorkingDirectory = "";


        #region Save / Load config data
        /// <summary>
        /// Load config data
        /// </summary>
        public static void LoadData()
        {
            if (File.Exists(DS.PROGRAM_PATHS.CONFIG))
                config = Tools.JsonFile.LoadJsonFile<DS.Config>(DS.PROGRAM_PATHS.CONFIG);
            else
            {
                const string LOR_PATH = "steamapps\\common\\Library Of Ruina";

                config = new DS.Config();
                #region Get stream install path by searching registry key
                string streamInstallPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam", "InstallPath", "") as string; // 64bit
                if (string.IsNullOrEmpty(streamInstallPath))
                    streamInstallPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", "") as string; // 32bit 
                #endregion
                #region If LOR folder existed, save path to config
                if (!string.IsNullOrEmpty(streamInstallPath))
                {
                    string LOR_PATH_FULL = $"{streamInstallPath}\\{LOR_PATH}";
                    if (Directory.Exists(LOR_PATH_FULL))
                    {
                        config.LORFolderPath = LOR_PATH_FULL;
                        SaveData();
                    }
                } 
                #endregion
            }
            InitGameResourcePaths();
        }

        /// <summary>
        /// Initiailize all global game resource paths
        /// </summary>
        public static void InitGameResourcePaths()
        {
            GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC = $"{config.LORFolderPath}\\{DS.GAME_RESOURCE_PATHS.RESOURCE_BASE_MODE}\\StaticInfo";
            GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE = $"{config.LORFolderPath}\\{DS.GAME_RESOURCE_PATHS.RESOURCE_BASE_MODE}\\Localize\\{config.localizeOption}";
            GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_EFFECT_INFO = $"{config.LORFolderPath}\\{DS.GAME_RESOURCE_PATHS.RESOURCE_BASE_MODE}\\Story\\EffectInfo";
            GAME_RESOURCE_PATHS.RESOURCE_ROOT_STORY_LOCALIZE = $"{config.LORFolderPath}\\{DS.GAME_RESOURCE_PATHS.RESOURCE_BASE_MODE}\\Story\\Localize\\kr";
        }


        /// <summary>
        /// Save config data
        /// </summary>
        public static void SaveData()
        {
            Tools.JsonFile.SaveJsonFile<DS.Config>(DS.PROGRAM_PATHS.CONFIG, config);
            InitGameResourcePaths();
        }
        #endregion


        /// <summary>
        /// Change working direcotry with given mode name
        /// </summary>
        /// <param name="modeName">Mode name to use</param>
        public static void ChangeWorkingDirectory(string modeName)
        {
            if (DM.Config.config.isDirectBaseModeExport)
                CurrentWorkingDirectory = $"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\{modeName}";
            else
                CurrentWorkingDirectory = $"{DS.PROGRAM_PATHS.DIC_EXPORT_DATAS}\\{modeName}";
        }


        /// <summary>
        /// Get save path from static path
        /// </summary>
        public static string GetStaticPathToSave(XmlData xmlData, string baseDic, bool returnOnlyRelativePath = false)
        {
            if (xmlData == null) return "";
            if (xmlData.currentXmlFilePaths.Count <= 0) return "";
            int FOUND_INDEX = xmlData.currentXmlFilePaths[0].IndexOf("StaticInfo");
            if (FOUND_INDEX < 0) return "";
            if (returnOnlyRelativePath)
                return xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX);
            else
                return $"{baseDic}\\{xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX)}";
        }

        /// <summary>
        /// Get save path from localize path
        /// </summary>
        public static string GetLocalizePathToSave(XmlData xmlData, string baseDic, bool returnOnlyRelativePath = false)
        {
            if (xmlData == null) return "";
            if (xmlData.currentXmlFilePaths.Count <= 0) return "";
            int FOUND_INDEX = xmlData.currentXmlFilePaths[0].IndexOf("Localize");
            if (FOUND_INDEX < 0) return "";
            if (returnOnlyRelativePath)
                return xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX);
            else
                return $"{baseDic}\\{xmlData.currentXmlFilePaths[0].Substring(FOUND_INDEX)}";
        }

        /// <summary>
        /// Get full path from relative path
        /// </summary>
        /// <param name="relativePath">Relative path to use</param>
        /// <param name="baseDic">Base directory</param>
        /// <returns>Full path</returns>
        public static string GetPathFromRelativePath(string relativePath, string baseDic)
        {
            return $"{baseDic}\\{relativePath}";
        }
    }
}

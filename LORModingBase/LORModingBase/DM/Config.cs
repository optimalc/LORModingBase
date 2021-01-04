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
        
            GAME_RESOURCE_PATHS.STATIC_EquipPage = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC}\\EquipPage";
            GAME_RESOURCE_PATHS.STATIC_StageInfo = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC}\\StageInfo";
            GAME_RESOURCE_PATHS.STATIC_DropBook = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC}\\DropBook";

            GAME_RESOURCE_PATHS.LOCALIZE_PassiveDesc = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE}\\PassiveDesc";
            GAME_RESOURCE_PATHS.LOCALIZE_StageName = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE}\\StageName";
            GAME_RESOURCE_PATHS.LOCALIZE_DropBook = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE}\\etc\\{DM.Config.config.localizeOption.ToUpper()}_Dropbook.txt";
            GAME_RESOURCE_PATHS.LOCALIZE_BOOKS = $"{GAME_RESOURCE_PATHS.RESOURCE_ROOT_LOCALIZE}\\Books";
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
    }
}

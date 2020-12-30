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

        #region Save / Load config data
        /// <summary>
        /// Load config data
        /// </summary>
        public static void LoadData()
        {
            if (File.Exists(DS.PATH.CONFIG))
                config = Tools.JsonFile.LoadJsonFile<DS.Config>(DS.PATH.CONFIG);
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
        }

        /// <summary>
        /// Save config data
        /// </summary>
        public static void SaveData()
        {
            Tools.JsonFile.SaveJsonFile<DS.Config>(DS.PATH.CONFIG, config);
        }
        #endregion
    }
}

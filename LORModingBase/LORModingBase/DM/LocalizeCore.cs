using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Localize core data management tools
    /// </summary>
    class LocalizeCore
    {
        /// <summary>
        /// Localize data (Localize Folder, File, Key, Value)
        /// </summary>
        public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> localizedData = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        /// <summary>
        /// Get localized info
        /// </summary>
        public static void LoadAllDatas()
        {
            localizedData.Clear();
            Directory.GetDirectories(DS.PROGRAM_PATHS.LOCAIZE_DIC).ToList().ForEach((string localizeDic) =>
            {
                string LOCALIZE_FORDER_KEY = localizeDic.Split('\\').Last();

                if (DM.Config.config.localizeOption == LOCALIZE_FORDER_KEY)
                {
                    Dictionary<string, Dictionary<string, string>> locaizeDatas = new Dictionary<string, Dictionary<string, string>>();
                    Directory.GetFiles(localizeDic).ToList().ForEach((string filePath) =>
                    {
                        string LOCALIZE_FILE_KEY = filePath.Split('\\').Last().Split('.')[0];
                        locaizeDatas[LOCALIZE_FILE_KEY] = Tools.JsonFile.LoadJsonFile<Dictionary<string, string>>(filePath);
                    });
                    localizedData[LOCALIZE_FORDER_KEY] = locaizeDatas;
                }
                else
                {
                    string LANGUAGE_FILE = $"{localizeDic}\\{LANGUAGE_FILE_NAME.OPTION}.json";
                    if(File.Exists(LANGUAGE_FILE))
                    {
                        Dictionary<string,string> OPTION_DATAS = Tools.JsonFile.LoadJsonFile<Dictionary<string, string>>(LANGUAGE_FILE);
                        if(OPTION_DATAS.ContainsKey("language"))
                        {
                            Dictionary<string, Dictionary<string, string>> locaizeDatas = new Dictionary<string, Dictionary<string, string>>();
                            locaizeDatas[LANGUAGE_FILE_NAME.OPTION] = new Dictionary<string, string>() { { "language",OPTION_DATAS["language"] } };
                            localizedData[LOCALIZE_FORDER_KEY] = locaizeDatas;
                        }
                    }
                }
            });
        }


        /// <summary>
        /// Get localized option (LanguageCode, Language)
        /// </summary>
        /// <returns>Localize list</returns>
        public static Dictionary<string, string> GetLocalizeOption()
        {
            Dictionary<string, string> localizePair = new Dictionary<string, string>();
            foreach(KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> localizeDataPair in localizedData)
            {
                localizePair[localizeDataPair.Key] = localizeDataPair.Value[LANGUAGE_FILE_NAME.OPTION]["language"];
            }
            return localizePair;
        }

        /// <summary>
        /// Get localized option (Language, LanguageCode)
        /// </summary>
        /// <returns>Localize list</returns>
        public static Dictionary<string, string> GetLocalizeOptionRev()
        {
            Dictionary<string, string> localizePair = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> localizeDataPair in localizedData)
            {
                localizePair[localizeDataPair.Value[LANGUAGE_FILE_NAME.OPTION]["language"]] = localizeDataPair.Key;
            }
            return localizePair;
        }


        /// <summary>
        /// Check if specific language key exists
        /// </summary>
        /// <param name="LANGUAGE_FILE_NAME">Language file name</param>
        /// <param name="keyName">Key name to use</param>
        /// <returns>True if key exists</returns>
        public static bool IsLanguageKeyExist(string LANGUAGE_FILE_NAME, string keyName)
        {
            return localizedData[DM.Config.config.localizeOption][LANGUAGE_FILE_NAME].ContainsKey(keyName);
        }

        /// <summary>
        /// Get localized language data
        /// </summary>
        /// <param name="LANGUAGE_FILE_NAME">Language file name</param>
        /// <param name="keyName">Key name to use</param>
        /// <returns>Localized data</returns>
        public static string GetLanguageData(string LANGUAGE_FILE_NAME, string keyName)
        {
            if (IsLanguageKeyExist(LANGUAGE_FILE_NAME, keyName))
                return localizedData[DM.Config.config.localizeOption][LANGUAGE_FILE_NAME][keyName];
            else
                return "Language load failed";
        }
    

        /// <summary>
        /// Get language dictionary for specific file name
        /// </summary>
        /// <param name="LANGUAGE_FILE_NAME">Language name to use</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetLanguageDictionary(string LANGUAGE_FILE_NAME)
        {
            return localizedData[DM.Config.config.localizeOption][LANGUAGE_FILE_NAME];
        }
    }

    /// <summary>
    /// Language file name list
    /// </summary>
    public class LANGUAGE_FILE_NAME
    {
        public static string OPTION = "option";
        public static string CARD_INFO = "cardInfo";
        public static string BOOK_INFO = "bookInfo";
        public static string MAIN_WINDOW = "mainWindow";
        public static string ETC = "etc";
        public static string GLOBAL_WINDOW = "global";
    }
}

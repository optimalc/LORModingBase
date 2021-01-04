using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game localized datas management
    /// </summary>
    class LocalizeInfos
    {
        /// <summary>
        /// Passive description localized datas
        /// </summary>
        public static XmlData XmlData_PassiveDesc = null;
        /// <summary>
        /// StageName localized datas
        /// </summary>
        public static XmlData XmlData_StageName = null;
        /// <summary>
        /// DropBook localized datas
        /// </summary>
        public static XmlData XmlData_DropBook = null;
        /// <summary>
        /// Books localized datas
        /// </summary>
        public static XmlData XmlData_Books = null;


        /// <summary>
        /// Load all localized datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadBookLocalizeInfos();
        }


        /// <summary>
        /// Load localized infos for book
        /// </summary>
        public static void LoadBookLocalizeInfos()
        {
            XmlData_PassiveDesc = new XmlData(DM.Config.GAME_RESOURCE_PATHS.LOCALIZE_PassiveDesc);
            XmlData_StageName = new XmlData(DM.Config.GAME_RESOURCE_PATHS.LOCALIZE_StageName);
            XmlData_DropBook = new XmlData(DM.Config.GAME_RESOURCE_PATHS.LOCALIZE_DropBook);
            XmlData_Books = new XmlData(DM.Config.GAME_RESOURCE_PATHS.LOCALIZE_BOOKS);
        }
    }
}

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
        /// Load all localized datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadBookLocalizeInfos();
        }

        public static void LoadBookLocalizeInfos()
        {
            XmlData_PassiveDesc = new XmlData(DM.Config.GAME_RESOURCE_PATHS.LOCALIZE_PassiveDesc);
        }
    }
}

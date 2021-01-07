using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Check datas
    /// </summary>
    class CheckDatas
    {
        /// <summary>
        /// Logging data
        /// </summary>
        public static string LOG_DATA = "";

        /// <summary>
        /// Check datas
        /// </summary>
        public static string CheckAllDatas()
        {
            LOG_DATA = "";
            CheckDatas_Critical();
            CheckDatas_Caution();
            return LOG_DATA;
        }

        /// <summary>
        /// Check critical infos
        /// </summary>
        public static void CheckDatas_Critical()
        {

        }

        /// <summary>
        /// Check caution infos
        /// </summary>
        public static void CheckDatas_Caution()
        {

        }
    }
}

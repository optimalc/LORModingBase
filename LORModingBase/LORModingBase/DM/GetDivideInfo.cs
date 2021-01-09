using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class GetDivideInfo
    {
        /// <summary>
        /// Get divided info
        /// </summary>
        /// <param name="IdToUse"></param>
        /// <returns></returns>
        public static string GetDividedKeyPageInfo(string IdToUse)
        {
            int KEY_PAGE_ID = Convert.ToInt32(IdToUse);
            if (KEY_PAGE_ID < 1000)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_KEY_PAGE_LIBRARIAN");
            else if (KEY_PAGE_ID > 100000 && KEY_PAGE_ID < 199999)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_KEY_PAGE_ENEMY");
            else if (KEY_PAGE_ID > 200000 && KEY_PAGE_ID < 999999)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_KEY_PAGE_USER");
            else if (KEY_PAGE_ID > 9000000 && KEY_PAGE_ID < 9999999)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_KEY_PAGE_CREATURE");
            else
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_ETC");
        }
        
        public static string GetDividedCardInfo(string IdToUse)
        {
            int CARD_ID = Convert.ToInt32(IdToUse);
            if (CARD_ID < 1000)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_BASIC");
            else if (CARD_ID > 100000 && CARD_ID < 900000)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_NOMAL");
            else if (CARD_ID > 910000 && CARD_ID < 920200)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_EGO");
            else if ((CARD_ID > 900001 && CARD_ID < 910000) || (CARD_ID > 920200 && CARD_ID < 999999))
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_CREATURE");
            else if (CARD_ID > 1100001 && CARD_ID < 1200000)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_SPECIAL");
            else if (CARD_ID > 9900000 && CARD_ID < 9999999)
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_CARD_FINAL");
            else
                return DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, $"DIV_ETC");
        }

        /// <summary>
        /// Get divided filter info
        /// </summary>
        public static List<string> GetAllDividedKeyPageFilterInfo()
        {
            List<string> filterList = new List<string>();
            List<string> FILTER_LOCALIZED_NAMES = new List<string>() {
                "DIV_KEY_PAGE_LIBRARIAN",
                "DIV_KEY_PAGE_ENEMY",
                "DIV_KEY_PAGE_USER",
                "DIV_KEY_PAGE_CREATURE",
                "DIV_ETC"
            };
            FILTER_LOCALIZED_NAMES.ForEach((string FILTER_LOCALIZED_NAME) =>
            {
                filterList.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, FILTER_LOCALIZED_NAME));
            });
            return filterList;
        }

        public static List<string> GetAllDividedCardFilterInfo()
        {
            List<string> filterList = new List<string>();
            List<string> FILTER_LOCALIZED_NAMES = new List<string>() {
                "DIV_CARD_BASIC",
                "DIV_CARD_NOMAL",
                "DIV_CARD_EGO",
                "DIV_CARD_CREATURE",
                "DIV_CARD_SPECIAL",
                "DIV_CARD_FINAL",
                "DIV_ETC"
            };
            FILTER_LOCALIZED_NAMES.ForEach((string FILTER_LOCALIZED_NAME) =>
            {
                filterList.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DIVIDE_INFO, FILTER_LOCALIZED_NAME));
            });
            return filterList;
        }
    }
}

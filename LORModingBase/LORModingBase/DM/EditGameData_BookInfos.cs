using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Data that is to be used for editing book information
    /// </summary>
    class EditGameData_BookInfos
    {
        /// <summary>
        /// Editing data for StaticEquipPage
        /// </summary>
        public static XmlData StaticEquipPage = null;
        /// <summary>
        /// Editing data for StaticDropBook
        /// </summary>
        public static XmlData StaticDropBook = null;

        /// <summary>
        /// Editing data for LocalizedBooks
        /// </summary>
        public static XmlData LocalizedBooks = null;


        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticEquipPage = new XmlData(DM.GameInfos.staticInfos["EquipPage"]);
            StaticDropBook = new XmlData(DM.GameInfos.staticInfos["DropBook"]);

            LocalizedBooks = new XmlData(DM.GameInfos.localizeInfos["Books"]);
        }
    }
}

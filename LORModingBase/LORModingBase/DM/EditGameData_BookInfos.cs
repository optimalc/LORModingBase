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
            LocalizedBooks.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("bookDescList");
        }
    
        /// <summary>
        /// Make new equip page base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewStaticEquipPageBase()
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.staticInfos["EquipPage"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                attributeToCheck: new Dictionary<string, string>() { { "ID", "200001" } });
            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                string RANDOM_BOOK_ID = Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_SPECIAL, DS.FilterDatas.CARD_DIV_FINAL_STORY).ToString();
                xmlDataNodeToAdd.attribute["ID"] = RANDOM_BOOK_ID;
                xmlDataNodeToAdd.SetXmlInfoByPath("TextId", RANDOM_BOOK_ID);

                xmlDataNodeToAdd.SetXmlInfoByPath("Name", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("BookIcon", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("Rarity", "Common");
                xmlDataNodeToAdd.SetXmlInfoByPath("Chapter", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("Episode", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("CharacterSkin", "");

                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/HP", "50");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/Break", "50");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/SpeedMin", "1");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/Speed", "6");

                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/SResist", "Normal");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/PResist", "Normal");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/HResist", "Normal");

                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/SBResist", "Normal");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/PBResist", "Normal");
                xmlDataNodeToAdd.SetXmlInfoByPath("EquipEffect/HBResist", "Normal");
                return xmlDataNodeToAdd;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new static books base by basic node in game data
        /// </summary>
        /// <returns>Created books info</returns>
        public static XmlDataNode MakeNewStaticBooksBase(string bookIdToSet="", string nameToSet="", string desc="")
        {
            List <XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["Books"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                attributeToCheck: new Dictionary<string, string>() { { "BookID", "200001" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["BookID"] = bookIdToSet;
                xmlDataNodeToAdd.SetXmlInfoByPath("BookName", nameToSet);

                if(!string.IsNullOrEmpty(desc))
                {
                    xmlDataNodeToAdd.RemoveXmlInfosByPath("TextList/Desc");

                    foreach (string desPart in desc.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None))
                    {
                        string DESC_TO_ADD = desPart.Replace("\r\n", " ").Replace("\r", "").Replace("\n", "");
                        xmlDataNodeToAdd.AddXmlInfoByPath("TextList/Desc", DESC_TO_ADD);
                    }
                }

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_DropBookInfo
    {
        /// <summary>
        /// Editing data for Drop Book info
        /// </summary>
        public static XmlData StaticDropBookInfo = null;
        /// <summary>
        /// Editing data for Card Drop Table info
        /// </summary>
        public static XmlData StaticCardDropTableInfo = null;
        /// <summary>
        /// Editing data for Localized Drop Book name
        /// </summary>
        public static XmlData LocalizedDropBookName = null;


        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticDropBookInfo = new XmlData(DM.GameInfos.staticInfos["DropBook"]);
            StaticCardDropTableInfo = new XmlData(DM.GameInfos.staticInfos["CardDropTable"]);
            LocalizedDropBookName = new XmlData(DM.GameInfos.localizeInfos["etc"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticDropBookInfo, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticCardDropTableInfo, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedDropBookName, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new drop book info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewDropBookInfoBase()
        {
            List<XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "230002" } });
            if (baseBookUseNode.Count > 0)
            {
                XmlDataNode bookUseIdBase = baseBookUseNode[0].Copy();
                bookUseIdBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(1000000, 9999999).ToString();
                bookUseIdBase.SetXmlInfoByPath("TextId", "");
                bookUseIdBase.SetXmlInfoByPath("BookIcon", "");
                bookUseIdBase.SetXmlInfoByPath("Chapter", "1");
                bookUseIdBase.RemoveXmlInfosByPath("DropItem");
                return bookUseIdBase;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new drop book info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewCardDropTableBase()
        {
            List<XmlDataNode> cardDropTableNodes = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "200001" } });
            if (cardDropTableNodes.Count > 0)
            {
                XmlDataNode caedDropTableBase = cardDropTableNodes[0].Copy();
                caedDropTableBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(1000000, 9999999).ToString();
                caedDropTableBase.RemoveXmlInfosByPath("Card");
                return caedDropTableBase;
            }
            else
                return null;
        }


        /// <summary>
        /// Make new book name base by basic node in game data
        /// </summary>
        /// <returns>Created new book book name</returns>
        public static XmlDataNode MakeNewDropBookNameBase(string dropBookIdToSet = "", string nameToSet = "")
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["etc"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("text",
            attributeToCheck: new Dictionary<string, string>() { { "id", "dropbook_rat" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["id"] = dropBookIdToSet;
                xmlDataNodeToAdd.innerText = nameToSet;

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

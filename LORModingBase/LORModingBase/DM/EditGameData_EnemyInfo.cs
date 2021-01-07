using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_EnemyInfo
    {
        /// <summary>
        /// Editing data for Enemy unit info
        /// </summary>
        public static XmlData StaticEnemyUnitInfo = null;
        /// <summary>
        /// Editing data for Localized characters name
        /// </summary>
        public static XmlData LocalizedCharactersName = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticEnemyUnitInfo = new XmlData(DM.GameInfos.staticInfos["EnemyUnitInfo"]);
            LocalizedCharactersName = new XmlData(DM.GameInfos.localizeInfos["CharactersName"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticEnemyUnitInfo, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedCharactersName, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new enemy unit info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewEnemyUnitInfoBase()
        {
            List<XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["EnemyUnitInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Enemy",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "1" } });
            if (baseBookUseNode.Count > 0)
            {
                XmlDataNode bookUseIdBase = baseBookUseNode[0].Copy();
                bookUseIdBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(100000, 999999).ToString();
                bookUseIdBase.SetXmlInfoByPath("NameID", "");
                bookUseIdBase.SetXmlInfoByPath("MinHeight", "");
                bookUseIdBase.SetXmlInfoByPath("MaxHeight", "");

                bookUseIdBase.SetXmlInfoByPath("BookId", "");
                bookUseIdBase.SetXmlInfoByPath("DeckId", "");

                bookUseIdBase.RemoveXmlInfosByPath("DropTable");
                return bookUseIdBase;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new character name base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewCharactersNameBase(string charIDToSet = "", string charToSet = "")
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["CharactersName"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
            attributeToCheck: new Dictionary<string, string>() { { "ID", "1" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["ID"] = charIDToSet;
                xmlDataNodeToAdd.innerText = charToSet;

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

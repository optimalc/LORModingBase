using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_PassiveInfo
    {
        /// <summary>
        /// Editing data for Passive List
        /// </summary>
        public static XmlData StaticPassiveList = null;
        /// <summary>
        /// Editing data for Localized Passive Desc
        /// </summary>
        public static XmlData LocalizedPassiveDesc = null;


        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticPassiveList = new XmlData(DM.GameInfos.staticInfos["PassiveList"]);
            LocalizedPassiveDesc = new XmlData(DM.GameInfos.localizeInfos["PassiveDesc"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticPassiveList, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedPassiveDesc, "", returnOnlyRelativePath: true));
        }

        /// <summary>
        /// Make new enemy unit info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewPassiveListBase()
        {
            List<XmlDataNode> basePassiveNode = DM.GameInfos.staticInfos["PassiveList"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Passive",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "250005" } });
            if (basePassiveNode.Count > 0)
            {
                XmlDataNode basePassiveBase = basePassiveNode[0].Copy();
                basePassiveBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(1000000, 9999999).ToString();
                basePassiveBase.SetXmlInfoByPath("Rarity", "Common");
                basePassiveBase.SetXmlInfoByPath("Cost", "1");
                return basePassiveBase;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new character name base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewPassiveDescBase(string descID, string descName, string descDetail)
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["PassiveDesc"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "250005" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["ID"] = descID;
                xmlDataNodeToAdd.SetXmlInfoByPath("Name", descName);
                xmlDataNodeToAdd.SetXmlInfoByPath("Desc", descDetail);

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

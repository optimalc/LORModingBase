using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Data that is to be used for editing stage information
    /// </summary>
    class EditGameData_StageInfo
    {
        /// <summary>
        /// Editing data for Stage info
        /// </summary>
        public static XmlData StaticStageInfo = null;
        /// <summary>
        /// Editing data for Localized stage name
        /// </summary>
        public static XmlData LocalizedStageName = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticStageInfo = new XmlData(DM.GameInfos.staticInfos["StageInfo"]);
            LocalizedStageName = new XmlData(DM.GameInfos.localizeInfos["StageName"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticStageInfo, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(LocalizedStageName, "", returnOnlyRelativePath: true));
        }

        /// <summary>
        /// Make new stage info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewStageInfoBase(string stageID)
        {
            List<XmlDataNode> foundBookUseIds = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
             attributeToCheck: new Dictionary<string, string>() { { "ID", stageID } });
            if (foundBookUseIds != null && foundBookUseIds.Count > 0)
                return foundBookUseIds[0].Copy();
            else
            {
                List<XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "2" } });
                if (baseBookUseNode.Count > 0)
                {
                    XmlDataNode bookUseIdBase = baseBookUseNode[0].Copy();
                    bookUseIdBase.attribute["id"] = stageID;
                    bookUseIdBase.SetXmlInfoByPath("Name", "");
                    bookUseIdBase.SetXmlInfoByPath("Chapter", "");
                    bookUseIdBase.SetXmlInfoByPath("StoryType", "");

                    bookUseIdBase.RemoveXmlInfosByPath("Wave");
                    bookUseIdBase.RemoveXmlInfosByPath("Invitation/Book");

                    bookUseIdBase.ActionXmlDataNodesByPath("Story", (XmlDataNode storyNode) =>
                    {
                        storyNode.innerText = "";
                    });
                    return bookUseIdBase;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Make new localize stage name base by basic node in game data
        /// </summary>
        /// <returns>Created books info</returns>
        public static XmlDataNode MakeNewStageNameBase(string stageIDToSet = "", string nameToSet = "")
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["StageName"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                attributeToCheck: new Dictionary<string, string>() { { "ID", "2" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["ID"] = stageIDToSet;
                xmlDataNodeToAdd.innerText = nameToSet;

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

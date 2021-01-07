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
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedStageName, "", returnOnlyRelativePath: true));
        }

        /// <summary>
        /// Make new stage info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewStageInfoBase()
        {
            List<XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                    attributeToCheck: new Dictionary<string, string>() { { "id", "2" } });
            if (baseBookUseNode.Count > 0)
            {
                XmlDataNode bookUseIdBase = baseBookUseNode[0].Copy();
                bookUseIdBase.attribute["id"] = Tools.MathTools.GetRandomNumber(1000000, 9999999).ToString();
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
    
        /// <summary>
        /// Make new wav info base by basic node in game data
        /// </summary>
        /// <returns></returns>
        public static XmlDataNode MakeNewWaveInfoBase()
        {
            List<XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                   attributeToCheck: new Dictionary<string, string>() { { "id", "2" } });
            if (baseBookUseNode.Count > 0)
            {
                XmlDataNode STAGE_NODE_TO_USE = baseBookUseNode[0];

                List<XmlDataNode> baseWaveNode = STAGE_NODE_TO_USE.GetXmlDataNodesByPath("Wave");
                if (baseWaveNode.Count > 0)
                {
                    XmlDataNode baseWaveNodeToUse = baseWaveNode[0].Copy();
                    baseWaveNodeToUse.SetXmlInfoByPath("Formation", "");
                    baseWaveNodeToUse.RemoveXmlInfosByPath("Unit");
                    return baseWaveNodeToUse;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}

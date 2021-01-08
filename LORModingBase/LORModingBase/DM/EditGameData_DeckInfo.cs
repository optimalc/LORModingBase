using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_DeckInfo
    {
        /// <summary>
        /// Editing data for Deck info
        /// </summary>
        public static XmlData StaticDeckInfo = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticDeckInfo = new XmlData(DM.GameInfos.staticInfos["Deck"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticDeckInfo, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new stage info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewDeckInfoBase()
        {
            List<XmlDataNode> baseDeckNode = DM.GameInfos.staticInfos["Deck"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Deck",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "200001" } });
            if (baseDeckNode.Count > 0)
            {
                XmlDataNode deckNodeBase = baseDeckNode[0].Copy();
                deckNodeBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(999999, 5999999).ToString();
                deckNodeBase.RemoveXmlInfosByPath("Card");
                deckNodeBase.RemoveXmlInfosByPath("Random");
                return deckNodeBase;
            }
            else
                return null;
        }
    }
}

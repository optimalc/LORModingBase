using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LORModingBase.CustomExtensions;

namespace LORModingBase.DM
{
    /// <summary>
    /// Data that is to be used for editing card information
    /// </summary>
    class EditGameData_CardInfos
    {
        /// <summary>
        /// Editing data for Card
        /// </summary>
        public static XmlData StaticCard = null;
        /// <summary>
        /// Editing data for Card drop table
        /// </summary>
        public static XmlData StaticCardDropTable = null;
        /// <summary>
        /// Editing data for LocalizedCards
        /// </summary>
        public static XmlData LocalizedBattleCards = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            StaticCard = new XmlData(DM.GameInfos.staticInfos["Card"]);
            StaticCardDropTable = new XmlData(DM.GameInfos.staticInfos["CardDropTable"]);

            LocalizedBattleCards = new XmlData(DM.GameInfos.localizeInfos["BattlesCards"]);
            LocalizedBattleCards.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("cardDescList");


            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticCard, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetStaticPathToSave(StaticCardDropTable, "", returnOnlyRelativePath: true));
            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedBattleCards, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new card base by basic node in game data
        /// </summary>
        /// <returns>Created new Card</returns>
        public static XmlDataNode MakeNewCardBase()
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                attributeToCheck: new Dictionary<string, string>() { { "ID", "100001" } });
            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                string RANDOM_CARD_ID = Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_SPECIAL, DS.FilterDatas.CARD_DIV_FINAL_STORY).ToString();
                xmlDataNodeToAdd.attribute["ID"] = RANDOM_CARD_ID;

                xmlDataNodeToAdd.SetXmlInfoByPath("Name", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("Artwork", "");
                xmlDataNodeToAdd.SetXmlInfoByPath("Rarity", "Common");
                xmlDataNodeToAdd.RemoveXmlInfosByPath("BehaviourList/Behaviour");
                return xmlDataNodeToAdd;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new dice base by basic node in game data
        /// </summary>
        /// <returns></returns>
        public static XmlDataNode MakeNewDiceBase()
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
              attributeToCheck: new Dictionary<string, string>() { { "ID", "100001" } });
            if (foundXmlDataNodes.Count > 0)
            {
                List<XmlDataNode> foundDiceXmlDataNodes = foundXmlDataNodes[0].GetXmlDataNodesByPathWithXmlInfo("BehaviourList/Behaviour");
                if (foundDiceXmlDataNodes.Count > 0)
                {
                    XmlDataNode diceNodeToAdd = foundDiceXmlDataNodes[0].Copy();
                    diceNodeToAdd.attribute["Min"] = "1";
                    diceNodeToAdd.attribute["Dice"] = "6";
                    diceNodeToAdd.attribute["Type"] = "Atk";
                    diceNodeToAdd.attribute["Detail"] = "Slash";
                    diceNodeToAdd.attribute["Motion"] = "H";
                    diceNodeToAdd.attribute["EffectRes"] = "";
                    diceNodeToAdd.attribute["Script"] = "";
                    diceNodeToAdd.attribute["Desc"] = "";

                    return diceNodeToAdd;
                }
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Make new static card drop table with given bookUseID
        /// </summary>
        /// <param name="CardDropTableID">Card drop table ID to use</param>
        /// <returns>If given card drop table id is already used in game. Load it</returns>
        public static XmlDataNode MakeNewStaticCardDropTableBase(string CardDropTableID = "")
        {
            List<XmlDataNode> foundCardDropTableIDs = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                attributeToCheck: new Dictionary<string, string>() { { "ID", CardDropTableID } });
            if (foundCardDropTableIDs != null && foundCardDropTableIDs.Count > 0)
                return foundCardDropTableIDs[0].Copy();
            else
            {
                List<XmlDataNode> baseCardDropTableNode = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "200001" } });
                if (baseCardDropTableNode.Count > 0)
                {
                    XmlDataNode cardDropTableNode = baseCardDropTableNode[0].Copy();
                    cardDropTableNode.RemoveXmlInfosByPath("Card");
                    return cardDropTableNode;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Make new localize battle cards by basic node in game data
        /// </summary>
        /// <returns>Created localized base card info</returns>
        public static XmlDataNode MakeNewLocalizedBattleCardsBase(string cardIdToSet = "", string nameToSet = "")
        {
            List<XmlDataNode> foundXmlDataNodes = DM.GameInfos.localizeInfos["BattlesCards"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                attributeToCheck: new Dictionary<string, string>() { { "ID", "1" } });

            if (foundXmlDataNodes.Count > 0)
            {
                XmlDataNode xmlDataNodeToAdd = foundXmlDataNodes[0].Copy();

                xmlDataNodeToAdd.attribute["ID"] = cardIdToSet;
                xmlDataNodeToAdd.SetXmlInfoByPath("LocalizedName", nameToSet);

                return xmlDataNodeToAdd;
            }
            else
                return null;
        }
    }
}

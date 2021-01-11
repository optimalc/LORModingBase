using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_CardAbilityInfo
    {
        /// <summary>
        /// Editing data for LocalizedCardAbility
        /// </summary>
        public static XmlData LocalizedCardAbility = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            LocalizedCardAbility = new XmlData(DM.GameInfos.localizeInfos["BattleCardAbilities"]);

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedCardAbility, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new stage info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewCardAbilityInfoBase()
        {
            List<XmlDataNode> baseAbilityNode = DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BattleCardAbility",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "vulnerable1atk" } });
            if (baseAbilityNode.Count > 0)
            {
                XmlDataNode abilityNodeBase = baseAbilityNode[0].Copy();
                abilityNodeBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(999999, 9999999).ToString();
                abilityNodeBase.SetXmlInfoByPath("Desc", "");
                return abilityNodeBase;
            }
            else
                return null;
        }
    }
}

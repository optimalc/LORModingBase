using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    class EditGameData_Buff
    {
        /// <summary>
        /// Editing data for LocalizedBuff
        /// </summary>
        public static XmlData LocalizedBuff = null;

        /// <summary>
        /// Initial each XmlDatas
        /// </summary>
        public static void InitDatas()
        {
            LocalizedBuff = new XmlData(DM.GameInfos.localizeInfos["EffectTexts"]);
            LocalizedBuff.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("effectTextList");

            MainWindow.EDITOR_SELECTION_MENU.Add(DM.Config.GetLocalizePathToSave(LocalizedBuff, "", returnOnlyRelativePath: true));
        }


        /// <summary>
        /// Make new buff info base by basic node in game data
        /// </summary>
        /// <returns>Created new equip page</returns>
        public static XmlDataNode MakeNewBuffInfoBase()
        {
            List<XmlDataNode> baseBuffNode = DM.GameInfos.localizeInfos["EffectTexts"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("effectTextList/BattleEffectText",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", "Burn" } });
            if (baseBuffNode.Count > 0)
            {
                XmlDataNode BuffNodeBase = baseBuffNode[0].Copy();
                BuffNodeBase.attribute["ID"] = Tools.MathTools.GetRandomNumber(999999, 9999999).ToString();
                BuffNodeBase.SetXmlInfoByPath("Name", "");
                BuffNodeBase.SetXmlInfoByPath("Desc", "");
                return BuffNodeBase;
            }
            else
                return null;
        }
    }
}

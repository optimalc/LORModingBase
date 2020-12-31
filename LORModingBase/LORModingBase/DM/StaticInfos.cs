using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game episode data managerment
    /// </summary>
    class StaticInfos
    {
        /// <summary>
        /// Loaded stage infos
        /// </summary>
        public static List<DS.StageInfo> stageInfos = new List<DS.StageInfo>();

        /// <summary>
        /// Loaded passive infos
        /// </summary>
        public static Dictionary<string, List<DS.PassiveInfo>> passiveInfos = new Dictionary<string, List<DS.PassiveInfo>>();

        /// <summary>
        /// Skin infos
        /// </summary>
        public static Dictionary<string, List<string>> skinInfos = new Dictionary<string, List<string>>();

        /// <summary>
        /// Book icon infos
        /// </summary>
        public static List<DS.BookIconInfo> bookIconInfos = new List<DS.BookIconInfo>();

        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadDatas_StageInfo();
            LoadDatas_SkinAndBookIconInfo();
            LoadDatas_PassiveInfo();
        }

        /// <summary>
        /// Load stage info datas
        /// </summary>
        public static void LoadDatas_StageInfo()
        {
            stageInfos.Clear();
            XmlNodeList stageNodeList =  Tools.XmlFile.SelectNodeLists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\StageInfo\\StageInfo.txt",
                "//Stage");

            foreach(XmlNode stageNode in stageNodeList)
            {
                if (stageNode.Attributes["id"] == null 
                    || stageNode["Chapter"] == null
                    || stageNode["Name"] == null)
                    continue;

                stageInfos.Add(new DS.StageInfo()
                {
                    stageID = stageNode.Attributes["id"].Value,
                    Chapter = stageNode["Chapter"].InnerText,
                    stageDoc = stageNode["Name"].InnerText
                });
            }
        }
    
        /// <summary>
        /// Load skin and book icon datas
        /// </summary>
        public static void LoadDatas_SkinAndBookIconInfo()
        {
            #region Load book icon infos
            bookIconInfos.Clear();
            string dropBookInfoPath = $"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\etc\\KR_Dropbook.txt";
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\DropBook").ToList().ForEach((string dropBookPath) =>
            {
                XmlNodeList bookUseNodeList = Tools.XmlFile.SelectNodeLists(dropBookPath, "//BookUse");
                foreach (XmlNode bookUseNode in bookUseNodeList)
                {
                    if (bookUseNode["TextId"] == null || bookUseNode["BookIcon"] == null || bookUseNode["Chapter"] == null)
                        continue;

                    XmlNode dropBookInfoNode = Tools.XmlFile.SelectSingleNode(dropBookInfoPath, $"//text[@id='{bookUseNode["TextId"].InnerText}']");
                    if (dropBookInfoNode == null)
                        continue;

                    bookIconInfos.Add(new DS.BookIconInfo()
                    {
                        iconName = bookUseNode["BookIcon"].InnerText,
                        iconDesc = dropBookInfoNode.InnerText,
                        chapter = bookUseNode["Chapter"].InnerText
                    });
                }
            });
            #endregion
            #region Load skin icon infos
            skinInfos.Clear();
            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\EquipPage").ToList().ForEach((string eqPath) =>
            {
                XmlNodeList bookNodeList = Tools.XmlFile.SelectNodeLists(eqPath, "//Book");

                List<string> skins = new List<string>();
                foreach (XmlNode bookNode in bookNodeList)
                {
                    if (bookNode["Name"] == null && bookNode["CharacterSkin"] != null)
                        skins.Add($":{bookNode["CharacterSkin"].InnerText}");
                    else if (bookNode["CharacterSkin"] != null)
                        skins.Add($"{bookNode["Name"].InnerText}:{bookNode["CharacterSkin"].InnerText}");
                }
                string PATH_TO_USE = eqPath.Split('\\').Last().Split('.')[0];
                if (skins.Count > 0) skinInfos[PATH_TO_USE] = skins;
            }); 
            #endregion

        }
    
        /// <summary>
        /// Load passive info datas
        /// </summary>
        public static void LoadDatas_PassiveInfo()
        {
            passiveInfos.Clear();

            Directory.GetFiles($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_LOCALIZE}\\kr\\PassiveDesc").ToList().ForEach((string pvPath) =>
            {
                XmlNodeList passiveDescNodeList = Tools.XmlFile.SelectNodeLists(pvPath, "//PassiveDesc");

                List<DS.PassiveInfo> passives = new List<DS.PassiveInfo>();
                foreach (XmlNode passiveDescNode in passiveDescNodeList)
                {
                    if (passiveDescNode.Attributes["ID"] == null
                        || passiveDescNode["Name"] == null
                        || passiveDescNode["Desc"] == null)
                            continue;

                    passives.Add(new DS.PassiveInfo()
                    {
                        passiveID = passiveDescNode.Attributes["ID"].Value,
                        passiveName = passiveDescNode["Name"].InnerText,
                        passiveDes = passiveDescNode["Desc"].InnerText
                    });
                }
                string PATH_TO_USE = pvPath.Split('\\').Last().Split('.')[0];
                if(passives.Count > 0) passiveInfos[PATH_TO_USE] = passives;
            });
        }
    }
}

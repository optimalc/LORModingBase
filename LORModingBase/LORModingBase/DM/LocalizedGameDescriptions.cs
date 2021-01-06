using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Get localized description
    /// </summary>
    class LocalizedGameDescriptions
    {
        /// <summary>
        /// Get description of Stage
        /// </summary>
        /// <param name="stageID">Stage ID to get</param>
        /// <returns>Stage desciption</returns>
        public static string GetDescriptionForStage(string stageID)
        {
            if (string.IsNullOrEmpty(stageID)) return "";
            string LOC_STAGE_NAME = DM.GameInfos.localizeInfos["StageName"].rootDataNode.GetInnerTextByAttributeWithPath("Name", "ID", stageID, $"STAGE ID : {stageID}");
            return LOC_STAGE_NAME;
        }

        /// <summary>
        /// Get description of Chpater
        /// </summary>
        /// <param name="chapterNum">Chapter num to get</param>
        /// <returns>Chapter description</returns>
        public static string GetDescriptionForChapter(string chapterNum)
        {
            if (string.IsNullOrEmpty(chapterNum)) return "";
            string LOC_CHAPTER_NAME = DM.GameInfos.localizeInfos["etc"].rootDataNode.GetInnerTextByAttributeWithPath("text", "id", $"ui_maintitle_citystate_{chapterNum}", $"Chapter : {chapterNum}");
            return LOC_CHAPTER_NAME;
        }
    }

    /// <summary>
    /// Fully constructed localized description
    /// </summary>
    class FullyLoclalizedGameDescriptions
    {
        /// <summary>
        /// Get full description of stage
        /// </summary>
        /// <param name="stageID">Stage ID to use</param>
        /// <returns>Full description for stage</returns>
        public static string GetFullDescriptionForStage(string stageID)
        {
            List<XmlDataNode> foundDataNodes = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage", 
                attributeToCheck:new Dictionary<string, string>() { {"id", stageID } });
            if(foundDataNodes.Count > 0)
            {
                XmlDataNode STAGE_NODE = foundDataNodes[0];
                string STAGE_ID = STAGE_NODE.GetAttributesSafe("id");
                string CHPATER_NUM = STAGE_NODE.GetInnerTextByPath("Chapter");

                string STAGE_DES = DM.LocalizedGameDescriptions.GetDescriptionForStage(STAGE_ID);
                string CHAPTER_DES = DM.LocalizedGameDescriptions.GetDescriptionForChapter(CHPATER_NUM);
                return $"{CHAPTER_DES} / {STAGE_DES}:{STAGE_ID}";
            }
            else
                return $"Stage ID : {stageID}";
        }
    }
}

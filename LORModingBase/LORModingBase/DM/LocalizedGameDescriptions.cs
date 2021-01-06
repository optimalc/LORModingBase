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
            string LOC_CHAPTER_NAME = GetDescriptionForETC($"ui_maintitle_citystate_{chapterNum}");
            return (string.IsNullOrEmpty(LOC_CHAPTER_NAME) ? $"Chpater : {chapterNum}" : LOC_CHAPTER_NAME);
        }

        /// <summary>
        /// Get description of Book
        /// </summary>
        /// <param name="bookID">Book id to use</param>
        /// <returns>Book description</returns>
        public static string GetDescriptionForBook(string bookID)
        {
            if (string.IsNullOrEmpty(bookID)) return "";
            List<XmlDataNode> bookNodes = DM.GameInfos.localizeInfos["BattlesCards"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                attributeToCheck: new Dictionary<string, string>() { { "ID", bookID } });
            if (bookNodes.Count > 0)
            {
                string LOCAL_BOOK_NAME = bookNodes[0].GetInnerTextByPath("LocalizedName");
                return string.IsNullOrEmpty(LOCAL_BOOK_NAME) ? $"Book ID : {bookID}" : LOCAL_BOOK_NAME;
            }
            else
                return $"Book ID : {bookID}";
        }
    
        /// <summary>
        /// Get description of ETC name
        /// </summary>
        /// <param name="etcName">Name in etc</param>
        /// <returns>Description</returns>
        public static string GetDescriptionForETC(string etcName)
        {
            if (string.IsNullOrEmpty(etcName)) return "";
            return DM.GameInfos.localizeInfos["etc"].rootDataNode.GetInnerTextByAttributeWithPath("text", "id", etcName, $"Etc : {etcName}");
        }
    
        /// <summary>
        /// Get description for Books.txt
        /// </summary>
        /// <param name="bookID">Books.txt ID</param>
        /// <returns>Books description</returns>
        public static string GetDescriptionForBooks(string bookID)
        {
            if (string.IsNullOrEmpty(bookID)) return "";
            List<XmlDataNode> BooksNodes = DM.GameInfos.localizeInfos["Books"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                attributeToCheck: new Dictionary<string, string>() { { "BookID", bookID } });
            if (BooksNodes.Count > 0)
            {
                string BOOK_NAME = BooksNodes[0].GetInnerTextByPath("BookName");
                return string.IsNullOrEmpty(BOOK_NAME) ? $"Book ID : {bookID}" : BOOK_NAME;
            }
            else
                return $"Book ID : {BooksNodes}";
        }
        
        /// <summary>
        /// Get description for character ID
        /// </summary>
        /// <param name="characterID">Character ID to use</param>
        /// <returns>Character name description</returns>
        public static string GetDescriptionForCharacter(string characterID)
        {
            if (string.IsNullOrEmpty(characterID)) return "";
            return DM.GameInfos.localizeInfos["CharactersName"].rootDataNode.GetInnerTextByAttributeWithPath("Name", "ID", characterID, $"ChID : {characterID}");
        }
    
        /// <summary>
        /// Get description for passive ID
        /// </summary>
        /// <param name="passiveID">Passive ID to use</param>
        /// <returns>Passive des</returns>
        public static string GetDescriptionForPassive(string passiveID, bool nameOnly = false)
        {
            if (string.IsNullOrEmpty(passiveID)) return "";
            List<XmlDataNode> PassiveNodes = DM.GameInfos.localizeInfos["PassiveDesc"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                attributeToCheck: new Dictionary<string, string>() { { "ID", passiveID } });
            if (PassiveNodes.Count > 0)
            {
                string PASSIVE_NAME = PassiveNodes[0].GetInnerTextByPath("Name");
                string PASSIVE_DESC = PassiveNodes[0].GetInnerTextByPath("Desc");
                if (string.IsNullOrEmpty(PASSIVE_NAME)) return $"Passive ID : {passiveID}";
                if (nameOnly)
                    return PASSIVE_NAME;
                else
                    return $"{PASSIVE_NAME} / {PASSIVE_DESC}";
            }
            else
                return $"Passive ID : {passiveID}";
        }
    
        /// <summary>
        /// Get description for Card ID
        /// </summary>
        /// <param name="cardID">Card id to use</param>
        /// <returns>Card description</returns>
        public static string GetDecriptionForCard(string cardID)
        {
            if (string.IsNullOrEmpty(cardID)) return "";
            List<XmlDataNode> cardNodes = DM.GameInfos.localizeInfos["BattlesCards"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                attributeToCheck: new Dictionary<string, string>() { { "ID", cardID } });
            if (cardNodes.Count > 0)
            {
                string CARD_NAME = cardNodes[0].GetInnerTextByPath("LocalizedName");
                if (string.IsNullOrEmpty(CARD_NAME)) return $"Card ID : {cardID}";
                else return CARD_NAME;
            }
            else
                return $"Card ID : {cardID}";
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

                string STAGE_DES = LocalizedGameDescriptions.GetDescriptionForStage(STAGE_ID);
                string CHAPTER_DES = LocalizedGameDescriptions.GetDescriptionForChapter(CHPATER_NUM);
                return $"{CHAPTER_DES} / {STAGE_DES}:{STAGE_ID}";
            }
            else
                return $"Stage ID : {stageID}";
        }
    
        /// <summary>
        /// Get full description of Book
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        public static string GetFullDescriptionForBook(string bookID)
        {
            List<XmlDataNode> foundDataNodes = DM.GameInfos.staticInfos["EquipPage"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                attributeToCheck: new Dictionary<string, string>() { { "ID", bookID } });
            if (foundDataNodes.Count > 0)
            {
                XmlDataNode STAGE_NODE = foundDataNodes[0];
                string BOOK_ID = STAGE_NODE.GetAttributesSafe("ID");
                string CHPATER_NUM = STAGE_NODE.GetInnerTextByPath("Chapter");

                string BOOK_DES = LocalizedGameDescriptions.GetDescriptionForBook(BOOK_ID);
                string CHAPTER_DES = LocalizedGameDescriptions.GetDescriptionForChapter(CHPATER_NUM);
                return $"{CHAPTER_DES} / {BOOK_DES}:{BOOK_ID}";
            }
            else
                return $"Book ID : {bookID}";
        }
    
        /// <summary>
        /// Get full description for DropBook
        /// </summary>
        /// <param name="dropBookID">Drop book ID to use</param>
        /// <returns>Dropbook description</returns>
        public static string GetFullDescriptionForDropBook(string dropBookID)
        {
            List<XmlDataNode> foundDataNodes = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                attributeToCheck: new Dictionary<string, string>() { { "ID", dropBookID } });
            if (foundDataNodes.Count > 0)
            {
                XmlDataNode DROP_BOOK_NODE = foundDataNodes[0];
                string DROP_BOOK_DES = LocalizedGameDescriptions.GetDescriptionForETC(DROP_BOOK_NODE.GetInnerTextByPath("TextId"));
                string CHAPTER_DES = LocalizedGameDescriptions.GetDescriptionForChapter(DROP_BOOK_NODE.GetInnerTextByPath("Chapter"));

                return $"{CHAPTER_DES} / {DROP_BOOK_DES}:{dropBookID}";
            }
            else
                return $"Drop Book ID : {dropBookID}";
        }
    
        /// <summary>
        /// Get full description for Card
        /// </summary>
        /// <param name="cardID">Card id to use</param>
        /// <returns>Card full desscription</returns>
        public static string GetFullDescriptionForCard(string cardID)
        {
            List<XmlDataNode> foundDataNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                attributeToCheck: new Dictionary<string, string>() { { "ID", cardID } });
            if (foundDataNodes.Count > 0)
            {
                XmlDataNode CARD_NODE = foundDataNodes[0];
                string CARD_NAME_DES = LocalizedGameDescriptions.GetDecriptionForCard(CARD_NODE.GetAttributesSafe("ID"));
                string CHAPTER_DES = LocalizedGameDescriptions.GetDescriptionForChapter(CARD_NODE.GetInnerTextByPath("Chapter"));

                return $"{CHAPTER_DES} / {CARD_NAME_DES}:{cardID}";
            }
            else
                return $"Card ID : {cardID}";
        }
    }
}

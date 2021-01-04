using System.Collections.Generic;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game episode data managerment
    /// </summary>
    partial class StaticInfos
    {
        public static XmlData bookXmlData = null;

        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            #region Load book datas
            new XmlData($"{DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC}\\Card").SaveNodeData("textData.txt");
            #endregion

            #region Load card data -> StaticInfo_Cards.cs
            LoadData_CardEffect();
            LoadData_CardsInfo();
            #endregion
            #region Load book datas -> StaticInfo_Books.cs
            LoadDatas_PassiveInfo();
            LoadDatas_StageInfo();
            LoadDatas_SkinAndBookIconInfo(); 
            #endregion
            LoadData_Dropbooks(); // Card drop book load
        }
    }
}

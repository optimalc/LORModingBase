namespace LORModingBase.DM
{
    /// <summary>
    /// Game episode data managerment
    /// </summary>
    partial class StaticInfos
    {
        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadData_UITexts();

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

        public static void LoadData_UITexts()
        {
            XmlData xmlData = new DM.XmlData($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}\\Card\\CardInfo_Basic.txt");
            new DM.XmlData(xmlData).SaveNodeData("./testXml.txt");
        }
    }
}

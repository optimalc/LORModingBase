using System.Collections.Generic;

namespace LORModingBase.DM
{
    /// <summary>
    /// Game static datas management
    /// </summary>
    partial class StaticInfos
    {
        /// <summary>
        /// Equip page static data
        /// </summary>
        public static XmlData XmlData_EquipPage = null;
        /// <summary>
        /// Stage info static data
        /// </summary>
        public static XmlData XmlData_StageInfo = null;
        /// <summary>
        /// DropBook static data
        /// </summary>
        public static XmlData XmlData_DropBook = null;


        /// <summary>
        /// Load all static datas
        /// </summary>
        public static void LoadAllDatas()
        {
            LoadBookStaticInfos();

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


        /// <summary>
        /// Load static infos for book
        /// </summary>
        public static void LoadBookStaticInfos()
        {
            XmlData_EquipPage = new XmlData(DM.Config.GAME_RESOURCE_PATHS.STATIC_EquipPage);
            XmlData_StageInfo = new XmlData(DM.Config.GAME_RESOURCE_PATHS.STATIC_StageInfo);
            XmlData_DropBook = new XmlData(DM.Config.GAME_RESOURCE_PATHS.STATIC_DropBook);
        }
    }
}

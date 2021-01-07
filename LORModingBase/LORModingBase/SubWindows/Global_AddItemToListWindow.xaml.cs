using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_AddItemToListWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_AddItemToListWindow : Window
    {
        Action<string> afterAddItem = null;
        Action<string> afterDeleteItem = null;
        List<string> selectedItems = null;
        List<string> selectItems = null;

        #region Constructor and preset
        public Global_AddItemToListWindow(Action<string> afterAddItem, Action<string> afterDeleteItem, List<string> selectedItems, List<string> searchTypes, List<string> selectItems,
            string windowTitle= "사용할 내용을 더블클릭", string helpMessage="※ 사용할 내용을 더블클릭 하세요", 
            string ItemHelpMessage="얻어진 내용들", string selectedItemsHelpMessage= "선택된 항목들" )
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterAddItem = afterAddItem;
            this.afterDeleteItem = afterDeleteItem;

            this.selectedItems = selectedItems;
            this.selectItems = selectItems;

            InitLbxSearchType(searchTypes);
            InitLbxSelectedItems();

            this.Title = windowTitle;
            LblHelpMessage.Content = helpMessage;
            LblItemHelpMessage.Content = ItemHelpMessage;
            LblSelectedItemLists.Content = selectedItemsHelpMessage;
        }

        public Global_AddItemToListWindow(Action<string> afterAddItem, Action<string> afterDeleteItem, List<string> selectedItems, AddItemToListWindow_PRESET preset)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterAddItem = afterAddItem;
            this.afterDeleteItem = afterDeleteItem;
            this.selectedItems = selectedItems;

            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (preset)
            {
                case AddItemToListWindow_PRESET.ONLY_CARD:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"ONLY_CARD_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        if(cardNode.GetInnerTextByPath("Option") == "OnlyPage")
                        {
                            string CUSTOM_FILTER_DES = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM");
                            string CUSTOM_NAME = cardNode.GetInnerTextByPath("Name");
                            string CUSTOM_ID = cardNode.GetAttributesSafe("ID");
                            if(!string.IsNullOrEmpty(CUSTOM_ID))
                                selectItems.Add($"{CUSTOM_FILTER_DES} {CUSTOM_NAME}:{CUSTOM_ID}");
                        }
                    });
                    searchTypes.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM"));
                    #endregion
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        if (cardNode.GetInnerTextByPath("Option") == "OnlyPage")
                        {
                            string CARD_ID = cardNode.GetAttributesSafe("ID");
                            if (!string.IsNullOrEmpty(CARD_ID))
                                selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(CARD_ID));
                        }
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case AddItemToListWindow_PRESET.DROP_BOOK:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DROP_BOOK_TITLE");
                    DM.GameInfos.staticInfos["DropBook"].rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode bookUseNode) =>
                    {
                        string BOOK_USE_ID = bookUseNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(BOOK_USE_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(BOOK_USE_ID));
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;

                case AddItemToListWindow_PRESET.DROP_TABLE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DROP_TABLE_TITLE");
                    DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.ActionXmlDataNodesByPath("DropTable", (DM.XmlDataNode cardDropTableID) =>
                    {
                        string DROP_TABLE_ID = cardDropTableID.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(DROP_TABLE_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(DROP_TABLE_ID));
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case AddItemToListWindow_PRESET.STAGES:
                    DM.GameInfos.staticInfos["StageInfo"].rootDataNode.ActionXmlDataNodesByPath("Stage", (DM.XmlDataNode stageID) =>
                    {
                        string STAGE_ID = stageID.GetAttributesSafe("id");
                        if (!string.IsNullOrEmpty(STAGE_ID))
                            selectItems.Add(STAGE_ID);
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
            }

            InitLbxSearchType(searchTypes);
            InitLbxSelectedItems();
        }
        #endregion

        #region Init controls
        private void InitLbxSearchType(List<string> searchTypes)
        {
            LbxSearchType.Items.Clear();
            LbxSearchType.Items.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"SEARCH_NO_FILTER"));
            searchTypes.ForEach((string searchType) =>
            {
                LbxSearchType.Items.Add(searchType);
            });

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitSearchItems();
            }
        }

        private void InitLbxSelectedItems()
        {
            LbxSelectedItems.Items.Clear();
            selectedItems.ForEach((string selectedItem) =>
            {
                LbxSelectedItems.Items.Add(selectedItem);
            });
        }

        private void InitSearchItems()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxItems.Items.Clear();
                foreach (string selectItem in selectItems)
                {
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !selectItem.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;
                    if (LbxSearchType.SelectedIndex == 0)
                        LbxItems.Items.Add(selectItem);
                    else
                    {
                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            default:
                                if (selectItem.ToLower().Contains(LbxSearchType.SelectedItem.ToString().ToLower()))
                                    LbxItems.Items.Add(selectItem);
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Add or Remove item functions
        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterAddItem(LbxItems.SelectedItem.ToString().Split(':').Last());
                selectedItems.Add(LbxItems.SelectedItem.ToString());
                InitLbxSelectedItems();
            }
        }

        private void LbxSelectedItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxSelectedItems.SelectedItem != null)
            {
                afterDeleteItem(LbxSelectedItems.SelectedItem.ToString().Split(':').Last());
                selectedItems.Remove(LbxSelectedItems.SelectedItem.ToString());
                InitLbxSelectedItems();
            }
        }
        #endregion

        #region Search help methodes
        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitSearchItems();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitSearchItems();
        }
        #endregion

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public enum AddItemToListWindow_PRESET
    {
        ONLY_CARD,
        DROP_BOOK,

        DROP_TABLE,
        STAGES
    };
}
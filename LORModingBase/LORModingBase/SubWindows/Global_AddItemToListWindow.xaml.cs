using System;
using System.Collections.Generic;
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
            this.afterAddItem = afterAddItem;
            this.afterDeleteItem = afterDeleteItem;
            this.selectedItems = selectedItems;

            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (preset)
            {
                case AddItemToListWindow_PRESET.ONLY_CARD:
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        string CARD_ID = cardNode.GetAttributesSafe("ID");
                        if(!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add(CARD_ID);
                    });
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
            LbxSearchType.Items.Add("필터 없음");
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
                    switch (LbxSearchType.SelectedItem.ToString())
                    {
                        case "필터 없음":
                            LbxItems.Items.Add(selectItem);
                            break;
                        default:
                            if (selectItem.ToLower().Contains(LbxSearchType.SelectedIndex.ToString().ToLower()))
                                LbxItems.Items.Add(selectItem);
                            break;
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
                afterAddItem(LbxItems.SelectedItem.ToString());
                selectedItems.Add(LbxItems.SelectedItem.ToString());
                InitLbxSelectedItems();
            }
        }

        private void LbxSelectedItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxSelectedItems.SelectedItem != null)
            {
                afterDeleteItem(LbxSelectedItems.SelectedItem.ToString());
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
        ONLY_CARD
    };
}
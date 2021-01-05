using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_InputInfoWithSearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_InputInfoWithSearchWindow : Window
    {
        Action<string> afterSelectItem = null;
        List<string> selectItems = null;

        #region Constructor and preset
        public Global_InputInfoWithSearchWindow(Action<string> afterSelectItem, List<string> searchTypes, List<string> selectItems,
            string windowTitle= "사용할 내용을 더블클릭", string helpMessage="※ 사용할 내용을 더블클릭 하세요", string ItemHelpMessage="얻어진 내용들" )
        {
            InitializeComponent();
            this.afterSelectItem = afterSelectItem;
            this.selectItems = selectItems;
            InitLbxSearchType(searchTypes);

            this.Title = windowTitle;
            LblHelpMessage.Content = helpMessage;
            LblItemHelpMessage.Content = ItemHelpMessage;
        }

        public Global_InputInfoWithSearchWindow(Action<string> afterSelectItem, InputInfoWithSearchWindow_PRESET preset)
        {
            InitializeComponent();
            this.afterSelectItem = afterSelectItem;
            switch (preset)
            {
                case InputInfoWithSearchWindow_PRESET.EPISODE:
                    break;
            }
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
                InitLbxBookPassive();
            }
        }

        private void InitLbxBookPassive()
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

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterSelectItem(LbxItems.ToString());
                this.Close();
            }
        }
        #region Search help methodes
        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxBookPassive();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitLbxBookPassive();
        } 
        #endregion
    }

    public enum InputInfoWithSearchWindow_PRESET
    {
        EPISODE
    };
}
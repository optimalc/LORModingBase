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
            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (preset)
            {
                case InputInfoWithSearchWindow_PRESET.EPISODE:
                    DM.GameInfos.staticInfos["StageInfo"].rootDataNode.ActionXmlDataNodesByPath("Stage", (DM.XmlDataNode stageNode) =>
                    {
                        string STAGE_ID = stageNode.GetAttributesSafe("id");
                        if(!string.IsNullOrEmpty(STAGE_ID))
                        {
                            if (Convert.ToInt32(STAGE_ID) < DS.FilterDatas.STAGEINFO_DIV_NOT_CREATURE)
                                selectItems.Add(STAGE_ID);
                        }
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.BOOK_ICON:
                    DM.GameInfos.staticInfos["DropBook"].rootDataNode.ActionXmlDataNodesByPath("BookUse/BookIcon", (DM.XmlDataNode bookIconNode) =>
                    {
                        string ICON_NAME = bookIconNode.GetInnerTextSafe();
                        if (!string.IsNullOrEmpty(ICON_NAME))
                            selectItems.Add(ICON_NAME);
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN:
                    DM.GameInfos.staticInfos["EquipPage"].rootDataNode.ActionXmlDataNodesByPath("Book/CharacterSkin", (DM.XmlDataNode skinNode) =>
                    {
                        string SKIN_NAME = skinNode.GetInnerTextSafe();
                        if (!string.IsNullOrEmpty(SKIN_NAME))
                            selectItems.Add(SKIN_NAME);
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.PASSIVE:
                    DM.GameInfos.staticInfos["PassiveList"].rootDataNode.ActionXmlDataNodesByPath("Passive", (DM.XmlDataNode passiveNode) =>
                    {
                        string PASSIVE_ID = passiveNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(PASSIVE_ID))
                            selectItems.Add(PASSIVE_ID);
                    });
                    break;
            }
            InitLbxSearchType(searchTypes);
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
                afterSelectItem(LbxItems.SelectedItem.ToString());
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
        EPISODE,
        BOOK_ICON,
        CHARACTER_SKIN,
        PASSIVE
    };
}
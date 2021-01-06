﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                        if (!string.IsNullOrEmpty(STAGE_ID) && Convert.ToInt32(STAGE_ID) < DS.FilterDatas.STAGEINFO_DIV_NOT_CREATURE && STAGE_ID != "1")
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(STAGE_ID));
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.BOOK_ICON:
                    DM.GameInfos.staticInfos["DropBook"].rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode bookUseNode) =>
                    {
                        string BOOK_ICON_NAME = bookUseNode.GetInnerTextByPath("BookIcon");
                        string BOOK_ICON_DES = DM.LocalizedGameDescriptions.GetDescriptionForETC(bookUseNode.GetInnerTextByPath("TextId"));
                        string CHPATER_NAME = DM.LocalizedGameDescriptions.GetDescriptionForChapter(bookUseNode.GetInnerTextByPath("Chapter"));
                        if (!string.IsNullOrEmpty(BOOK_ICON_NAME))
                            selectItems.Add($"{CHPATER_NAME} / {BOOK_ICON_DES}:{BOOK_ICON_NAME}");
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN:
                    DM.GameInfos.staticInfos["EquipPage"].rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode bookNode) =>
                    {
                        string CHPATER_NAME = DM.LocalizedGameDescriptions.GetDescriptionForChapter(bookNode.GetInnerTextByPath("Chapter"));
                        string BOOK_DES = DM.LocalizedGameDescriptions.GetDescriptionForBooks(bookNode.GetInnerTextByPath("TextId"));
                        string SKIN_NAME = bookNode.GetInnerTextByPath("CharacterSkin");
                        if (!string.IsNullOrEmpty(SKIN_NAME))
                            selectItems.Add($"{CHPATER_NAME} / {BOOK_DES}:{SKIN_NAME}");
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
                case InputInfoWithSearchWindow_PRESET.CRITICAL_BOOKS:
                    DM.GameInfos.staticInfos["EquipPage"].rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode eqNode) =>
                    {
                        string EQ_BOOK_ID = eqNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(EQ_BOOK_ID))
                            selectItems.Add(EQ_BOOK_ID);
                    });
                    break;


                case InputInfoWithSearchWindow_PRESET.CARD_ARTWORK:
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card/Artwork", (DM.XmlDataNode artworkNode) =>
                    {
                        string ARTWORK_NAME = artworkNode.innerText;
                        if (!string.IsNullOrEmpty(ARTWORK_NAME))
                            selectItems.Add(ARTWORK_NAME);
                    });
                    break;

                case InputInfoWithSearchWindow_PRESET.CARD_ABILITES:
                    DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode abilityNode) =>
                    {
                        string ABILITIY_ID = abilityNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                            selectItems.Add(ABILITIY_ID);
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.DICE_ABILITES:
                    DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode abilityNode) =>
                    {
                        string ABILITIY_ID = abilityNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                            selectItems.Add(ABILITIY_ID);
                    });
                    break;

                case InputInfoWithSearchWindow_PRESET.CARDS:
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        string CARD_ID = cardNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add(CARD_ID);
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
                InitSearchItems();
            }
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

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterSelectItem(LbxItems.SelectedItem.ToString().Split(':').Last());
                this.Close();
            }
        }
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
    }

    public enum InputInfoWithSearchWindow_PRESET
    {
        EPISODE,
        BOOK_ICON,
        CHARACTER_SKIN,
        PASSIVE,
        CRITICAL_BOOKS,

        CARD_ARTWORK,
        CARD_ABILITES,
        DICE_ABILITES,
        CARDS
    };
}
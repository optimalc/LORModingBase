using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditCriticalPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCriticalPage : UserControl
    {
        DM.XmlDataNode innerCriticalPageNode = null;
        Action initStack = null;

        #region Init controls
        public EditCriticalPage(DM.XmlDataNode innerCriticalPageNode, Action initStack)
        {
            try
            {
                this.innerCriticalPageNode = innerCriticalPageNode;
                this.initStack = initStack;
                InitializeComponent();

                #region 일반적인 핵심책장 정보 UI 반영시키기
                ChangeRarityUIInit(innerCriticalPageNode.GetInnerTextByPath("Rarity"));

                BtnEpisode.Content = innerCriticalPageNode.GetInnerTextByPath("Episode");
                BtnEpisode.ToolTip = innerCriticalPageNode.GetInnerTextByPath("Episode");

                TbxPageName.Text = innerCriticalPageNode.GetInnerTextByPath("Name");
                TbxPageUniqueID.Text = innerCriticalPageNode.GetAttributesSafe("ID");

                TbxHP.Text = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HP");
                TbxBR.Text = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/Break");
                TbxSpeedDiceMin.Text = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SpeedMin");
                TbxSpeedDiceMax.Text = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/Speed");

                BtnBookIcon.Content = innerCriticalPageNode.GetInnerTextByPath("BookIcon");
                BtnBookIcon.ToolTip = innerCriticalPageNode.GetInnerTextByPath("BookIcon");

                BtnSkin.Content = innerCriticalPageNode.GetInnerTextByPath("CharacterSkin");
                BtnSkin.ToolTip = innerCriticalPageNode.GetInnerTextByPath("CharacterSkin");

                BtnSResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SResist");
                BtnPResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist");
                BtnHResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist");

                BtnBSResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist");
                BtnBPResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist");
                BtnBHResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist");

                InitLbxPassives();
                #endregion

                #region 핵심책장 설명부분 UI 반영시키기
                string DESCRIPTION = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetInnerTextByAttributeWithPath("bookDescList/BookDesc", "BookID",
                    innerCriticalPageNode.GetAttributesSafe("ID"));
                if (!string.IsNullOrEmpty(DESCRIPTION))
                {
                    BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                    BtnCiricalBookInfo.ToolTip = "핵심 책장에 대한 설명을 입력합니다 (입력됨)";
                }
                #endregion
                #region 유니크 전용 책장 설정 부분 UI 반영시키기
                if (innerCriticalPageNode.GetXmlDataNodesByName("EquipEffect/OnlyCard").Count > 0)
                {
                    string extraInfo = "";
                    innerCriticalPageNode.ActionXmlDataNodesByName("EquipEffect/OnlyCard", (DM.XmlDataNode xmlDataNode) =>
                    {
                        extraInfo += $"{xmlDataNode.GetInnerTextSafe()}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesUniqueCard.png");
                    BookUniqueCards.ToolTip = $"이 핵심책장이 사용할 수 있는 고유 책장을 입력합니다 (입력됨)\n{extraInfo}";
                }
                #endregion

                List<string> DROP_BOOKS = new List<string>();
                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.ActionXmlDataNodesByName("BookUse/DropItem", (DM.XmlDataNode dropItemNode) => {
                    if (dropItemNode.GetInnerTextSafe() == innerCriticalPageNode.GetAttributesSafe("ID"))
                        DROP_BOOKS.Add(dropItemNode.GetInnerTextSafe());
                });
                #region 핵심책장 드랍 책 부분 UI 반영시키기
                if (DROP_BOOKS.Count > 0)
                {
                    string extraInfo = "";
                    DROP_BOOKS.ForEach((string DROP_DOOK) =>
                    {
                        extraInfo += $"{DROP_DOOK}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                    BtnDropBooks.ToolTip = $"이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
                }
                #endregion
                #region 적 전용책장 입력 부분 UI 반영시키기
                if (!string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")))
                {
                    string extraInfo = "";
                    extraInfo += $"시작시 빛의 수 : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint")}\n";
                    extraInfo += $"최대 빛의 수 : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint")}\n";
                    extraInfo += $"최대 감정 레벨 : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel")}\n";
                    extraInfo += $"추가로 드로우하는 책장의 수: {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")}";

                    BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesEnemy.png");
                    BtnEnemySetting.ToolTip = $"적 전용 책장에서 추가로 입력할 수 있는 값을 입력합니다 (입력됨)\n{extraInfo}";
                }
                #endregion

                #region 핵심책장 원거리 속성 UI 반영시키기
                if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Range")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeRange.png");
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 원거리 전용 책장)";
                }
                else if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Hybrid")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeHybrid.png");
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 하이브리드 책장)";
                }
                else
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNomal.png");
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 일반 책장)";
                }
                #endregion

                CriticalPageTypeUIUpdating();
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "핵심 책장 초기화에서 오류 발생");
            }
        }

        private void ChangeRarityUIInit(string rarity)
        {
            BtnRarityCommon.Background = null;
            BtnRarityUncommon.Background = null;
            BtnRarityRare.Background = null;
            BtnRarityUnique.Background = null;

            switch (rarity)
            {
                case "Common":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#5430BF4B");
                    BtnRarityCommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageNode.SetXmlInfoByPath("Rarity", "Common");
                    break;
                case "Uncommon":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54306ABF");
                    BtnRarityUncommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageNode.SetXmlInfoByPath("Rarity", "Uncommon");
                    break;
                case "Rare":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#548030BF");
                    BtnRarityRare.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageNode.SetXmlInfoByPath("Rarity", "Rare");
                    break;
                case "Unique":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54F3B530");
                    BtnRarityUnique.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageNode.SetXmlInfoByPath("Rarity", "Unique");
                    break;
            }
        }
        
        private void InitResistFromButton(Button targetButton, bool isLeft)
        {
            // Down index
            if(isLeft)
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.IndexOf(targetButton.Content.ToString()) - 1;
                if (RESISTS_INDEX < 0) RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.Count - 1;
                targetButton.Content = DS.GameInfo.resistInfo_Doc[RESISTS_INDEX];
            }
            // Up index
            else
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.IndexOf(targetButton.Content.ToString()) + 1;
                if (RESISTS_INDEX >= DS.GameInfo.resistInfo_Doc.Count) RESISTS_INDEX = 0;
                targetButton.Content = DS.GameInfo.resistInfo_Doc[RESISTS_INDEX];
            }

            switch (targetButton.Name)
            {
                case "BtnSResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/SResist", targetButton.Content.ToString());
                    break;
                case "BtnPResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/PResist", targetButton.Content.ToString());
                    break;
                case "BtnHResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/HResist", targetButton.Content.ToString());
                    break;

                case "BtnBSResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/SBResist", targetButton.Content.ToString());
                    break;
                case "BtnBPResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/PBResist", targetButton.Content.ToString());
                    break;
                case "BtnBHResist":
                    innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/HBResist", targetButton.Content.ToString());
                    break;
            }
        }
        #endregion
        #region Button events
        #region Rarity buttons
        private void BtnRarityCommon_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Common");
        }

        private void BtnRarityUncommon_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Uncommon");
        }

        private void BtnRarityRare_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Rare");
        }

        private void BtnRarityUnique_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Unique");
        }
        #endregion

        private void BtnEpisode_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputEpisodeWindow((string chapter, string episodeID, string episodeDoc) =>
            {
                string CONTENT_TO_SHOW = $"{DS.GameInfo.chapter_Dic[chapter]} / {episodeDoc}:{episodeID}";
                BtnEpisode.Content = CONTENT_TO_SHOW;
                BtnEpisode.ToolTip = CONTENT_TO_SHOW;

                innerCriticalPageNode.SetXmlInfoByPath("Chapter", chapter);
                innerCriticalPageNode.SetXmlInfoByPath("Episode", episodeID);
            }).ShowDialog();
        }
        private void BtnBookIcon_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookIconWindow((string chpater, string bookIconName, string bookIconDesc) =>
            {
                string ICON_DESC = $"{bookIconDesc}:{bookIconName}";
                BtnBookIcon.Content = ICON_DESC;
                BtnBookIcon.ToolTip = ICON_DESC;

                innerCriticalPageNode.SetXmlInfoByPath("BookIcon", bookIconName);
            }).ShowDialog();
        }
        private void BtnSkin_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookSkinWindow((string chpater, string bookSkinName, string bookSkinDesc) =>
            {
                string SKIN_DESC = $"{bookSkinDesc}:{bookSkinName}";
                BtnSkin.Content = SKIN_DESC;
                BtnSkin.ToolTip = SKIN_DESC;

                innerCriticalPageNode.SetXmlInfoByPath("CharacterSkin", bookSkinName);
            }).ShowDialog();
        }

        #region HP resist buttons
        private void BtnSResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnSResist, true);
        }

        private void BtnSResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnSResist, false);
        }

        private void BtnPResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnPResist, true);
        }

        private void BtnPResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnPResist, false);
        }

        private void BtnHResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnHResist, true);
        }

        private void BtnHResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnHResist, false);
        }
        #endregion
        #region Break resist buttons
        private void BtnBSResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBSResist, true);
        }

        private void BtnBSResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBSResist, false);
        }


        private void BtnBPResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBPResist, true);
        }

        private void BtnBPResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBPResist, false);
        }

        private void BtnBHResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBHResist, true);
        }

        private void BtnBHResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBHResist, false);
        }
        #endregion

        #endregion

        #region Passive events
        private void InitLbxPassives()
        {
            //LbxPassives.Items.Clear();
            //innerCriticalPageInfo.passiveIDs.ForEach((string passiveName) =>
            //{
            //    LbxPassives.Items.Add(passiveName);
            //});
        }

        private void BtnAddPassive_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookPassiveWindow((string passiveDec) =>
            {
                innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/Passive", passiveDec
                    , new Dictionary<string, string>() { { "Level" , "10" } });
                InitLbxPassives();
            }).ShowDialog();
        }

        private void BtnDeletePassive_Click(object sender, RoutedEventArgs e)
        {
            if(LbxPassives.SelectedItem != null)
            {
                innerCriticalPageNode.RemoveXmlInfosByPath("EquipEffect/Passive", LbxPassives.SelectedItem.ToString());
                InitLbxPassives();
            }
        }
        #endregion
        #region Text change events
        private void TbxPageName_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.SetXmlInfoByPath("Name", TbxPageName.Text);
        }

        private void TbxPageUniqueID_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.attribute["ID"] = TbxPageUniqueID.Text;
            CriticalPageTypeUIUpdating();
        }

        private void TbxHP_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/HP", TbxHP.Text);
        }

        private void TbxBR_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/Break", TbxBR.Text);
        }

        private void TbxSpeedDiceMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/SpeedMin", TbxSpeedDiceMin.Text);
        }

        private void TbxSpeedDiceMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageNode.SetXmlInfoByPath("EquipEffect/Speed", TbxSpeedDiceMax.Text);
        }
        #endregion

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.subNodes.Remove(innerCriticalPageNode);
            initStack();
        }

        #region Right button events (Upside)
        private void BtnCiricalBookInfo_Click(object sender, RoutedEventArgs e)
        {
            //new SubWindows.InputCriticalBookDescription((string inputedDes) =>
            //{
            //    innerCriticalPageInfo.description = inputedDes;
            //    BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
            //    BtnCiricalBookInfo.ToolTip = "핵심 책장에 대한 설명을 입력합니다 (입력됨)";
            //}, innerCriticalPageInfo.description).ShowDialog();
        }

        private void BtnDropBooks_Click(object sender, RoutedEventArgs e)
        {
            //new SubWindows.InputDropBookInfosWindow(innerCriticalPageInfo.dropBooks).ShowDialog();

            //if(innerCriticalPageInfo.dropBooks.Count > 0)
            //{
            //    string extraInfo = "";
            //    innerCriticalPageInfo.dropBooks.ForEach((string dropBookInfo) =>
            //    {
            //        extraInfo += $"{dropBookInfo}\n";
            //    });
            //    extraInfo = extraInfo.TrimEnd('\n');

            //    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
            //    BtnDropBooks.ToolTip = $"이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
            //}
            //else
            //{
            //    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
            //    BtnDropBooks.ToolTip = "이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (미입력)";
            //}
        }

        private void BtnEnemySetting_Click(object sender, RoutedEventArgs e)
        {
            //new SubWindows.InputEnemyInfoWindow(innerCriticalPageInfo).ShowDialog();

            //string extraInfo = "";
            //extraInfo += $"시작시 빛의 수 : {innerCriticalPageInfo.ENEMY_StartPlayPoint}\n";
            //extraInfo += $"최대 빛의 수 : {innerCriticalPageInfo.ENEMY_MaxPlayPoint}\n";
            //extraInfo += $"최대 감정 레벨 : {innerCriticalPageInfo.ENEMY_EmotionLevel}\n";
            //extraInfo += $"추가로 드로우하는 책장의 수: {innerCriticalPageInfo.ENEMY_AddedStartDraw}";

            //if (!string.IsNullOrEmpty(innerCriticalPageInfo.ENEMY_StartPlayPoint) ||
            //    !string.IsNullOrEmpty(innerCriticalPageInfo.ENEMY_MaxPlayPoint) ||
            //    !string.IsNullOrEmpty(innerCriticalPageInfo.ENEMY_AddedStartDraw) ||
            //    !string.IsNullOrEmpty(innerCriticalPageInfo.ENEMY_EmotionLevel))
            //{
            //    BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesEnemy.png");
            //    BtnEnemySetting.ToolTip = $"적 전용 책장에서 추가로 입력할 수 있는 값을 입력합니다 (입력됨)\n{extraInfo}";
            //}
            //else
            //{
            //    BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoEnemy.png");
            //    BtnEnemySetting.ToolTip = "적 전용 책장에서 추가로 입력할 수 있는 값을 입력합니다 (미입력))";
            //}
            CriticalPageTypeUIUpdating();
        }

        private void BookUniqueCards_Click(object sender, RoutedEventArgs e)
        {
            //new SubWindows.InputUniqueCardsWindow(innerCriticalPageInfo.onlyCards).ShowDialog();

            //if (innerCriticalPageInfo.onlyCards.Count > 0)
            //{
            //    string extraInfo = "";
            //    innerCriticalPageInfo.onlyCards.ForEach((string onlyCardInfo) =>
            //    {
            //        extraInfo += $"{onlyCardInfo}\n";
            //    });
            //    extraInfo = extraInfo.TrimEnd('\n');

            //    BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesUniqueCard.png");
            //    BookUniqueCards.ToolTip = $"이 핵심책장이 사용할 수 있는 고유 책장을 입력합니다 (입력됨)\n{extraInfo}";
            //}
            //else
            //{
            //    BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoUniqueCard.png");
            //    BookUniqueCards.ToolTip = "이 핵심책장이 사용할 수 있는 고유 책장을 입력합니다 (미입력)";
            //}
        }
        #endregion

        private void BtnRangeType_Click(object sender, RoutedEventArgs e)
        {
            //switch(innerCriticalPageInfo.rangeType)
            //{
            //    case "Range":
            //        innerCriticalPageInfo.rangeType = "Hybrid";
            //        break;
            //    case "Hybrid":
            //        innerCriticalPageInfo.rangeType = "Nomal";
            //        break;
            //    default:
            //        innerCriticalPageInfo.rangeType = "Range";
            //        break;
            //}

            //#region 핵심책장 원거리 속성 UI 반영시키기
            //if (innerCriticalPageInfo.rangeType == "Range")
            //{
            //    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeRange.png");
            //    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 원거리 전용 책장)";
            //}
            //else if (innerCriticalPageInfo.rangeType == "Hybrid")
            //{
            //    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeHybrid.png");
            //    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 하이브리드 책장)";
            //}
            //else
            //{
            //    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNomal.png");
            //    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 일반 책장)";
            //}
            //#endregion
        }
    
        private void CriticalPageTypeUIUpdating()
        {
            //try
            //{
            //    bool BOOK_ID_CHECK = false;
            //    if(!string.IsNullOrEmpty(innerCriticalPageInfo.bookID))
            //    {
            //        int BOOK_ID = Convert.ToInt32(innerCriticalPageInfo.bookID);
            //        BOOK_ID_CHECK = (BOOK_ID < DS.FilterDatas.CRITICAL_PAGE_DIV_ENEMY) || (BOOK_ID > DS.FilterDatas.CRITICAL_PAGE_DIV_CUSTOM && BOOK_ID < (DS.FilterDatas.CRITICAL_PAGE_DIV_CUSTOM + 1000000));
            //    }

            //    bool FORCE_ENEMY = innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE;
            //    bool FORCE_USER = innerCriticalPageInfo.USER_TYPE_CH_FORCE;
            //    bool FORECLY_INPUTED = FORCE_ENEMY || FORCE_USER;

            //    if ((BOOK_ID_CHECK || FORCE_ENEMY) && !FORCE_USER)
            //    {
            //        innerCriticalPageInfo.ENEMY_IS_ENEMY_TYPE = true;
            //        BtnCriticalPageType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeEnemy.png");
            //        BtnCriticalPageType.ToolTip = $"클릭시 책장을 속성을 수동으로 수정합니다. (현재 : 적 전용 책장[{(FORECLY_INPUTED ? "수동" : "자동")}]) {DS.LongDescription.EditCriticalPage_TypeChange}";
            //    }
            //    else
            //    {
            //        innerCriticalPageInfo.ENEMY_IS_ENEMY_TYPE = false;
            //        BtnCriticalPageType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeUser.png");
            //        BtnCriticalPageType.ToolTip = $"클릭시 책장을 속성을 수동으로 수정합니다. (현재 : 유저 전용 책장[{(FORECLY_INPUTED ? "수동" : "자동")}]) {DS.LongDescription.EditCriticalPage_TypeChange}";
            //    }
            //}
            //catch
            //{

            //}
        }

        private void BtnCriticalPageType_Click(object sender, RoutedEventArgs e)
        {
            //if(!innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE && !innerCriticalPageInfo.USER_TYPE_CH_FORCE)
            //{
            //    innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE = true;
            //    innerCriticalPageInfo.USER_TYPE_CH_FORCE = false;
            //}
            //else if(innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE)
            //{
            //    innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE = false;
            //    innerCriticalPageInfo.USER_TYPE_CH_FORCE = true;
            //}
            //else if (innerCriticalPageInfo.USER_TYPE_CH_FORCE)
            //{
            //    innerCriticalPageInfo.ENEMY_TYPE_CH_FORCE = false;
            //    innerCriticalPageInfo.USER_TYPE_CH_FORCE = false;
            //}

            CriticalPageTypeUIUpdating();
        }

        private void BtnCopyPage_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.criticalPageInfos.Add(Tools.DeepCopy.DeepClone(innerCriticalPageInfo));
            //initStack();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LORModingBase.CustomExtensions;

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
                Tools.WindowControls.InitTextBoxControlsByUsingName(this, innerCriticalPageNode);

                switch(innerCriticalPageNode.GetInnerTextByPath("Rarity"))
                {
                    case "Common":
                        ChangeRarityButtonEvents(BtnRarity_Common, null);
                        break;
                    case "Uncommon":
                        ChangeRarityButtonEvents(BtnRarity_Uncommon, null);
                        break;
                    case "Rare":
                        ChangeRarityButtonEvents(BtnRarity_Rare, null);
                        break;
                    case "Unique":
                        ChangeRarityButtonEvents(BtnRarity_Unique, null);
                        break;
                }
                TbxPageUniqueID.Text = innerCriticalPageNode.GetAttributesSafe("ID");

                innerCriticalPageNode.ActionIfInnertTextIsNotNullOrEmpty("Episode", (string innerText) =>
                {
                    BtnEpisode.Content = innerText;
                    BtnEpisode.ToolTip = innerText;
                });
                innerCriticalPageNode.ActionIfInnertTextIsNotNullOrEmpty("BookIcon", (string innerText) =>
                {
                    BtnBookIcon.Content = innerText;
                    BtnBookIcon.ToolTip = innerText;
                });
                innerCriticalPageNode.ActionIfInnertTextIsNotNullOrEmpty("CharacterSkin", (string innerText) =>
                {
                    BtnSkin.Content = innerText;
                    BtnSkin.ToolTip = innerText;
                });

                Btn_SResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SResist");
                Btn_PResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist");
                Btn_HResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist");

                Btn_SBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist");
                Btn_PBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist");
                Btn_HBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist");

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
                if (innerCriticalPageNode.GetXmlDataNodesByPath("EquipEffect/OnlyCard").Count > 0)
                {
                    string extraInfo = "";
                    innerCriticalPageNode.ActionXmlDataNodesByPath("EquipEffect/OnlyCard", (DM.XmlDataNode xmlDataNode) =>
                    {
                        extraInfo += $"{xmlDataNode.GetInnerTextSafe()}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesUniqueCard.png");
                    BookUniqueCards.ToolTip = $"이 핵심책장이 사용할 수 있는 고유 책장을 입력합니다 (입력됨)\n{extraInfo}";
                }
                #endregion

                List<string> DROP_BOOKS = new List<string>();
                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.ActionXmlDataNodesByPath("BookUse/DropItem", (DM.XmlDataNode dropItemNode) =>
                {
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
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "핵심 책장 초기화에서 오류 발생");
            }
        }

        /// <summary>
        /// Change rarity button events
        /// </summary>
        private void ChangeRarityButtonEvents(object sender, RoutedEventArgs e)
        {
            Button rarityButton = sender as Button;

            BtnRarity_Common.Background = null;
            BtnRarity_Uncommon.Background = null;
            BtnRarity_Rare.Background = null;
            BtnRarity_Unique.Background = null;

            rarityButton.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
            WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr(rarityButton.Tag.ToString());
            innerCriticalPageNode.SetXmlInfoByPath("Rarity", rarityButton.Name.Split('_').Last());
        }

        /// <summary>
        /// Resist button events
        /// </summary>
        private void InitResistFromButtonEvents(object sender, MouseButtonEventArgs e)
        {
            Button resistButton = sender as Button;

            // Down index
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Code.IndexOf(resistButton.Content.ToString()) - 1;
                if (RESISTS_INDEX < 0) RESISTS_INDEX = DS.GameInfo.resistInfo_Code.Count - 1;
                resistButton.Content = DS.GameInfo.resistInfo_Code[RESISTS_INDEX];
                resistButton.Tag = DS.GameInfo.resistInfo_Code[RESISTS_INDEX];
            }
            // Up index
            else
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Code.IndexOf(resistButton.Content.ToString()) + 1;
                if (RESISTS_INDEX >= DS.GameInfo.resistInfo_Code.Count) RESISTS_INDEX = 0;
                resistButton.Content = DS.GameInfo.resistInfo_Code[RESISTS_INDEX];
                resistButton.Tag = DS.GameInfo.resistInfo_Code[RESISTS_INDEX];
            }
            innerCriticalPageNode.SetXmlInfoByPath($"EquipEffect/{resistButton.Name.Split('_').Last()}", resistButton.Tag.ToString());
        }
        #endregion

        /// <summary>
        /// Button events that need search window
        /// </summary>
        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnEpisode":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.SetXmlInfoByPath("Episode", selectedItem);
                        MainWindow.mainWindow.UpdateDebugInfo();
                        BtnEpisode.Content = selectedItem;
                        BtnEpisode.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnBookIcon":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.SetXmlInfoByPath("BookIcon", selectedItem);
                        MainWindow.mainWindow.UpdateDebugInfo();
                        BtnBookIcon.Content = selectedItem;
                        BtnBookIcon.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.BOOK_ICON).ShowDialog();
                    break;
                case "BtnSkin":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.SetXmlInfoByPath("CharacterSkin", selectedItem);
                        MainWindow.mainWindow.UpdateDebugInfo();
                        BtnSkin.Content = selectedItem;
                        BtnSkin.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN).ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxPageUniqueID":
                    innerCriticalPageNode.attribute["ID"] = tbx.Text;
                    innerCriticalPageNode.SetXmlInfoByPath("TextId", tbx.Text);
                    break;
                default:
                    List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                    if (SPLIT_NAME.Count == 2)
                        innerCriticalPageNode.SetXmlInfoByPath(SPLIT_NAME.Last(), tbx.Text);
                    else if (SPLIT_NAME.Count > 2)
                        innerCriticalPageNode.SetXmlInfoByPath(String.Join("/", SPLIT_NAME.Skip(1)), tbx.Text);
                    break;
            }
        }

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
                    , new Dictionary<string, string>() { { "Level", "10" } });
                InitLbxPassives();
            }).ShowDialog();
        }

        private void BtnDeletePassive_Click(object sender, RoutedEventArgs e)
        {
            if (LbxPassives.SelectedItem != null)
            {
                innerCriticalPageNode.RemoveXmlInfosByPath("EquipEffect/Passive", LbxPassives.SelectedItem.ToString());
                InitLbxPassives();
            }
        }
        #endregion

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Remove(innerCriticalPageNode);
            initStack();
            MainWindow.mainWindow.UpdateDebugInfo();
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
        }

        private void BtnCopyPage_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.criticalPageInfos.Add(Tools.DeepCopy.DeepClone(innerCriticalPageInfo));
            //initStack();
        }
    }
}

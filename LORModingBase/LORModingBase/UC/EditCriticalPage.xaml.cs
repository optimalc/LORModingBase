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
                Btn_SResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SResist");
                Btn_PResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist");
                Btn_PResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist");
                Btn_HResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist");
                Btn_HResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist");

                Btn_SBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist");
                Btn_SBResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist");
                Btn_PBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist");
                Btn_PBResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist");
                Btn_HBResist.Content = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist");
                Btn_HBResist.Tag = innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist");

                InitLbxPassives();
                #endregion

                #region 핵심책장 설명부분 UI 반영시키기
                List<DM.XmlDataNode> foundXmlDataNodes = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                if (foundXmlDataNodes.Count > 0)
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

                #region 드랍 목록에 반영시키기
                List<string> selectedDropBooks = new List<string>();
                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseID) =>
                {
                    if (bookUseID.CheckIfGivenPathWithXmlInfoExists("DropItem", innerCriticalPageNode.attribute["ID"]))
                        selectedDropBooks.Add(bookUseID.attribute["ID"]);
                });

                if (selectedDropBooks.Count > 0)
                {
                    string extraInfo = "";
                    selectedDropBooks.ForEach((string dropBookInfo) =>
                    {
                        extraInfo += $"{dropBookInfo}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                    BtnDropBooks.ToolTip = $"이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
                }
                else
                {
                    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                    BtnDropBooks.ToolTip = "이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (미입력)";
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
                BtnRangeType.Tag = innerCriticalPageNode.GetInnerTextByPath("RangeType");
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
        /// Initialize passive list
        /// </summary>
        private void InitLbxPassives()
        {
            LbxPassives.Items.Clear();
            innerCriticalPageNode.ActionXmlDataNodesByPath("EquipEffect/Passive", (DM.XmlDataNode passiveNode) =>
            {
                LbxPassives.Items.Add(passiveNode.innerText);
            });
        }
        #endregion


        #region Button events
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
                        BtnEpisode.Content = selectedItem;
                        BtnEpisode.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnBookIcon":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.SetXmlInfoByPath("BookIcon", selectedItem);
                        BtnBookIcon.Content = selectedItem;
                        BtnBookIcon.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.BOOK_ICON).ShowDialog();
                    break;
                case "BtnSkin":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.SetXmlInfoByPath("CharacterSkin", selectedItem);
                        BtnSkin.Content = selectedItem;
                        BtnSkin.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN).ShowDialog();
                    break;

                case "BtnAddPassive":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.AddXmlInfoByPath("EquipEffect/Passive", selectedItem
                , new Dictionary<string, string>() { { "Level", "10" } });
                        InitLbxPassives();
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.PASSIVE).ShowDialog();
                    break;
                case "BtnDeletePassive":
                    if (LbxPassives.SelectedItem != null)
                    {
                        innerCriticalPageNode.RemoveXmlInfosByPath("EquipEffect/Passive", LbxPassives.SelectedItem.ToString(), deleteOnce: true);
                        InitLbxPassives();
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }
                    break;
            }
        }

        /// <summary>
        /// Button events that need to multiple items to be selected
        /// </summary>
        private void SelectItemListButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnDropBooks":
                    List<string> selectedDropBooks = new List<string>();
                    DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseID) =>
                    {
                        if (bookUseID.CheckIfGivenPathWithXmlInfoExists("DropItem", innerCriticalPageNode.attribute["ID"]))
                            selectedDropBooks.Add(bookUseID.attribute["ID"]);
                    });

                    new SubWindows.Global_AddItemToListWindow((string addedDropBookItemID) =>
                    {
                        if (DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.CheckIfGivenPathWithXmlInfoExists("BookUse",
                            attributeToCheck: new Dictionary<string, string> { { "ID", addedDropBookItemID } }))
                        {
                            List<DM.XmlDataNode> foundDropBookNode = DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                                attributeToCheck: new Dictionary<string, string> { { "ID", addedDropBookItemID } });
                            if(foundDropBookNode.Count > 0
                                && !foundDropBookNode[0].CheckIfGivenPathWithXmlInfoExists("DropItem", innerCriticalPageNode.attribute["ID"]))
                                foundDropBookNode[0].AddXmlInfoByPath("DropItem", innerCriticalPageNode.attribute["ID"],
                                                            new Dictionary<string, string>() { { "Type", "Equip" } });
                        }
                        else
                        {
                            DM.XmlDataNode madeDropBookNode = DM.EditGameData_BookInfos.MakeNewLocalizeDropBook(addedDropBookItemID);
                            if(!madeDropBookNode.CheckIfGivenPathWithXmlInfoExists("DropItem", innerCriticalPageNode.attribute["ID"]))
                            {
                                madeDropBookNode.AddXmlInfoByPath("DropItem", innerCriticalPageNode.attribute["ID"],
                                    new Dictionary<string, string>() { { "Type", "Equip" } });
                                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.subNodes.Add(madeDropBookNode);
                            }
                        }
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, (string deletedDropBookItemID) => {
                        if (DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.CheckIfGivenPathWithXmlInfoExists("BookUse",
                            attributeToCheck: new Dictionary<string, string> { { "ID", deletedDropBookItemID } }))
                        {
                            List<DM.XmlDataNode> foundDropBookNode = DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                              attributeToCheck: new Dictionary<string, string> { { "ID", deletedDropBookItemID } });
                            if (foundDropBookNode.Count > 0)
                            {
                                DM.XmlDataNode FOUND_DROP_BOOK_NODE = foundDropBookNode[0];
                                FOUND_DROP_BOOK_NODE.RemoveXmlInfosByPath("DropItem", innerCriticalPageNode.attribute["ID"], deleteOnce:true);

                                List<DM.XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropBookItemID } });
                                if(baseBookUseNode.Count > 0)
                                {
                                    DM.XmlDataNode FOUND_DROP_BOOK_IN_GAME = baseBookUseNode[0];

                                    List<string> foundDropBookDropItems = new List<string>();
                                    FOUND_DROP_BOOK_NODE.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                                    {
                                        foundDropBookDropItems.Add(DropItem.innerText);
                                    });

                                    List<string> foundDropBookInGameItems = new List<string>();
                                    FOUND_DROP_BOOK_IN_GAME.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                                    {
                                        foundDropBookInGameItems.Add(DropItem.innerText);
                                    });

                                    if (foundDropBookDropItems.Count == foundDropBookInGameItems.Count
                                        && foundDropBookDropItems.Except(foundDropBookInGameItems).Count() == 0)
                                    {
                                        DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.RemoveXmlInfosByPath("BookUse",
                                            attributeToCheck: new Dictionary<string, string> { { "ID", deletedDropBookItemID } }, deleteOnce:true);
                                    }
                                }
                            }
                            MainWindow.mainWindow.UpdateDebugInfo();
                        }
                    }, selectedDropBooks, SubWindows.AddItemToListWindow_PRESET.DROP_BOOK).ShowDialog();

                    #region Update dropbook Input
                    List<string> selectedDropBooks_toUpdate = new List<string>();
                    DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseID) =>
                    {
                        if (bookUseID.CheckIfGivenPathWithXmlInfoExists("DropItem", innerCriticalPageNode.attribute["ID"]))
                            selectedDropBooks_toUpdate.Add(bookUseID.attribute["ID"]);
                    });

                    if (selectedDropBooks_toUpdate.Count > 0)
                    {
                        string extraInfo = "";
                        selectedDropBooks_toUpdate.ForEach((string dropBookInfo) =>
                        {
                            extraInfo += $"{dropBookInfo}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                        BtnDropBooks.ToolTip = $"이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
                    }
                    else
                    {
                        BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                        BtnDropBooks.ToolTip = "이 핵심책장이 어느 책에서 드랍되는지 입력합니다 (미입력)";
                    } 
                    #endregion
                    break;
                case "BookUniqueCards":
                    List<string> selectedUniqCards = new List<string>();
                    innerCriticalPageNode.GetXmlDataNodesByPath("EquipEffect/OnlyCard").ForEachSafe((DM.XmlDataNode onlyCardNode) =>
                    {
                        selectedUniqCards.Add(onlyCardNode.GetInnerTextSafe());
                    });

                    new SubWindows.Global_AddItemToListWindow((string addedItem) =>
                    {
                        innerCriticalPageNode.AddXmlInfoByPath("EquipEffect/OnlyCard", addedItem);
                    }, (string deletedItem) => {

                        innerCriticalPageNode.RemoveXmlInfosByPath("EquipEffect/OnlyCard", deletedItem, deleteOnce:true);
                    }, selectedUniqCards, SubWindows.AddItemToListWindow_PRESET.ONLY_CARD).ShowDialog();


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
                    else
                    {
                        BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoUniqueCard.png");
                        BookUniqueCards.ToolTip = "이 핵심책장이 사용할 수 있는 고유 책장을 입력합니다 (미입력)";
                    }
                    break;
            }
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnCiricalBookInfo":
                    string prevStory = "";
                    List<DM.XmlDataNode> foundXmlDataNodesToPrevInput = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                    if(foundXmlDataNodesToPrevInput.Count > 0)
                    {
                        List<string> descInnerTexts = new List<string>();
                        foundXmlDataNodesToPrevInput[0].GetXmlDataNodesByPath("TextList/Desc").ForEachSafe((DM.XmlDataNode descNode) =>
                        {
                            if (!string.IsNullOrEmpty(descNode.innerText))
                                descInnerTexts.Add(descNode.innerText);
                        });
                        prevStory = String.Join("\r\n\r\n", descInnerTexts).Replace(".", ".\r\n");
                    }

                    new SubWindows.Global_InputOneColumnData((string description) =>
                    {
                        DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.RemoveXmlInfosByPath("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });

                        if (!string.IsNullOrEmpty(description))
                        {
                            List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPath("bookDescList");

                            if(foundXmlDataNode.Count > 0)
                            {
                                foundXmlDataNode[0].subNodes.Add(DM.EditGameData_BookInfos.MakeNewLocalizeBooksBase(
                                    innerCriticalPageNode.GetAttributesSafe("ID"),
                                    innerCriticalPageNode.GetInnerTextByPath("Name"),
                                    description));
                            }
                            else
                            {
                                DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("bookDescList")
                                    .subNodes.Add(DM.EditGameData_BookInfos.MakeNewLocalizeBooksBase(
                                    innerCriticalPageNode.GetAttributesSafe("ID"),
                                    innerCriticalPageNode.GetInnerTextByPath("Name"),
                                    description));
                            }

                            MainWindow.mainWindow.UpdateDebugInfo();
                        }
                    }, prevStory).ShowDialog();

                    List<DM.XmlDataNode> foundXmlDataNodes = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                    if (foundXmlDataNodes.Count > 0)
                    {
                        BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                        BtnCiricalBookInfo.ToolTip = "핵심 책장에 대한 설명을 입력합니다 (입력됨)";
                    }
                    else
                    {
                        BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNobookInfo.png");
                        BtnCiricalBookInfo.ToolTip = "핵심 책장에 대한 설명을 입력합니다 (미입력)";
                    }
                    break;
                case "BtnEnemySetting":
                    new SubWindows.Global_MultipleValueInputed(new Dictionary<string, string>() {
                        { "시작시 빛의 수", "시작할때 적이 가지게 되는 빛의 개수를 입력합니다"},
                        { "최대 빛의 수", "적이 가지게 되는 빛의 최대 개수를 입력합니다" },
                        { "최대 감정 레벨", "적이 가지게 되는 최대 감정레벨을 입력합니다" },
                        { "추가로 드로우하는 책장의 수", "적이 책장을 추가로 드러우 할 때의 수치입니다" }
                    }, new List<string>()
                    {
                        innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint"),
                        innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint"),
                        innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel"),
                        innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")
                    }, new List<Action<string>>()
                    {
                        (string inputedVar) => {
                            innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("EquipEffect/StartPlayPoint", inputedVar);},
                        (string inputedVar) => {
                            innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("EquipEffect/MaxPlayPoint", inputedVar);},
                        (string inputedVar) => {
                            innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("EquipEffect/EmotionLevel", inputedVar);},
                        (string inputedVar) => {
                            innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("EquipEffect/AddedStartDraw", inputedVar);}
                    }).ShowDialog();

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
                    else
                    {
                        BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoEnemy.png");
                        BtnEnemySetting.ToolTip = "적 전용 책장에서 추가로 입력할 수 있는 값을 입력합니다 (미입력))";
                    }
                    break;
                case "BtnCopyPage":
                    DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Add(innerCriticalPageNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    break;
                case "BtnDelete":
                    if( DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.CheckIfGivenPathWithXmlInfoExists("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } })
                        && !DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Book",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", innerCriticalPageNode.GetAttributesSafe("ID") } }))
                        DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.RemoveXmlInfosByPath("bookDescList/BookDesc",
                       attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });

                    DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseNode) =>
                    {
                        bookUseNode.RemoveXmlInfosByPath("DropItem", innerCriticalPageNode.attribute["ID"], deleteOnce: true);

                        List<DM.XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", bookUseNode.attribute["ID"] } });
                        if (baseBookUseNode.Count > 0)
                        {
                            DM.XmlDataNode FOUND_DROP_BOOK_IN_GAME = baseBookUseNode[0];

                            List<string> foundDropBookDropItems = new List<string>();
                            bookUseNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                            {
                                foundDropBookDropItems.Add(DropItem.innerText);
                            });

                            List<string> foundDropBookInGameItems = new List<string>();
                            FOUND_DROP_BOOK_IN_GAME.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                            {
                                foundDropBookInGameItems.Add(DropItem.innerText);
                            });

                            if (foundDropBookDropItems.Count == foundDropBookInGameItems.Count
                                && foundDropBookDropItems.Except(foundDropBookInGameItems).Count() == 0)
                            {
                                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.RemoveXmlInfosByPath("BookUse",
                                    attributeToCheck: new Dictionary<string, string> { { "ID", bookUseNode.attribute["ID"] } }, deleteOnce: true);
                            }
                        }});

                    DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Remove(innerCriticalPageNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    break;
            }
        }
        #endregion

        #region Type change button events
        /// <summary>
        /// Resist info code
        /// </summary>
        private List<string> RESIST_LOOP_LIST = new List<string>() { "Vulnerable", "Weak", "Normal", "Endure", "Resist", "Immune" };
        /// <summary>
        /// Resist info code
        /// </summary>
        private List<string> RANGE_LOOP_LIST = new List<string>() { "", "Range", "Hybrid" };

        /// <summary>
        /// Type loop button events
        /// </summary>
        private void TypeLoopButtonEvents(object sender, MouseButtonEventArgs e)
        {
            Button loopButton = sender as Button;

            List<string> LOOP_LIST = null;
            if (loopButton.Name.Contains("Resist"))
                LOOP_LIST = RESIST_LOOP_LIST;
            else if (loopButton.Name == "BtnRangeType")
                LOOP_LIST = RANGE_LOOP_LIST;
            if (LOOP_LIST == null)
                return;

            // Down index
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int LEFT_INDEX = LOOP_LIST.IndexOf(loopButton.Tag.ToString()) - 1;
                if (LEFT_INDEX < 0) LEFT_INDEX = LOOP_LIST.Count - 1;
                if (loopButton.Name.Contains("Resist"))
                    loopButton.Content = LOOP_LIST[LEFT_INDEX];
                loopButton.Tag = LOOP_LIST[LEFT_INDEX];
            }
            // Up index
            else
            {
                int RIGHT_INDEX = LOOP_LIST.IndexOf(loopButton.Tag.ToString()) + 1;
                if (RIGHT_INDEX >= LOOP_LIST.Count) RIGHT_INDEX = 0;
                if (loopButton.Name.Contains("Resist"))
                    loopButton.Content = LOOP_LIST[RIGHT_INDEX];
                loopButton.Tag = LOOP_LIST[RIGHT_INDEX];
            }

            if (loopButton.Name.Contains("Resist"))
                innerCriticalPageNode.SetXmlInfoByPath($"EquipEffect/{loopButton.Name.Split('_').Last()}", loopButton.Tag.ToString());
            else if (loopButton.Name == "BtnRangeType")
            {
                string RANGE_NAME = (string.IsNullOrEmpty(loopButton.Tag.ToString()) ? "Nomal" : loopButton.Tag.ToString());
                BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{RANGE_NAME}.png");
                innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("RangeType", loopButton.Tag.ToString());
            }
        }
        #endregion


        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxPageUniqueID":
                    string PREV_PAGE_ID = innerCriticalPageNode.attribute["ID"];
                    #region Books info localizing ID refrect
                    if (DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.CheckIfGivenPathWithXmlInfoExists("bookDescList/BookDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "BookID", PREV_PAGE_ID } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", PREV_PAGE_ID } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].attribute["BookID"] = tbx.Text;
                        }
                    }
                    #endregion
                    #region Drop table info ID refrect
                    DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseNode) =>
                    {
                        bookUseNode.GetXmlDataNodesByPathWithXmlInfo("DropItem").ForEachSafe((DM.XmlDataNode dropBookNode) =>
                        {
                            if (dropBookNode.innerText == PREV_PAGE_ID)
                                dropBookNode.innerText = tbx.Text;
                        });
                    });
                    #endregion
                    innerCriticalPageNode.attribute["ID"] = tbx.Text;
                    innerCriticalPageNode.SetXmlInfoByPath("TextId", tbx.Text);
                    break;
                case "TbxPageName_Name":
                    innerCriticalPageNode.SetXmlInfoByPath("Name", tbx.Text);
                    if (DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.CheckIfGivenPathWithXmlInfoExists("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].SetXmlInfoByPath("BookName", tbx.Text);
                        }
                    }
                    else
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPath("bookDescList");

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].subNodes.Add(DM.EditGameData_BookInfos.MakeNewLocalizeBooksBase(
                                innerCriticalPageNode.GetAttributesSafe("ID"),
                                innerCriticalPageNode.GetInnerTextByPath("Name"),
                                ""));
                        }
                        else
                        {
                            DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("bookDescList")
                                .subNodes.Add(DM.EditGameData_BookInfos.MakeNewLocalizeBooksBase(
                                innerCriticalPageNode.GetAttributesSafe("ID"),
                                innerCriticalPageNode.GetInnerTextByPath("Name"),
                                ""));
                        }

                        MainWindow.mainWindow.UpdateDebugInfo();
                    }
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
    }
}

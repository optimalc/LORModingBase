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
                Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.BOOK_INFO);

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
                    string STAGE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EPISODE");
                    string STAGE_DES = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(innerText);
                    BtnEpisode.ToolTip = $"{STAGE_WORD} : {STAGE_DES}";

                    LblEpisode.Content = $"{STAGE_WORD} : {STAGE_DES}";
                    BtnEpisode.Content = "          ";
                });
                innerCriticalPageNode.ActionIfInnertTextIsNotNullOrEmpty("BookIcon", (string innerText) =>
                {
                    string ICON_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"ICON");
                    BtnBookIcon.ToolTip = $"{ICON_WORD} : {innerText}";

                    LblBookIconViewLabel.Content = $"{ICON_WORD} : {innerText}";
                    BtnBookIcon.Content = "          ";
                });

                innerCriticalPageNode.ActionIfInnertTextIsNotNullOrEmpty("CharacterSkin", (string innerText) =>
                {
                    string SKIN_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"SKIN");

                    BtnSkin.ToolTip = $"{SKIN_WORD} : {innerText}";

                    LblSkin.Content = $"{SKIN_WORD} : {innerText}";
                    BtnSkin.Content = "          ";
                });

                Btn_SResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SResist"));
                Btn_SResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SResist"));
                Btn_PResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist"));
                Btn_PResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PResist"));
                Btn_HResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist"));
                Btn_HResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HResist"));

                Btn_SBResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist"));
                Btn_SBResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/SBResist"));
                Btn_PBResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist"));
                Btn_PBResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/PBResist"));
                Btn_HBResist.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist"));
                Btn_HBResist.Tag = DM.LocalizedGameDescriptions.GetDescriptionForResist(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/HBResist"));

                InitLbxPassives();
                #endregion

                #region 핵심책장 설명부분 UI 반영시키기
                List<DM.XmlDataNode> foundXmlDataNodes = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                if (foundXmlDataNodes.Count > 0)
                {
                    BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                    BtnCiricalBookInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnCiricalBookInfo_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})";
                }
                #endregion
                #region 유니크 전용 책장 설정 부분 UI 반영시키기
                if (innerCriticalPageNode.GetXmlDataNodesByPath("EquipEffect/OnlyCard").Count > 0)
                {
                    string extraInfo = "";
                    innerCriticalPageNode.ActionXmlDataNodesByPath("EquipEffect/OnlyCard", (DM.XmlDataNode xmlDataNode) =>
                    {
                        extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(xmlDataNode.GetInnerTextSafe())}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesUniqueCard.png");
                    BookUniqueCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BookUniqueCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
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
                        extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(dropBookInfo)}\n";
                    });
                    extraInfo = extraInfo.TrimEnd('\n');

                    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                    BtnDropBooks.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnDropBooks_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                }
                else
                {
                    BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                    BtnDropBooks.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnDropBooks_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                } 
                #endregion
                #region 적 전용책장 입력 부분 UI 반영시키기
                if (!string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel")) ||
                    !string.IsNullOrEmpty(innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")))
                {
                    string extraInfo = "";
                    extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"StartPlayPoint")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint")}\n";
                    extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"MaxPlayPoint")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint")}\n";
                    extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EmotionLevel")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel")}\n";
                    extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"AddedStartDraw")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")}";

                    BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesEnemy.png");
                    BtnEnemySetting.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnEnemySetting_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                }
                #endregion

                #region 핵심책장 원거리 속성 UI 반영시키기
                BtnRangeType.Tag = innerCriticalPageNode.GetInnerTextByPath("RangeType");
                if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Range")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeRange.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeRange")})";
                }
                else if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Hybrid")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeHybrid.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeHybrid")})";
                }
                else
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNear.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeNear")})";
                }
                #endregion
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EditCriticalPage_Error_1"));
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
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
        }

        /// <summary>
        /// Initialize passive list
        /// </summary>
        private void InitLbxPassives()
        {
            LbxPassives.Items.Clear();
            innerCriticalPageNode.ActionXmlDataNodesByPath("EquipEffect/Passive", (DM.XmlDataNode passiveNode) =>
            {
                LbxPassives.Items.Add($"{DM.LocalizedGameDescriptions.GetDescriptionForPassive(passiveNode.innerText)}:{passiveNode.innerText}");
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

                        string STAGE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EPISODE");
                        string STAGE_DES = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(selectedItem);
                        BtnEpisode.ToolTip = $"{STAGE_WORD} : {STAGE_DES}";

                        LblEpisode.Content = $"{STAGE_WORD} : {STAGE_DES}";
                        BtnEpisode.Content = "          ";

                        #region Input Chapter (Critical)
                        DM.GameInfos.staticInfos["StageInfo"].rootDataNode.ActionXmlDataNodesByAttributeWithPath("Stage", "id", selectedItem, (DM.XmlDataNode episodeNode) =>
                        {
                            string CHPATER_INFO = episodeNode.GetInnerTextByPath("Chapter");
                            if (string.IsNullOrEmpty(CHPATER_INFO))
                                CHPATER_INFO = "1";
                            innerCriticalPageNode.SetXmlInfoByPath("Chapter", CHPATER_INFO);
                        });
                        #endregion

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnBookIcon":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        string ICON_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"ICON");

                        innerCriticalPageNode.SetXmlInfoByPath("BookIcon", selectedItem);
                        BtnBookIcon.ToolTip = $"{ICON_WORD} : {selectedItem}";

                        LblBookIconViewLabel.Content = $"{ICON_WORD} : {selectedItem}";
                        BtnBookIcon.Content = "          ";

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.BOOK_ICON).ShowDialog();
                    break;
                case "BtnSkin":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        string SKIN_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"SKIN");

                        innerCriticalPageNode.SetXmlInfoByPath("CharacterSkin", selectedItem);

                        BtnSkin.ToolTip = $"{SKIN_WORD} : {selectedItem}";

                        LblSkin.Content = $"{SKIN_WORD} : {selectedItem}";
                        BtnSkin.Content = "          ";

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN).ShowDialog();
                    break;

                case "BtnAddPassive":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCriticalPageNode.AddXmlInfoByPath("EquipEffect/Passive", selectedItem
                            , new Dictionary<string, string>() { { "Level", "10" } });
                        InitLbxPassives();

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.PASSIVE).ShowDialog();
                    break;
                case "BtnDeletePassive":
                    if (LbxPassives.SelectedItem != null)
                    {
                        innerCriticalPageNode.RemoveXmlInfosByPath("EquipEffect/Passive", LbxPassives.SelectedItem.ToString().Split(':').Last(), deleteOnce: true);
                        InitLbxPassives();
                        MainWindow.mainWindow.UpdateDebugInfo();
                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
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
                            selectedDropBooks.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(bookUseID.attribute["ID"]));
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
                            DM.XmlDataNode madeDropBookNode = DM.EditGameData_BookInfos.MakeNewStaticDropBookBase(addedDropBookItemID);
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

                                List<DM.XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropBookItemID } });
                                if(baseBookUseNode.Count > 0)
                                {
                                    DM.XmlDataNode FOUND_DROP_BOOK_IN_GAME = baseBookUseNode[0];

                                    List<string> foundDropBookInGameItems = new List<string>();
                                    FOUND_DROP_BOOK_IN_GAME.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                                    {
                                        foundDropBookInGameItems.Add(DropItem.innerText);
                                    });

                                    if (DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                          attributeToCheck: new Dictionary<string, string>() { { "ID", innerCriticalPageNode.GetAttributesSafe("ID") } }).Count == 1
                                          && !foundDropBookInGameItems.Contains(innerCriticalPageNode.GetAttributesSafe("ID")))
                                    {
                                        FOUND_DROP_BOOK_NODE.RemoveXmlInfosByPath("DropItem", innerCriticalPageNode.attribute["ID"], deleteOnce: true);
                                    }

                                    List<string> foundDropBookDropItems = new List<string>();
                                    FOUND_DROP_BOOK_NODE.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                                    {
                                        foundDropBookDropItems.Add(DropItem.innerText);
                                    });

                                    if (foundDropBookDropItems.Count == foundDropBookInGameItems.Count
                                        && foundDropBookDropItems.Except(foundDropBookInGameItems).Count() == 0
                                        && DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                          attributeToCheck: new Dictionary<string, string>() { { "ID", innerCriticalPageNode.GetAttributesSafe("ID") } }).Count == 1)
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
                            extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(dropBookInfo)}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                        BtnDropBooks.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnDropBooks_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                    }
                    else
                    {
                        BtnDropBooks.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                        BtnDropBooks.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnDropBooks_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    #endregion
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK);
                    break;
                case "BookUniqueCards":
                    List<string> selectedUniqCards = new List<string>();
                    innerCriticalPageNode.GetXmlDataNodesByPath("EquipEffect/OnlyCard").ForEachSafe((DM.XmlDataNode onlyCardNode) =>
                    {
                        selectedUniqCards.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(onlyCardNode.GetInnerTextSafe()));
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
                            extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(xmlDataNode.GetInnerTextSafe())}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesUniqueCard.png");
                        BookUniqueCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BookUniqueCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                    }
                    else
                    {
                        BookUniqueCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoUniqueCard.png");
                        BookUniqueCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BookUniqueCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
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

                            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BOOKS);
                            MainWindow.mainWindow.UpdateDebugInfo();
                        }
                    }, prevStory,
                    windowTitle: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"KEY_PAGE_STORY_TITLE"),
                    tbxToolTip: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"%TbxData_ToolTip%")).ShowDialog();

                    List<DM.XmlDataNode> foundXmlDataNodes = DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                    if (foundXmlDataNodes.Count > 0)
                    {
                        BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                        BtnCiricalBookInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnCiricalBookInfo_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})";
                    }
                    else
                    {
                        BtnCiricalBookInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNobookInfo.png");
                        BtnCiricalBookInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnCiricalBookInfo_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    break;
                case "BtnEnemySetting":
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    new SubWindows.Global_MultipleValueInputed(new Dictionary<string, string>() {
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"StartPlayPoint"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"StartPlayPoint_ToolTip")},
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"MaxPlayPoint"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"MaxPlayPoint_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EmotionLevel"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EmotionLevel_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"AddedStartDraw"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"AddedStartDraw_ToolTip")}
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
                        extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"StartPlayPoint")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/StartPlayPoint")}\n";
                        extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"MaxPlayPoint")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/MaxPlayPoint")}\n";
                        extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"EmotionLevel")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/EmotionLevel")}\n";
                        extraInfo += $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"AddedStartDraw")} : {innerCriticalPageNode.GetInnerTextByPath("EquipEffect/AddedStartDraw")}";

                        BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesEnemy.png");
                        BtnEnemySetting.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnEnemySetting_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                    }
                    else
                    {
                        BtnEnemySetting.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoEnemy.png");
                        BtnEnemySetting.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"%BtnEnemySetting_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    break;
                case "BtnCopyPage":
                    DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Add(innerCriticalPageNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    break;
                case "BtnDelete":
                    if( DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.CheckIfGivenPathWithXmlInfoExists("bookDescList/BookDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } }))
                    {
                        if(DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book/TextId", 
                            innerCriticalPageNode.GetInnerTextByPath("TextId")).Count == 1)
                            DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.RemoveXmlInfosByPath("bookDescList/BookDesc",
                                attributeToCheck: new Dictionary<string, string>() { { "BookID", innerCriticalPageNode.GetAttributesSafe("ID") } });
                    }

                    DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPath("BookUse").ForEachSafe((DM.XmlDataNode bookUseNode) =>
                    {
                        List<DM.XmlDataNode> baseBookUseNode = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", bookUseNode.attribute["ID"] } });
                        if (baseBookUseNode.Count > 0)
                        {
                            DM.XmlDataNode FOUND_DROP_BOOK_IN_GAME = baseBookUseNode[0];

                            List<string> foundDropBookInGameItems = new List<string>();
                            FOUND_DROP_BOOK_IN_GAME.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                            {
                                foundDropBookInGameItems.Add(DropItem.innerText);
                            });

                            if (DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", innerCriticalPageNode.GetAttributesSafe("ID") } }).Count == 1
                                && !foundDropBookInGameItems.Contains(innerCriticalPageNode.GetAttributesSafe("ID")))
                            {
                                bookUseNode.RemoveXmlInfosByPath("DropItem", innerCriticalPageNode.attribute["ID"], deleteOnce: true);
                            }

                            List<string> foundDropBookDropItems = new List<string>();
                            bookUseNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode DropItem) =>
                            {
                                foundDropBookDropItems.Add(DropItem.innerText);
                            });

                            if (foundDropBookDropItems.Count == foundDropBookInGameItems.Count
                                && foundDropBookDropItems.Except(foundDropBookInGameItems).Count() == 0
                                && DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", innerCriticalPageNode.GetAttributesSafe("ID") } }).Count == 1)
                            {
                                DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.RemoveXmlInfosByPath("BookUse",
                                    attributeToCheck: new Dictionary<string, string> { { "ID", bookUseNode.attribute["ID"] } }, deleteOnce: true);
                            }
                        }});

                    DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Remove(innerCriticalPageNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
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

            if (loopButton.Tag == null || LOOP_LIST.IndexOf(loopButton.Tag.ToString()) < 0)
                loopButton.Tag = LOOP_LIST[0];
            // Down index
            else if (e.LeftButton == MouseButtonState.Pressed)
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
            {
                innerCriticalPageNode.SetXmlInfoByPath($"EquipEffect/{loopButton.Name.Split('_').Last()}", loopButton.Tag.ToString());
                loopButton.Content = DM.LocalizedGameDescriptions.GetDescriptionForResist(loopButton.Tag.ToString());
                loopButton.ToolTip = DM.LocalizedGameDescriptions.GetDescriptionForResist(loopButton.Tag.ToString());
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
            }
            else if (loopButton.Name == "BtnRangeType")
            {
                string RANGE_NAME = (string.IsNullOrEmpty(loopButton.Tag.ToString()) ? "Near" : loopButton.Tag.ToString());
                BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{RANGE_NAME}.png");
                innerCriticalPageNode.SetXmlInfoByPathAndEmptyWillRemove("RangeType", loopButton.Tag.ToString());

                if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Range")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeRange.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeRange")})";
                }
                else if (innerCriticalPageNode.GetInnerTextByPath("RangeType") == "Hybrid")
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeHybrid.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeHybrid")})";
                }
                else
                {
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNear.png");
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"BtnRangeType_ToolTip")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"TypeNear")})";
                }
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
            }
        }
        #endregion


        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerCriticalPageNode == null)
                return;

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
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
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
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BOOKS);
                    break;
                default:
                    List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                    if (SPLIT_NAME.Count == 2)
                        innerCriticalPageNode.SetXmlInfoByPath(SPLIT_NAME.Last(), tbx.Text);
                    else if (SPLIT_NAME.Count > 2)
                        innerCriticalPageNode.SetXmlInfoByPath(String.Join("/", SPLIT_NAME.Skip(1)), tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                    break;
            }
        }
    }
}

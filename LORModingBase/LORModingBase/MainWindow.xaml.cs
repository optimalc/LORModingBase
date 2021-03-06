﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LORModingBase.CustomExtensions;

namespace LORModingBase
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow = null;

        #region Init controls
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            MainWindowButtonClickEvents(BtnCriticalPage, null);

            try
            {
                LoadAllRelatedDatasAfterChangePath();

                Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.MAIN_WINDOW);
                ReloadAllStackDatas();
                InitLbxTextEditor();
                DLLEditor.CodeBlockDataManagement.LoadData();
                DLLEditor.AutoGenerateCodeBlockDataManagement.LoadData();
                DLLEditor.CodeBlockPresetDataManagerment.LoadData();
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindow_Error"));
            }
        }

        private void ReloadAllStackDatas()
        {
            string SAVED_SELECTED_ITEM = "";
            if (LbxTextEditor.SelectedItem != null)
                SAVED_SELECTED_ITEM = LbxTextEditor.SelectedItem.ToString();

            InitSplCriticalPage();
            InitSplCards();

            InitSplStages();
            InitSplEnemy();
            InitSplDecks();

            InitSplDropBooks();
            InitSplPassives();
            InitSplCardAbilities();
            InitSplBuff();

            if(!string.IsNullOrEmpty(SAVED_SELECTED_ITEM))
                LbxTextEditor.SelectedItem = SAVED_SELECTED_ITEM;
        }

        /// <summary>
        /// Load all related datas after path is changed
        /// </summary>
        private void LoadAllRelatedDatasAfterChangePath()
        {
            bool configFileExisted = File.Exists(DS.PROGRAM_PATHS.CONFIG);
            DM.Config.LoadData();
            DM.LocalizeCore.LoadAllDatas();
            if (!configFileExisted)
                MainWindowButtonClickEvents(BtnConfig, null);

            #region Check LOR folder exists. If exists, init LblLORPath
            if (!Directory.Exists(DM.Config.config.LORFolderPath) || !Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data") 
                || string.IsNullOrEmpty(DM.Config.config.LORFolderPath))
            {
                DM.Config.config.LORFolderPath = "";
                DM.Config.SaveData();
                throw new Exception(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"LoadAllRelatedDatasAfterChangePath_Error_1"));
            }
            #endregion
            #region Check LOR mode resources
            if(!Directory.Exists(DM.Config.GAME_RESOURCE_PATHS.RESOURCE_ROOT_STATIC))
                throw new Exception(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"LoadAllRelatedDatasAfterChangePath_Error_2"));
            #endregion

            if (!Directory.Exists(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS);

            if (File.Exists(DS.PROGRAM_PATHS.VERSION))
            {
                if(DM.Config.config.isDeveloperMode)
                {
                    this.Title = $"LOR Moding Base {File.ReadAllText(DS.PROGRAM_PATHS.VERSION)} (Developer Mode)";
                    BtnLanguageReload.Visibility = Visibility.Visible;
                    BtnProgramReload.Visibility = Visibility.Visible;
                }
                else
                {
                    this.Title = $"LOR Moding Base {File.ReadAllText(DS.PROGRAM_PATHS.VERSION)}";
                    BtnLanguageReload.Visibility = Visibility.Collapsed;
                    BtnProgramReload.Visibility = Visibility.Collapsed;
                }
            }

            DM.GameInfos.LoadAllDatas();

            MainWindow.EDITOR_SELECTION_MENU.Clear();
            DM.EditGameData_BookInfos.InitDatas();
            DM.EditGameData_CardInfos.InitDatas();
            DM.EditGameData_StageInfo.InitDatas();
            DM.EditGameData_EnemyInfo.InitDatas();
            DM.EditGameData_DeckInfo.InitDatas();
            DM.EditGameData_DropBookInfo.InitDatas();
            DM.EditGameData_PassiveInfo.InitDatas();
            DM.EditGameData_CardAbilityInfo.InitDatas();
            DM.EditGameData_Buff.InitDatas();
        }
        #endregion

        #region Main window events
        /// <summary>
        /// Hide all main window menu grids
        /// </summary>
        private void HideAllGrid()
        {
            GrdCriticalPage.Visibility = Visibility.Collapsed;
            BtnCriticalPage.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdCards.Visibility = Visibility.Collapsed;
            BtnCards.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");


            GrdStages.Visibility = Visibility.Collapsed;
            BtnStages.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdEmemy.Visibility = Visibility.Collapsed;
            BtnEnemy.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdDecks.Visibility = Visibility.Collapsed;
            BtnDecks.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");


            GrdDropBooks.Visibility = Visibility.Collapsed;
            BtnDropBook.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdPassives.Visibility = Visibility.Collapsed;
            BtnPassive.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdCardAbilities.Visibility = Visibility.Collapsed;
            BtnCardAbility.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdBuff.Visibility = Visibility.Collapsed;
            BtnBuff.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");
        }

        /// <summary>
        /// Main window button events
        /// </summary>
        private void MainWindowButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnCriticalPage":
                        HideAllGrid();
                        GrdCriticalPage.Visibility = Visibility.Visible;
                        BtnCriticalPage.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_EQUIP_PAGE);
                        break;
                    case "BtnCards":
                        HideAllGrid();
                        GrdCards.Visibility = Visibility.Visible;
                        BtnCards.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_CARD);
                        break;

                    case "BtnStages":
                        HideAllGrid();
                        GrdStages.Visibility = Visibility.Visible;
                        BtnStages.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_STAGE_INFO);
                        break;
                    case "BtnEnemy":
                        HideAllGrid();
                        GrdEmemy.Visibility = Visibility.Visible;
                        BtnEnemy.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_ENEMY_INFO);
                        break;
                    case "BtnDecks":
                        HideAllGrid();
                        GrdDecks.Visibility = Visibility.Visible;
                        BtnDecks.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_DECKS);
                        break;

                    case "BtnDropBook":
                        HideAllGrid();
                        GrdDropBooks.Visibility = Visibility.Visible;
                        BtnDropBook.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                        break;
                    case "BtnPassive":
                        HideAllGrid();
                        GrdPassives.Visibility = Visibility.Visible;
                        BtnPassive.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.STATIC_PASSIVE_INTO);
                        break;
                    case "BtnCardAbility":
                        HideAllGrid();
                        GrdCardAbilities.Visibility = Visibility.Visible;
                        BtnCardAbility.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.LOCALIZED_CARD_ABILITY_DESC);
                        break;

                    case "BtnBuff":
                        HideAllGrid();
                        GrdBuff.Visibility = Visibility.Visible;
                        BtnBuff.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        ChangeDebugLocation(DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                        break;

                    case "BtnSetWorkingSpace":
                        new SubWindows.Global_ListSeleteWithEditWindow(null, null, null, null,
                            SubWindows.Global_ListSeleteWithEditWindow_PRESET.WORKING_SPACE).ShowDialog();

                        LblSetWorkingSpace.Content = $"> {DM.Config.CurrentWorkingDirectory.Split('\\').Last()}";
                        LblSetWorkingSpace.ToolTip = DM.Config.CurrentWorkingDirectory;

                        ReloadAllStackDatas();
                        InitLbxTextEditor();
                        break;

                    case "BtnStartGame":
                        Tools.ProcessTools.StartProcess($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina.exe");
                        if (DM.Config.config.isLogPlusMod)
                        {
                            if (File.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt"))
                                File.WriteAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt", "");
                            if (File.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt"))
                                File.WriteAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt", "");

                            new SubWindows.ExtraLogWindow().Show();
                        }
                        break;

                    case "BtnOpenBaseFolder":
                        if (Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods"))
                            Tools.ProcessTools.OpenExplorer($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods");
                        break;
                    case "BtnOpenExportFolder":
                        if (Directory.Exists($"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes"))
                            Tools.ProcessTools.OpenExplorer($"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes");
                        break;

                    case "BtnExtURL":
                        new SubWindows.Global_ListSeleteWindow(null, SubWindows.Global_ListSeleteWindow_PRESET.EXT_URL).ShowDialog();
                        break;
                    case "BtnTools":
                        new SubWindows.ExtraToolsWindow().ShowDialog();
                        break;
                    case "BtnConfig":
                        new SubWindows.OptionWindow(LoadAllRelatedDatasAfterChangePath).ShowDialog();
                        break;

                    case "BtnLanguageReload":
                        DM.LocalizeCore.LoadAllDatas();
                        ReloadAllStackDatas();
                        break;
                    case "BtnProgramReload":
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                        break;

                    case "BtnResources":
                        if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                        {
                            MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                            return;
                        }
                        new SubWindows.ResourceWindow().ShowDialog();
                        break;
                    case "BtnOpenSkinWindow":
                        if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                        {
                            MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                            return;
                        }
                        new SubWindows.CustomSkinWindow().ShowDialog();
                        break;
                    case "BtnDLLEdit":
                        if (string.IsNullOrEmpty(DM.Config.config.DLLCompilerPath))
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"DLLCompilerPathError2"));
                            MainWindowButtonClickEvents(BtnConfig, null);
                            return;
                        }
                        if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                        {
                            MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                            return;
                        }

                        new DLLEditor.DLLEditorMainWindow().ShowDialog();
                        break;
                    case "BtnAutoDLLGenerate":
                        if (string.IsNullOrEmpty(DM.Config.config.DLLCompilerPath))
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"DLLCompilerPathError2"));
                            MainWindowButtonClickEvents(BtnConfig, null);
                            return;
                        }
                        if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                        {
                            MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                            return;
                        }

                        new DLLEditor.AutoGenerateDLLWindow().ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_6"));
            }
        } 
        #endregion


        #region EDIT MENU - Critical Page Infos
        private void InitSplCriticalPage()
        {
            SplCriticalPage.Children.Clear();
            DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode xmlDataNode) =>
            {
                SplCriticalPage.Children.Add(new UC.EditCriticalPage(xmlDataNode, InitSplCriticalPage));
            });
        }

        private void CriticalPageGridButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddCriticalBook":
                        DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Add(
                            DM.EditGameData_BookInfos.MakeNewStaticEquipPageBase());
                        InitSplCriticalPage();
                        break;
                    case "BtnLoadCriticalBook":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundEqNodes = DM.GameInfos.staticInfos["EquipPage"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundEqNodes.Count <= 0)
                                foundEqNodes = DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Book",
                                        attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundEqNodes.Count > 0)
                            {
                                DM.XmlDataNode EQ_NODE_TO_USE = foundEqNodes[0].Copy();
                                int KEY_PAGE_ID = Convert.ToInt32(selectedItem);
                                if (KEY_PAGE_ID > 9000000 && KEY_PAGE_ID < 9999999)
                                    EQ_NODE_TO_USE.SetXmlInfoByPath("Name",
                                        DM.LocalizedGameDescriptions.GetDescriptionForBooks(selectedItem));
                                else
                                    EQ_NODE_TO_USE.SetXmlInfoByPath("Name",
                                        DM.LocalizedGameDescriptions.GetDescriptionForBooks(EQ_NODE_TO_USE.GetInnerTextByPath("TextId")));

                                DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.subNodes.Add(EQ_NODE_TO_USE);

                                List<DM.XmlDataNode> foundLocalizeBooks = DM.GameInfos.localizeInfos["Books"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("bookDescList/BookDesc",
                                    attributeToCheck: new Dictionary<string, string>() { { "BookID", selectedItem } });
                                if(foundLocalizeBooks.Count > 0)
                                {
                                    if (!DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.CheckIfGivenPathWithXmlInfoExists("bookDescList/BookDesc",
                                               attributeToCheck: new Dictionary<string, string>() { { "BookID", selectedItem } }))
                                    {
                                        DM.EditGameData_BookInfos.LocalizedBooks.rootDataNode.GetXmlDataNodesByPath("bookDescList").ActionOneItemSafe((DM.XmlDataNode bookDescList) =>
                                        {
                                            bookDescList.subNodes.Add(foundLocalizeBooks[0].Copy());
                                        });
                                    }
                                }

                                DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPath("BookUse").ForEach((DM.XmlDataNode bookUseNode) =>
                                {
                                    List<string> dropItems = new List<string>();
                                    bookUseNode.GetXmlDataNodesByPath("DropItem").ForEach((DM.XmlDataNode dropItemNode) =>
                                    {
                                        dropItems.Add(dropItemNode.innerText);
                                    });
                                    if(dropItems.Contains(selectedItem))
                                    {
                                        if(!DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.CheckIfGivenPathWithXmlInfoExists("BookUse", 
                                            attributeToCheck:new Dictionary<string, string>() { { "ID", bookUseNode.GetAttributesSafe("ID") } }))
                                            DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.subNodes.Add(bookUseNode.Copy());
                                    }
                                });

                                InitSplCriticalPage();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.CRITICAL_BOOKS).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"CriticalPageGridButtonClickEvents_Error_1"));
            }
        }
        #endregion   
        #region EDIT MENU - Cards Page
        private void InitSplCards()
        {
            SplCards.Children.Clear();
            DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode xmlDataNode) =>
            {
                SplCards.Children.Add(new UC.EditCard(xmlDataNode, InitSplCards));
            });
        }

        private void BattleCardGridButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddCard":
                        DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Add(
                            DM.EditGameData_CardInfos.MakeNewCardBase());
                        InitSplCards();
                        break;
                    case "BtnLoadCard":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundCardNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundCardNodes.Count <= 0)
                                foundCardNodes = DM.EditGameData_CardInfos.StaticCard.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                            attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundCardNodes.Count > 0)
                            {
                                DM.XmlDataNode CARD_NODE_TO_USE = foundCardNodes[0].Copy();
                                CARD_NODE_TO_USE.SetXmlInfoByPath("Name",
                                    DM.LocalizedGameDescriptions.GetDecriptionForCard(CARD_NODE_TO_USE.GetAttributesSafe("ID")));

                                DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Add(CARD_NODE_TO_USE);

                                List<DM.XmlDataNode> foundLocalizeCards = DM.GameInfos.localizeInfos["BattlesCards"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                                if (foundLocalizeCards.Count > 0)
                                {
                                    if (!DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.CheckIfGivenPathWithXmlInfoExists("cardDescList/BattleCardDesc",
                                               attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } }))
                                    {
                                        DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.GetXmlDataNodesByPath("cardDescList").ActionOneItemSafe((DM.XmlDataNode cardDescList) =>
                                        {
                                            cardDescList.subNodes.Add(foundLocalizeCards[0].Copy());
                                        });
                                    }
                                }

                                DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPath("DropTable").ForEach((DM.XmlDataNode dropTableNode) =>
                                {
                                    List<string> deopCards = new List<string>();
                                    dropTableNode.GetXmlDataNodesByPath("Card").ForEach((DM.XmlDataNode dropCardNode) =>
                                    {
                                        deopCards.Add(dropCardNode.innerText);
                                    });
                                    if (deopCards.Contains(selectedItem))
                                    {
                                        if (!DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                                            attributeToCheck: new Dictionary<string, string>() { { "ID", dropTableNode.GetAttributesSafe("ID") } }))
                                            DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.subNodes.Add(dropTableNode.Copy());
                                    }
                                });
                                InitSplCards();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.CARDS).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Stage Infos
        private void InitSplStages()
        {
            SplStages.Children.Clear();
            DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.ActionXmlDataNodesByPath("Stage", (DM.XmlDataNode xmlDataNode) =>
            {
                SplStages.Children.Add(new UC.EditStage(xmlDataNode, InitSplStages));
            });
        }

        private void StageGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddStage":
                        DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.subNodes.Add(
                        DM.EditGameData_StageInfo.MakeNewStageInfoBase());
                        InitSplStages();
                        break;
                    case "BtnLoadStage":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundStageIds = DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                                    attributeToCheck: new Dictionary<string, string>() { { "id", selectedItem } });
                            if (foundStageIds.Count <= 0)
                                foundStageIds = DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                                    attributeToCheck: new Dictionary<string, string>() { { "id", selectedItem } });

                            if (foundStageIds.Count > 0)
                            {
                                DM.XmlDataNode STAGE_NODE_TO_USE = foundStageIds[0].Copy();
                                DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.subNodes.Add(STAGE_NODE_TO_USE);

                                List <DM.XmlDataNode> foundLocalizeStageNames = DM.GameInfos.localizeInfos["StageName"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                                if (foundLocalizeStageNames.Count > 0)
                                {
                                    STAGE_NODE_TO_USE.SetXmlInfoByPath("Name", foundLocalizeStageNames[0].innerText);
                                    if (!DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                                               attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } }))
                                    {
                                        DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.subNodes.Add(foundLocalizeStageNames[0].Copy());
                                    }
                                }
                                InitSplStages();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Enemy Infos
        private void InitSplEnemy()
        {
            SplEnemy.Children.Clear();
            DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.ActionXmlDataNodesByPath("Enemy", (DM.XmlDataNode xmlDataNode) =>
            {
                SplEnemy.Children.Add(new UC.EditEnemyUnit(xmlDataNode, InitSplEnemy));
            });
        }

        private void EnemyGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddEnemy":
                        DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.subNodes.Add(
                        DM.EditGameData_EnemyInfo.MakeNewEnemyUnitInfoBase());
                        InitSplEnemy();
                        break;
                    case "BtnLoadEnemy":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundEnemyIds = DM.GameInfos.staticInfos["EnemyUnitInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Enemy",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundEnemyIds.Count <= 0)
                                foundEnemyIds = DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Enemy",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundEnemyIds.Count > 0)
                            {
                                DM.XmlDataNode ENEMY_NODE_TO_USE = foundEnemyIds[0].Copy();
                                DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.subNodes.Add(ENEMY_NODE_TO_USE);

                                List<DM.XmlDataNode> foundLocalizeCharNames = DM.GameInfos.localizeInfos["CharactersName"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", ENEMY_NODE_TO_USE.GetInnerTextByPath("NameID") } });
                                if (foundLocalizeCharNames.Count > 0)
                                {
                                    if (!DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                                               attributeToCheck: new Dictionary<string, string>() { { "ID", ENEMY_NODE_TO_USE.GetInnerTextByPath("NameID") } }))
                                    {
                                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.subNodes.Add(foundLocalizeCharNames[0].Copy());
                                    }
                                }
                                InitSplEnemy();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.ENEMIES).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Deck Infos
        private void InitSplDecks()
        {
            SplDecks.Children.Clear();
            DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.ActionXmlDataNodesByPath("Deck", (DM.XmlDataNode xmlDataNode) =>
            {
                SplDecks.Children.Add(new UC.EditDeck(xmlDataNode, InitSplDecks));
            });
        }

        private void DecksGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddDecks":
                        DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Add(
                        DM.EditGameData_DeckInfo.MakeNewDeckInfoBase());
                        InitSplDecks();
                        break;
                    case "BtnLoadDecks":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundDeckIds = DM.GameInfos.staticInfos["Deck"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Deck",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundDeckIds.Count <= 0)
                                foundDeckIds = DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Deck",
                                                attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundDeckIds.Count > 0)
                            {
                                DM.XmlDataNode DECK_NODE_TO_USE = foundDeckIds[0].Copy();
                                DECK_NODE_TO_USE.RemoveXmlInfosByPath("Random");
                                DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Add(DECK_NODE_TO_USE);
                                InitSplDecks();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.DECKS).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Drop Book Infos
        private void InitSplDropBooks()
        {
            SplDropBooks.Children.Clear();
            DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode xmlDataNode) =>
            {
                SplDropBooks.Children.Add(new UC.EditDropBook(xmlDataNode, InitSplDropBooks));
            });
        }

        private void DropBookGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddDropBooks":
                        DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.subNodes.Add(
                        DM.EditGameData_DropBookInfo.MakeNewDropBookInfoBase());
                        InitSplDropBooks();
                        break;
                    case "BtnLoadDropBooks":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundDropBookIds = DM.GameInfos.staticInfos["DropBook"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundDropBookIds.Count <= 0)
                                foundDropBookIds = DM.EditGameData_BookInfos.StaticDropBook.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundDropBookIds.Count > 0)
                            {
                                DM.XmlDataNode DROP_BOOK_NODE_TO_USE = foundDropBookIds[0].Copy();
                                DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.subNodes.Add(DROP_BOOK_NODE_TO_USE);


                                #region Add localized book name
                                List<DM.XmlDataNode> foundLocalizeBookName = DM.GameInfos.localizeInfos["etc"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("text",
                                                                                                   attributeToCheck: new Dictionary<string, string>() { { "id", DROP_BOOK_NODE_TO_USE.GetInnerTextByPath("TextId") } });
                                if (foundLocalizeBookName.Count > 0)
                                {
                                    if (!DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("text",
                                               attributeToCheck: new Dictionary<string, string>() { { "id", DROP_BOOK_NODE_TO_USE.GetInnerTextByPath("TextId") } }))
                                    {
                                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.subNodes.Add(foundLocalizeBookName[0].Copy());
                                    }
                                }
                                #endregion
                                #region Add additionanl card table info
                                List<DM.XmlDataNode> foundCardDropTables = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                                        attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                                if (foundCardDropTables.Count > 0)
                                {
                                    if (!DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                                            attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } }))
                                    {
                                        DM.XmlDataNode CARD_DROP_TABLE_TO_USE = foundCardDropTables[0].Copy();
                                        DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.subNodes.Add(CARD_DROP_TABLE_TO_USE);
                                    }
                                }
                                #endregion

                                InitSplDropBooks();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.DROP_BOOK).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Passive Infos
        private void InitSplPassives()
        {
            SqlPassives.Children.Clear();
            DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.ActionXmlDataNodesByPath("Passive", (DM.XmlDataNode xmlDataNode) =>
            {
                SqlPassives.Children.Add(new UC.EditPassive(xmlDataNode, InitSplPassives));
            });
        }

        private void PassiveGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddPassives":
                        DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.subNodes.Add(
                        DM.EditGameData_PassiveInfo.MakeNewPassiveListBase());
                        InitSplPassives();
                        break;
                    case "BtnLoadPassives":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundPassiveIds = DM.GameInfos.staticInfos["PassiveList"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Passive",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundPassiveIds.Count <= 0)
                                foundPassiveIds = DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Passive",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundPassiveIds.Count > 0)
                            {
                                DM.XmlDataNode PASSIVE_NODE_TO_USE = foundPassiveIds[0].Copy();
                                DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.subNodes.Add(PASSIVE_NODE_TO_USE);


                                #region Add localized book name
                                List<DM.XmlDataNode> foundLocalizePassiveDesc = DM.GameInfos.localizeInfos["PassiveDesc"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                                                                                                   attributeToCheck: new Dictionary<string, string>() { { "ID", PASSIVE_NODE_TO_USE.GetAttributesSafe("ID") } });
                                if (foundLocalizePassiveDesc.Count > 0)
                                {
                                    if (!DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.CheckIfGivenPathWithXmlInfoExists("PassiveDesc",
                                               attributeToCheck: new Dictionary<string, string>() { { "ID", PASSIVE_NODE_TO_USE.GetAttributesSafe("ID") } }))
                                    {
                                        DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.subNodes.Add(foundLocalizePassiveDesc[0].Copy());
                                    }
                                }
                                #endregion

                                InitSplPassives();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.PASSIVE).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Card Ability Infos
        private void InitSplCardAbilities()
        {
            SqlCardAbilies.Children.Clear();
            DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode xmlDataNode) =>
            {
                SqlCardAbilies.Children.Add(new UC.EditCardAbility(xmlDataNode, InitSplCardAbilities));
            });
        }

        private void CardAbilityGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddCardAbility":
                        DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.subNodes.Add(
                        DM.EditGameData_CardAbilityInfo.MakeNewCardAbilityInfoBase());
                        InitSplCardAbilities();
                        break;
                    case "BtnLoadCardAbility":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundCardAbilityIds = DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BattleCardAbility",
                           attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundCardAbilityIds.Count <= 0)
                                foundCardAbilityIds = DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BattleCardAbility",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundCardAbilityIds.Count > 0)
                            {
                                DM.XmlDataNode CARD_ABILITY_NODE_TO_USE = foundCardAbilityIds[0].Copy();
                                DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.subNodes.Add(CARD_ABILITY_NODE_TO_USE);
                                InitSplCardAbilities();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.ALL_ABILITES).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion
        #region EDIT MENU - Buff Infos
        private void InitSplBuff()
        {
            SqlBuffs.Children.Clear();
            DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList/BattleEffectText", (DM.XmlDataNode xmlDataNode) =>
            {
                SqlBuffs.Children.Add(new UC.EditBuff(xmlDataNode, InitSplBuff));
            });
        }

        private void BuffGridButtonClickEvents(object sender, RoutedEventArgs e)
        {

            Button clickButton = sender as Button;
            try
            {
                if (string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
                {
                    MainWindowButtonClickEvents(BtnSetWorkingSpace, null);
                    return;
                }

                switch (clickButton.Name)
                {
                    case "BtnAddBuff":
                        DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList", (DM.XmlDataNode effectTextList) => {
                            effectTextList.subNodes.Add(
                                DM.EditGameData_Buff.MakeNewBuffInfoBase());
                        });
                        InitSplBuff();
                        break;
                    case "BtnLoadBuff":
                        new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                        {
                            List<DM.XmlDataNode> foundBuffIds = DM.GameInfos.localizeInfos["EffectTexts"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("effectTextList/BattleEffectText",
                           attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });
                            if (foundBuffIds.Count <= 0)
                                foundBuffIds = DM.EditGameData_Buff.LocalizedBuff.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("effectTextList/BattleEffectText",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", selectedItem } });

                            if (foundBuffIds.Count > 0)
                            {
                                DM.XmlDataNode BUFF_NODE_TO_USE = foundBuffIds[0].Copy();
                                DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList", (DM.XmlDataNode effectTextList) => {
                                    effectTextList.subNodes.Add(
                                        BUFF_NODE_TO_USE);
                                });
                                InitSplBuff();
                            }
                        }, SubWindows.InputInfoWithSearchWindow_PRESET.BUFFS).ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"BattleCardGridButtonClickEvents_Error_1"));
            }
        }
        #endregion


        #region Text editor functions
        public static List<string> EDITOR_SELECTION_MENU = new List<string>();

        private void InitLbxTextEditor()
        {
            LbxTextEditor.Items.Clear();
            EDITOR_SELECTION_MENU.ForEach((string menu) =>
            {
                LbxTextEditor.Items.Add(menu);
            });
            if (LbxTextEditor.Items.Count > 0)
                LbxTextEditor.SelectedIndex = 0;
        }

        /// <summary>
        /// Update debugging info
        /// </summary>
        public void UpdateDebugInfo()
        {
            if (!string.IsNullOrEmpty(DM.Config.CurrentWorkingDirectory))
            {
                string LOG_MESSAGES = "";

                try
                {

                    if (GrdCriticalPage.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_BookInfos.StaticEquipPage.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticEquipPage, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_BookInfos.StaticDropBook.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticDropBook, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_BookInfos.LocalizedBooks.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_BookInfos.LocalizedBooks, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdCards.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_CardInfos.StaticCard.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCard, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_CardInfos.StaticCardDropTable.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCardDropTable, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_CardInfos.LocalizedBattleCards.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardInfos.LocalizedBattleCards, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdStages.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_StageInfo.StaticStageInfo.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_StageInfo.StaticStageInfo, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_StageInfo.LocalizedStageName.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_StageInfo.LocalizedStageName, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdEmemy.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_EnemyInfo.LocalizedCharactersName, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdDecks.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_DeckInfo.StaticDeckInfo.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_DeckInfo.StaticDeckInfo, DM.Config.CurrentWorkingDirectory));

                    }
                    if (GrdDropBooks.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_DropBookInfo.StaticDropBookInfo.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticDropBookInfo, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticCardDropTableInfo, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_DropBookInfo.LocalizedDropBookName, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdPassives.Visibility == Visibility.Visible)
                    {
                        DM.EditGameData_PassiveInfo.StaticPassiveList.SaveNodeData(DM.Config.GetStaticPathToSave(DM.EditGameData_PassiveInfo.StaticPassiveList, DM.Config.CurrentWorkingDirectory));
                        DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_PassiveInfo.LocalizedPassiveDesc, DM.Config.CurrentWorkingDirectory));
                    }
                    if (GrdCardAbilities.Visibility == Visibility.Visible)
                        DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardAbilityInfo.LocalizedCardAbility, DM.Config.CurrentWorkingDirectory));
                    if (GrdBuff.Visibility == Visibility.Visible)
                        DM.EditGameData_Buff.LocalizedBuff.SaveNodeData(DM.Config.GetLocalizePathToSave(DM.EditGameData_Buff.LocalizedBuff, DM.Config.CurrentWorkingDirectory));

                    string debugFileName = "";
                    if (LbxTextEditor.SelectedItem != null)
                    {
                        debugFileName = DM.Config.GetPathFromRelativePath(LbxTextEditor.SelectedItem.ToString(), DM.Config.CurrentWorkingDirectory);
                        if (File.Exists(debugFileName))
                            TbxTextEditor.Text = File.ReadAllText(debugFileName);
                    }

                    LOG_MESSAGES += $"[*] {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Output_Logging_Complete_1")}\n";

                }
                catch (Exception ex)
                {
                    LOG_MESSAGES += $"[!] {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Output_Logging_Error_1")} \n> {ex.Message}\n"; ;
                }

                try
                {
                    LOG_MESSAGES += DM.CheckDatas.CheckAllDatas();
                }
                catch (Exception ex)
                {
                    LOG_MESSAGES += $"[!] {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.LOGGING, $"Output_Logging_Error_2")} \n> {ex.Message}\n";
                }

                TbxTextEditorLog.Text = LOG_MESSAGES;
                TbxTextEditorLog.ToolTip = LOG_MESSAGES;
            }
        }

        /// <summary>
        /// Change debug location
        /// </summary>
        /// <param name="DEBUG_INDEX">Debug index to use</param>
        public void ChangeDebugLocation(DEBUG_LOCATION DEBUG_INDEX)
        {
            LbxTextEditor.SelectedIndex = (int)DEBUG_INDEX;
        }
        /// <summary>
        /// Debug information index
        /// </summary>
        public enum DEBUG_LOCATION
        { 
            STATIC_EQUIP_PAGE = 0,
            STATIC_DROP_BOOK = 1,
            LOCALIZED_BOOKS = 2,

            STATIC_CARD = 3,
            STATIC_CARD_DROP_TABLE=4,
            LOCALIZED_BATTLE_CARDS=5,

            STATIC_STAGE_INFO=6,
            LOCALIZED_STAGE_NAME=7,

            STATIC_ENEMY_INFO=8,
            LOCALIZED_CHAR_NAME=9,

            STATIC_DECKS=10,

            STATIC_DROP_BOOK_INFO=11,
            STATIC_CARD_DROP_TABLE_INFO=12,
            LOCALIZED_DROP_BOOK_NAME=13,

            STATIC_PASSIVE_INTO=14,
            LOCALIZED_PASSIVE_DESC=15,

            LOCALIZED_CARD_ABILITY_DESC=16,
            
            LOCALIZED_BUFF_DESC=17
        }


        private void EditorButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnReverseLoad":
                        if (LbxTextEditor.SelectedItem != null)
                        {
                            string FILE_NAME_TO_RELOAD = DM.Config.GetPathFromRelativePath(LbxTextEditor.SelectedItem.ToString(), DM.Config.CurrentWorkingDirectory);
                            if (File.Exists(FILE_NAME_TO_RELOAD))
                            {
                                File.WriteAllText(FILE_NAME_TO_RELOAD, TbxTextEditor.Text);
                                DM.XmlData RELOADED_XML_DATA = new DM.XmlData(FILE_NAME_TO_RELOAD);

                                switch(LbxTextEditor.SelectedIndex)
                                {
                                    case 0: DM.EditGameData_BookInfos.StaticEquipPage = RELOADED_XML_DATA; break;
                                    case 1: DM.EditGameData_BookInfos.StaticDropBook = RELOADED_XML_DATA; break;
                                    case 2: DM.EditGameData_BookInfos.LocalizedBooks = RELOADED_XML_DATA; break;

                                    case 3: DM.EditGameData_CardInfos.StaticCard = RELOADED_XML_DATA; break;
                                    case 4: DM.EditGameData_CardInfos.StaticCardDropTable = RELOADED_XML_DATA; break;
                                    case 5: DM.EditGameData_CardInfos.LocalizedBattleCards = RELOADED_XML_DATA; break;

                                    case 6: DM.EditGameData_StageInfo.StaticStageInfo = RELOADED_XML_DATA; break;
                                    case 7: DM.EditGameData_StageInfo.LocalizedStageName = RELOADED_XML_DATA; break;

                                    case 8: DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo = RELOADED_XML_DATA; break;
                                    case 9: DM.EditGameData_EnemyInfo.LocalizedCharactersName = RELOADED_XML_DATA; break;

                                    case 10: DM.EditGameData_DeckInfo.StaticDeckInfo = RELOADED_XML_DATA; break;

                                    case 11: DM.EditGameData_DropBookInfo.StaticDropBookInfo = RELOADED_XML_DATA; break;
                                    case 12: DM.EditGameData_DropBookInfo.StaticCardDropTableInfo = RELOADED_XML_DATA; break;
                                    case 13: DM.EditGameData_DropBookInfo.LocalizedDropBookName = RELOADED_XML_DATA; break;

                                    case 14: DM.EditGameData_PassiveInfo.StaticPassiveList = RELOADED_XML_DATA; break;
                                    case 15: DM.EditGameData_PassiveInfo.LocalizedPassiveDesc = RELOADED_XML_DATA; break;

                                    case 16: DM.EditGameData_CardAbilityInfo.LocalizedCardAbility = RELOADED_XML_DATA; break;

                                    case 17: DM.EditGameData_Buff.LocalizedBuff = RELOADED_XML_DATA; break;
                                }
                                ReloadAllStackDatas();
                            }
                            TbxTextEditor.Text = File.ReadAllText(FILE_NAME_TO_RELOAD);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_7"));
            }
        }
        #endregion

        private void LbxTextEditor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbxTextEditor.SelectedItem != null)
            {
                LblTextEditor.Content = LbxTextEditor.SelectedItem.ToString();
                LblTextEditor.ToolTip = LbxTextEditor.SelectedItem.ToString();
            }
            UpdateDebugInfo();
        }
    }
}

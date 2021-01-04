using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LORModingBase
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Init controls
        public MainWindow()
        {
            InitializeComponent();
            MainWindowButtonClickEvents(BtnCriticalPage, null);

            try
            {
                LoadAllRelatedDatasAfterChangePath();

                Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.MAIN_WINDOW);
                InitSplCriticalPage();
                InitSplCards();
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindow_Error"));
            }
        }

        /// <summary>
        /// Load all related datas after path is changed
        /// </summary>
        private void LoadAllRelatedDatasAfterChangePath()
        {
            DM.Config.LoadData();
            DM.LocalizeCore.LoadAllDatas();

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
            if(!Directory.Exists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}"))
                throw new Exception(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"LoadAllRelatedDatasAfterChangePath_Error_2"));
            #endregion

            if (!Directory.Exists(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS);

            if (File.Exists(DS.PROGRAM_PATHS.VERSION))
                this.Title = $"LOR Moding Base {File.ReadAllText(DS.PROGRAM_PATHS.VERSION)}";

            DM.GameInfos.LoadAllDatas();

            DM.EditGameData_BookInfos.InitDatas();
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
                        break;
                    case "BtnCards":
                        HideAllGrid();
                        GrdCards.Visibility = Visibility.Visible;
                        BtnCards.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
                        break;

                    case "BtnSave":
                        try
                        {
                            if (string.IsNullOrEmpty(TbxModeName.Text))
                                throw new Exception(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_1"));

                            DM.CheckDatas.CheckAllDatas();
                            string MOD_DIR_TO_USE = DM.ExportDatas.ExportAllDatas(TbxModeName.Text);

                            Tools.MessageBoxTools.ShowInfoMessageBox(
                                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Info_1"),
                                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Info_2"));
                            Tools.ProcessTools.OpenExplorer(MOD_DIR_TO_USE);

                            if (DM.Config.config.isExecuteAfterExport)
                                MainWindowButtonClickEvents(BtnStartGame, null);
                        }
                        catch (Exception ex)
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox(
                                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_2"),
                                ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_3"));
                        }
                        break;
                    case "BtnLoad":
                        try
                        {
                            Tools.Dialog.SelectDirectory((string selectedDir) =>
                            {
                                DM.ImportDatas.ImportAllDatas(selectedDir);
                                Tools.MessageBoxTools.ShowInfoMessageBox(
                                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Info_3")
                                    ,DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Info_2"));

                                InitSplCriticalPage();
                                InitSplCards();
                            }, $"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes");
                        }
                        catch (Exception ex)
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox(
                                DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_4"),
                                ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_5"));
                        }
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
                        new SubWindows.ToEXTUrl().ShowDialog();
                        break;
                    case "BtnTools":
                        new SubWindows.ExtraToolsWindow().ShowDialog();
                        break;
                    case "BtnConfig":
                        new SubWindows.OptionWindow(LoadAllRelatedDatasAfterChangePath).ShowDialog();
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
        public static List<DS.CriticalPageInfo> criticalPageInfos = new List<DS.CriticalPageInfo>();

        private void InitSplCriticalPage()
        {
            SplCriticalPage.Children.Clear();
            DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.ActionXmlDataNodesByName("Book", (DM.XmlDataNode xmlDataNode) =>
            {
                SplCriticalPage.Children.Add(new UC.EditCriticalPage(xmlDataNode, InitSplCriticalPage));
            });
        }

        private void CriticalPageGridButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnAddCriticalBook":
                        DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.AddXmlInfoByPath("Book",
                            attributePairsToSet: new Dictionary<string, string>() { { "ID", Tools.MathTools.GetRandomNumber(DS.FilterDatas.CARD_DIV_SPECIAL, DS.FilterDatas.CARD_DIV_FINAL_STORY).ToString() } });
                        InitSplCriticalPage();
                        break;
                    case "BtnLoadCriticalBook":
                        new SubWindows.InputGameCriticalPage((DS.CriticalPageInfo selectedCriticalPageInfo) =>
                        {
                            criticalPageInfos.Add(Tools.DeepCopy.DeepClone(selectedCriticalPageInfo));
                            InitSplCriticalPage();
                        }).ShowDialog();
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
        public static List<DS.CardInfo> cardInfos = new List<DS.CardInfo>();

        private void InitSplCards()
        {
            SplCards.Children.Clear();
            cardInfos.ForEach((DS.CardInfo cardInfo) =>
            {
                SplCards.Children.Add(new UC.EditCard(cardInfo, InitSplCards));
            });
        }

        private void BattleCardGridButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnAddCard":
                        cardInfos.Add(new DS.CardInfo());
                        InitSplCards();
                        break;
                    case "BtnLoadCard":
                        new SubWindows.InputGameCard((DS.CardInfo selectedCardInfo) =>
                        {
                            cardInfos.Add(Tools.DeepCopy.DeepClone(selectedCardInfo));
                            InitSplCards();
                        }).ShowDialog();
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
    }
}

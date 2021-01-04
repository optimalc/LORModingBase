using System;
using System.Collections.Generic;
using System.IO;
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
        #region Init controls
        public MainWindow()
        {
            InitializeComponent();
            MainWindowButtonClickEvents(BtnCriticalPage, null);

            try
            {
                LoadAllRelatedDatasAfterChangePath();

                Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.MAIN_WINDOW);
                ExtraLocalizeWindow();

                InitSplCriticalPage();
                InitSplCards();
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "초기화 과정 중 오류");
            }
        }

        private void ExtraLocalizeWindow()
        {
            BtnCriticalPage.Content = $"[{DM.GameInfos.localizeInfos["etc"].rootDataNode.GetInnerTextByAttribute("text", "id", "ui_passivesuccession_title", "Edit Key Page")}]";
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
                throw new Exception("설정된 라오루 폴더가 존재하지 않거나 적절하지 않습니다. 수동 설정이 필요합니다");
            }
            #endregion
            #region Check LOR mode resources
            if(!Directory.Exists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}"))
                throw new Exception("모드 리소스가 없습니다. (기반 모드를 적용시키고 라오루를 한번 실행시켜주세요)");
            #endregion

            if (!Directory.Exists(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS);

            if (File.Exists(DS.PROGRAM_PATHS.VERSION))
                this.Title = $"LOR Moding Base {File.ReadAllText(DS.PROGRAM_PATHS.VERSION)}";

            DM.GameInfos.LoadAllDatas();
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
                                throw new Exception("모드명이 입력되어 있지 않습니다.");

                            DM.CheckDatas.CheckAllDatas();
                            string MOD_DIR_TO_USE = DM.ExportDatas.ExportAllDatas(TbxModeName.Text);

                            Tools.MessageBoxTools.ShowInfoMessageBox("내보내기가 정상적으로 완료되었습니다.", "완료");
                            Tools.ProcessTools.OpenExplorer(MOD_DIR_TO_USE);

                            if (DM.Config.config.isExecuteAfterExport)
                                MainWindowButtonClickEvents(BtnStartGame, null);
                        }
                        catch (Exception ex)
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox("내보내는 도중 오류가 발생했습니다.", ex, "내보내기 오류");
                        }
                        break;
                    case "BtnLoad":
                        try
                        {
                            Tools.Dialog.SelectDirectory((string selectedDir) =>
                            {
                                DM.ImportDatas.ImportAllDatas(selectedDir);
                                Tools.MessageBoxTools.ShowInfoMessageBox("성공적으로 로드되었습니다.", "완료");

                                InitSplCriticalPage();
                                InitSplCards();
                            }, $"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes");
                        }
                        catch (Exception ex)
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox("불러오는 도중 오류가 발생했습니다.", ex, "불러오기 오류");
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
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "메인 윈도우의 버튼 클릭 이벤트에서 오류");
            }
        } 
        #endregion


        #region EDIT MENU - Critical Page Infos
        public static List<DS.CriticalPageInfo> criticalPageInfos = new List<DS.CriticalPageInfo>();

        private void InitSplCriticalPage()
        {
            SplCriticalPage.Children.Clear();
            criticalPageInfos.ForEach((DS.CriticalPageInfo criticalPageInfo) =>
            {
                SplCriticalPage.Children.Add(new UC.EditCriticalPage(criticalPageInfo, InitSplCriticalPage));
            });
        }

        private void BtnAddCriticalBook_Click(object sender, RoutedEventArgs e)
        {
            criticalPageInfos.Add(new DS.CriticalPageInfo());
            InitSplCriticalPage();
        }

        private void BtnLoadCriticalBook_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputGameCriticalPage((DS.CriticalPageInfo selectedCriticalPageInfo) =>
            {
                criticalPageInfos.Add(Tools.DeepCopy.DeepClone(selectedCriticalPageInfo));
                InitSplCriticalPage();
            }).ShowDialog();
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

        private void BtnAddCard_Click(object sender, RoutedEventArgs e)
        {
            cardInfos.Add(new DS.CardInfo());
            InitSplCards();
        }

        private void BtnLoadCard_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputGameCard((DS.CardInfo selectedCardInfo) =>
            {
                cardInfos.Add(Tools.DeepCopy.DeepClone(selectedCardInfo));
                InitSplCards();
            }).ShowDialog();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

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
            BtnCriticalPage_Click(null, null);

            try
            {
                InitLORPathResourceLabel();
                LocalizeWindow();

                InitSplCriticalPage();
                InitSplCards();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "초기화 과정 중 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LocalizeWindow()
        {
            BtnCriticalPage.Content = $"[{DM.GameInfos.localizeInfos["etc"].rootDataNode.GetInnerTextByAttribute("text", "id", "ui_passivesuccession_title", "Edit Key Page")}]";
        }

        /// <summary>
        /// Load config datas etc...
        /// </summary>
        private void LoadDatas()
        {
            if(!Directory.Exists(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PROGRAM_PATHS.DIC_EXPORT_DATAS);

            if (File.Exists(DS.PROGRAM_PATHS.VERSION))
                this.Title = $"LOR Moding Base {File.ReadAllText(DS.PROGRAM_PATHS.VERSION)}";

            DM.GameInfos.LoadAllDatas();
        }

        /// <summary>
        /// Reflect changed LOR path and load all static datas
        /// </summary>
        private void InitLORPathResourceLabel()
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
            
            LoadDatas();
        }
        #endregion
        
        #region Top menu button events
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TbxModeName.Text))
                    throw new Exception("모드명이 입력되어 있지 않습니다.");

                DM.CheckDatas.CheckAllDatas();
                string MOD_DIR_TO_USE = DM.ExportDatas.ExportAllDatas(TbxModeName.Text);

                MessageBox.Show("내보내기가 정상적으로 완료되었습니다.", "완료", MessageBoxButton.OK, MessageBoxImage.Information);
                Tools.ProcessTools.OpenExplorer(MOD_DIR_TO_USE);

                if (DM.Config.config.isExecuteAfterExport)
                    BtnStartGame_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"내보내는 도중 오류가 발생했습니다. : {ex.Message}", "내보내기 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.Dialog.SelectDirectory((string selectedDir) =>
                {
                    DM.ImportDatas.ImportAllDatas(selectedDir);
                    MessageBox.Show("성공적으로 로드되었습니다.", "완료", MessageBoxButton.OK, MessageBoxImage.Information);

                    InitSplCriticalPage();
                    InitSplCards();
                }, $"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"불러오는 도중 오류가 발생했습니다. : {ex.Message}", "불러오기 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.OptionWindow(InitLORPathResourceLabel).ShowDialog();
        }

        private void BtnExtURL_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.ToEXTUrl().ShowDialog();
        }

        private void BtnOpenBaseFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods"))
                Tools.ProcessTools.OpenExplorer($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods");
        }

        private void BtnOpenExportFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists($"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes"))
                Tools.ProcessTools.OpenExplorer($"{Tools.ProcessTools.GetWorkingDirectory()}\\exportedModes");
        }

        private void BtnTools_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.ExtraToolsWindow().ShowDialog();
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            Tools.ProcessTools.StartProcess($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina.exe");
            if (DM.Config.config.isLogPlusMod)
            {
                if (File.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt"))
                    File.WriteAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt", "");
                if (File.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt"))
                    File.WriteAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt", "");

                new SubWindows.ExtraLogWindow().Show();
            }
        }
        #endregion

        #region Top Second menu click event
        private void HideAllGrid()
        {
            GrdCriticalPage.Visibility = Visibility.Collapsed;
            BtnCriticalPage.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");

            GrdCards.Visibility = Visibility.Collapsed;
            BtnCards.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFFD9A3");
        }

        private void BtnCriticalPage_Click(object sender, RoutedEventArgs e)
        {
            HideAllGrid();
            GrdCriticalPage.Visibility = Visibility.Visible;
            BtnCriticalPage.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
        }

        private void BtnCards_Click(object sender, RoutedEventArgs e)
        {
            HideAllGrid();
            GrdCards.Visibility = Visibility.Visible;
            BtnCards.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFFDC61B");
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

        #region Left menu button events

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

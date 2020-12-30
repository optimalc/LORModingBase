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
        public static List<DS.CriticalPageInfo> criticalPageInfos = new List<DS.CriticalPageInfo>();

        #region Init controls
        public MainWindow()
        {
            InitializeComponent();

            InitLORPathResourceLabel();
            InitSplCriticalPage();
        } 

        /// <summary>
        /// Load config datas etc...
        /// </summary>
        private void LoadDatas()
        {
            if(!Directory.Exists(DS.PATH.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PATH.DIC_EXPORT_DATAS);

            if (File.Exists(DS.PATH.VERSION))
                this.Title = $"LOR Moding Base {File.ReadAllText(DS.PATH.VERSION)}";

            DM.StaticInfos.LoadAllDatas();
        }

        private void InitLORPathResourceLabel()
        {
            DM.Config.LoadData();
            #region Check LOR folder exists. If exists, init LblLORPath
            if (!Directory.Exists(DM.Config.config.LORFolderPath) || !Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data") 
                || string.IsNullOrEmpty(DM.Config.config.LORFolderPath))
            {
                MessageBox.Show("설정된 라오루 폴더가 존재하지 않거나 적절하지 않습니다. 수동 설정이 필요합니다.", "인식 실패", MessageBoxButton.OK, MessageBoxImage.Error);
                DM.Config.config.LORFolderPath = "";
                DM.Config.SaveData();
                LblLORPath.Content = "라오루 폴더 자동 인식 실패. 수동으로 설정해주세요.";
                LblLORPath.ToolTip = "라오루 폴더 자동 인식 실패. 수동으로 설정해주세요.";
                return;
            }
            else
            {
                LblLORPath.Content = DM.Config.config.LORFolderPath;
                LblLORPath.ToolTip = DM.Config.config.LORFolderPath;
            }
            #endregion
            #region Check LOR mode resources
            if(!Directory.Exists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}"))
            {
                MessageBox.Show("모드 리소스가 없습니다. (기반 모드를 적용시키고 라오루를 한번 실행시켜주세요)", "인식 실패", MessageBoxButton.OK, MessageBoxImage.Error);
                LblResourceCheck.Content = "X";
                LblResourceCheck.ToolTip = "모드 리소스가 없습니다. (기반 모드를 적용시키고 라오루를 한번 실행시켜주세요)";
                return;
            }
            else
            {
                LblResourceCheck.Content = "O";
                LblResourceCheck.ToolTip = "리소스가 정상적으로 발견되었습니다.";
            }
            #endregion
            
            LoadDatas();
        }

        private void InitSplCriticalPage()
        {
            SplCriticalPage.Children.Clear();
            criticalPageInfos.ForEach((DS.CriticalPageInfo criticalPageInfo) =>
            {
                SplCriticalPage.Children.Add(new UC.EditCriticalPage(criticalPageInfo));
            });
        }
        #endregion
        #region Click events
        private void BtnLORPath_Click(object sender, RoutedEventArgs e)
        {
            Tools.Dialog.SelectDirectory((string selectedDir) =>
            {
                DM.Config.config.LORFolderPath = selectedDir;
                DM.Config.SaveData();
                InitLORPathResourceLabel();
            });
        }

        private void BtnAddCriticalBook_Click(object sender, RoutedEventArgs e)
        {
            criticalPageInfos.Add(new DS.CriticalPageInfo());
            InitSplCriticalPage();
        }
        #endregion

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
            }
            catch(Exception ex)
            {
                MessageBox.Show($"내보내는 도중 오류가 발생했습니다. : {ex.Message}", "내보내기 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

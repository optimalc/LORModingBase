using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            LoadDatas();
            InitLORPathLabel();
            InitSplCriticalPage();
        } 

        /// <summary>
        /// Load config datas etc...
        /// </summary>
        private void LoadDatas()
        {
            if(Directory.Exists(DS.PATH.DIC_EXPORT_DATAS))
                Directory.CreateDirectory(DS.PATH.DIC_EXPORT_DATAS);

            DM.Config.LoadData();
        }

        private void InitLORPathLabel()
        {
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
            }
            #endregion

        }

        private void InitSplCriticalPage()
        {
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
            SplCriticalPage.Children.Add(new UC.EditCriticalPage());
        }
        #endregion
        #region Click events
        private void BtnLORPath_Click(object sender, RoutedEventArgs e)
        {
            Tools.Dialog.SelectDirectory((string selectedDir) =>
            {
                DM.Config.config.LORFolderPath = selectedDir;
                DM.Config.SaveData();
                InitLORPathLabel();
            });
        } 
        #endregion
    }
}

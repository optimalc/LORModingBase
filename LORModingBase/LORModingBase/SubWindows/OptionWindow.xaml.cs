using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// OptionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionWindow : Window
    {
        Action initResourceFunc = null;

        #region Init controls
        public OptionWindow(Action initResourceFunc)
        {
            InitializeComponent();
            this.initResourceFunc = initResourceFunc;

            InitSettingUIs();
        } 

        private void InitSettingUIs()
        {
            TbxLORPath.Text = DM.Config.config.LORFolderPath;
            TbxLORPath.ToolTip = $"{DS.LongDescription.LORPathHelp}\n현재 설정된 경로 : {DM.Config.config.LORFolderPath}";

            if (!Directory.Exists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}"))
            {
                LblBaseModeResource.Content = "X";
                LblBaseModeResource.ToolTip = "모드 리소스가 없습니다. (기반 모드를 적용시키고 라오루를 한번 실행시켜주세요)";
            }
            else
            {
                LblBaseModeResource.Content = "O";
                LblBaseModeResource.ToolTip = "리소스가 정상적으로 발견되었습니다";
            }
        }
        #endregion

        #region Button events
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void TbxLORPath_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Tools.Dialog.SelectDirectory((string selectedDir) =>
            {
                try
                {
                    DM.Config.config.LORFolderPath = selectedDir;
                    DM.Config.SaveData();
                    InitSettingUIs();

                    initResourceFunc();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "변경된 경로를 반영하는 과정에서 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}

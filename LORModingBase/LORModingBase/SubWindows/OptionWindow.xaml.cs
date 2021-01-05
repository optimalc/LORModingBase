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
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.OPTION);

            InitSettingUIs();
        } 

        private void InitSettingUIs()
        {
            TbxLORPath.Text = DM.Config.config.LORFolderPath;
            TbxLORPath.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"%TbxLORPath_ToolTip%")}{DM.Config.config.LORFolderPath}";

            if (!Directory.Exists($"{DM.Config.config.LORFolderPath}\\{DS.PATH.RELATIVE_DIC_LOR_MODE_RESOURCES_STATIC_INFO}"))
            {
                LblBaseModeResource.Content = "X";
                LblBaseModeResource.ToolTip = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"LblBaseModeResourceDes_Error");
            }
            else
            {
                LblBaseModeResource.Content = "O";
                LblBaseModeResource.ToolTip = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"LblBaseModeResourceDes_Info");
            }

            CbxDirectBaseModeExport.IsChecked = DM.Config.config.isDirectBaseModeExport;
            CbxExecuteAfterExport.IsChecked = DM.Config.config.isExecuteAfterExport;

            TbxProgramLanguage.Text = DM.LocalizeCore.GetLocalizeOption()[DM.Config.config.localizeOption];
        }
        #endregion

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OptionWindowTextBoxLeftButtonDownEvents(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBox clickTextBox = sender as TextBox;
            try
            {
                switch (clickTextBox.Name)
                {
                    case "TbxLORPath":
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
                                Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"OptionWindowTextBoxLeftButtonDownEvents_Error"));
                            }
                        });
                        break;
                    case "TbxProgramLanguage":
                        new SubWindows.InputLanguageWindow((string languageName) =>
                        {
                            try
                            {
                                DM.Config.config.localizeOption = DM.LocalizeCore.GetLocalizeOptionRev()[languageName];
                                DM.Config.SaveData();

                                System.Windows.Forms.Application.Restart();
                                System.Windows.Application.Current.Shutdown();
                            }
                            catch (Exception ex)
                            {
                                Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"OptionWindowTextBoxLeftButtonDownEvents_Error")); ;
                            }
                        }).ShowDialog();
                        break;

                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "Error");
            }
        }

        private void OptionWindowClickCheckboxEvents(object sender, RoutedEventArgs e)
        {
            CheckBox clickCheckBox = sender as CheckBox;
            try
            {
                switch (clickCheckBox.Name)
                {
                    case "CbxDirectBaseModeExport":
                        DM.Config.config.isDirectBaseModeExport = (bool)CbxDirectBaseModeExport.IsChecked;
                        InitSettingUIs();
                        break;
                    case "LblExecuteAfterExport":
                        DM.Config.config.isExecuteAfterExport = (bool)CbxExecuteAfterExport.IsChecked;
                        InitSettingUIs();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "Error");
            }
        }
    }
}

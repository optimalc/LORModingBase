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
using System.Windows.Shapes;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// ExtraToolsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExtraToolsWindow : Window
    {
        #region Initilize controls
        public ExtraToolsWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.ETC);
            UpdateUI();
        }

        private void UpdateUI()
        {
            CbxIsLogPlusMod.IsChecked = DM.Config.config.isLogPlusMod;
        }
        #endregion


        #region Button click events
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnExecuteDebugRedirect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(DS.PATH.RESOURCE_UNITY_ENGINE_DEBUG))
                {
                    File.Delete($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\Managed\\UnityEngine.dll");

                    File.Copy(DS.PATH.RESOURCE_UNITY_ENGINE_DEBUG, $"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\Managed\\UnityEngine.dll");
                    Tools.MessageBoxTools.ShowInfoMessageBox(
                        DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ETC, $"BtnExecuteDebugRedirect_Click_Info_1"),
                        DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ETC, $"BtnExecuteDebugRedirect_Click_Info_2"));

                    File.Create($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt");
                    File.Create($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt");

                    if (Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods"))
                        Tools.ProcessTools.OpenExplorer($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods");
                }
                else
                    new Exception(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ETC, $"BtnExecuteDebugRedirect_Click_Error_1"));
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ETC, $"BtnExecuteDebugRedirect_Click_Error_2"), 
                    ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ETC, $"BtnExecuteDebugRedirect_Click_Error_3"));
            }
        } 
        #endregion

        private void CbxIsLogPlusMod_Click(object sender, RoutedEventArgs e)
        {
            DM.Config.config.isLogPlusMod = (bool)CbxIsLogPlusMod.IsChecked;
            DM.Config.SaveData();

            UpdateUI();
        }
    }
}

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
        public ExtraToolsWindow()
        {
            InitializeComponent();
        }

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
                    MessageBox.Show($"정상적으로 적용되었습니다. \\Library Of Ruina\\LibraryOfRuina_Data\\BaseMods에 로그 메세지가 텍스트 파일로 출력됩니다", "적용 완료", MessageBoxButton.OK, MessageBoxImage.Information);

                    File.Create($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt");
                    File.Create($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt");

                    if (Directory.Exists($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods"))
                        Tools.ProcessTools.OpenExplorer($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods");
                }
                else
                    new Exception("툴 관련 리소스가 없습니다. 공식 홈페이지에서 관련 데이터를 다운받으세요");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"툴 적용 과정에서 오류가 발생했습니다 : {ex.Message}", "툴 적용 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

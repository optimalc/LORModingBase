using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// ExtraLogWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExtraLogWindow : Window
    {
        bool threadContinue = true;

        public ExtraLogWindow()
        {
            InitializeComponent();

            Tools.ThreadTools.MakeSTAThreadAndStart(() =>
            {
                while(threadContinue)
                {
                    try
                    {
                        string INFO_LOG = File.ReadAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugLogMessage.txt");
                        Tools.ThreadTools.ExecuteSafeUIUpdate(() =>
                        {
                            TbxLogInfo.Text = INFO_LOG;
                        });
                    }
                    catch
                    {

                    }

                    try
                    {
                        string ERROR_LOG = File.ReadAllText($"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods\\debugErrorMessage.txt");
                        Tools.ThreadTools.ExecuteSafeUIUpdate(() =>
                        {
                            TbxLogError.Text = ERROR_LOG;
                        });
                    }
                    catch
                    {

                    }

                    Thread.Sleep(3000);
                }
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            threadContinue = false;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

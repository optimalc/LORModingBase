using System;
using System.Collections.Generic;
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

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// DLLEditorMainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DLLEditorMainWindow : Window
    {
        public DLLEditorMainWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);
            InitDLLStacks();
        }

        private void InitDLLStacks()
        {

        }

        private void DLLEditorButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnSetDLLWorkingSpace":
                        break;
                    case "BtnCompileDLL":
                        break;

                    case "BtnAddCodeBase":
                        break;

                    case "BtnClose":
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                    DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"DLLEditorMainWindow_Error_1"));
            }
        }
    }
}

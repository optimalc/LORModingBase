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

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// DLLEditorMainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DLLEditorMainWindow : Window
    {
        public static string targetSourceFilePath = "";
        public static List<CodeBlock> rootCodeBlocks = new List<CodeBlock>();

        public DLLEditorMainWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);
            InitDLLStacks();
        }

        private void InitDLLStacks()
        {
            if(string.IsNullOrEmpty(DLLEditor.DLLEditorMainWindow.targetSourceFilePath))
            {
                Tools.JsonFile.SaveJsonFile<List<CodeBlock>>(DLLEditor.DLLEditorMainWindow.targetSourceFilePath, rootCodeBlocks);
            }
        }

        private void DLLEditorButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnSetDLLWorkingSpace":
                        new SubWindows.Global_ListSeleteWithEditWindow(null, null, null, null,
                             SubWindows.Global_ListSeleteWithEditWindow_PRESET.DLL_WORKING_SPACE).ShowDialog();
                        if (!string.IsNullOrEmpty(targetSourceFilePath))
                            this.Title = $"DLL Editor Window - {targetSourceFilePath.Split('\\').Last().Split('.')[0]}.dll";
                        InitDLLStacks();
                        break;
                    case "BtnCompileDLL":
                        if (string.IsNullOrEmpty(targetSourceFilePath))
                        {
                            DLLEditorButtonClickEvents(BtnSetDLLWorkingSpace, null);
                            return;
                        }
                        break;

                    case "BtnAddCodeBase":
                        if (string.IsNullOrEmpty(targetSourceFilePath))
                        {
                            DLLEditorButtonClickEvents(BtnSetDLLWorkingSpace, null);
                            return;
                        }

                        new DLLEditorGlobalSearchWindow((CodeBlock selectedCodeBlock) =>
                        {
                            rootCodeBlocks.Add(selectedCodeBlock);
                            InitDLLStacks();
                        }, DLLEditorGlobalSearchWindow_PRESET.BASE_BLOCK).ShowDialog();
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

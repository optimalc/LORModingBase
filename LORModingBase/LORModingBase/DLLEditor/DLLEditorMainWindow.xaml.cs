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
using LORModingBase.CustomExtensions;

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// DLLEditorMainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DLLEditorMainWindow : Window
    {
        public static string targetSourceFilePath = "";
        public static List<CodeBlock> rootCodeBlocks = new List<CodeBlock>();

        #region Init controls
        public DLLEditorMainWindow()
        {
            InitializeComponent();
            TbxNameSpace.Text = DM.Config.config.nameSpaceToUse;
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);
            InitDLLStacks();
        }

        private void InitDLLStacks()
        {
            SqlCodeBlocks.Children.Clear();
            rootCodeBlocks.ForEach((CodeBlock codeBlock) =>
            {
                LooplyInitDLLStacks(codeBlock, null);
            });
        }

        private void LooplyInitDLLStacks(CodeBlock codeBlock, CodeBlock perentBlock)
        {
            SqlCodeBlocks.Children.Add(
                    new CodeBlockControls.GlobalCodeBlockControl(codeBlock, InitCreatedSourceCodeTextBox, InitDLLStacks, perentBlock));
            codeBlock.subCodeBlocks.ForEach((CodeBlock subCodeBlock) =>
            {
                LooplyInitDLLStacks(subCodeBlock, codeBlock);
            });
        }

        private void InitCreatedSourceCodeTextBox()
        {
            if (!string.IsNullOrEmpty(DLLEditor.DLLEditorMainWindow.targetSourceFilePath))
            {
                Tools.JsonFile.SaveJsonFile<List<CodeBlock>>(DLLEditor.DLLEditorMainWindow.targetSourceFilePath, rootCodeBlocks);

                TbxTextEditor.Text = "";
                #region Make using code
                List<string> usingList = new List<string>();
                rootCodeBlocks.ForEach((CodeBlock rootCodeBlock) =>
                {
                    GetAllUsingCases(rootCodeBlock, usingList);
                });

                HashSet<string> UNIQUE_USING_HASH_SET = new HashSet<string>(usingList);
                foreach (string UNIQUE_USING_NAME in UNIQUE_USING_HASH_SET)
                    TbxTextEditor.Text += $"using {UNIQUE_USING_NAME};\n";
                #endregion
                TbxTextEditor.Text += $"\nnamespace {DM.Config.config.nameSpaceToUse}\n{{\n}}";

                rootCodeBlocks.ForEach((CodeBlock rootCodeBlock) =>
                {
                    MakeAllCodeBlockStructure(rootCodeBlock, 0, TbxTextEditor.Text.Count(f => f == '}')-1);
                });
            }
        } 

        private void MakeAllCodeBlockStructure(CodeBlock codeBlockToUse, int innerPara, int endPara)
        {
            int INDEX_TO_INPUT = TbxTextEditor.Text.IndexOfNth("}", endPara);

            string CODE_TO_USE = codeBlockToUse.codes;
            for (int paraIndex = 0; paraIndex < codeBlockToUse.inputtedParameterList.Count; paraIndex++)
                CODE_TO_USE = CODE_TO_USE.Replace("{{" + paraIndex.ToString() + "}}", codeBlockToUse.inputtedParameterList[paraIndex]);

            TbxTextEditor.Text = TbxTextEditor.Text.Insert(INDEX_TO_INPUT - innerPara - 1, "\n" + "\t".Multiple(innerPara + 1) + CODE_TO_USE.Replace("\n", "\n"+"\t".Multiple(innerPara + 1)));
            codeBlockToUse.subCodeBlocks.ForEach((CodeBlock codeBlock) =>
            {
                MakeAllCodeBlockStructure(codeBlock, innerPara + 1, endPara);
            });
        }

        private void GetAllUsingCases(CodeBlock codeBlockToUse, List<string> usingCaseList)
        {
            usingCaseList.AddRange(codeBlockToUse.usings);
            codeBlockToUse.subCodeBlocks.ForEach((CodeBlock codeBlock) =>
            {
                usingCaseList.AddRange(codeBlock.usings);
                GetAllUsingCases(codeBlock, usingCaseList);
            });
        }
        #endregion

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
                        InitCreatedSourceCodeTextBox();
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
                            InitCreatedSourceCodeTextBox();
                            InitDLLStacks();
                        }, null).ShowDialog();
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

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxNameSpace":
                    DM.Config.config.nameSpaceToUse = TbxNameSpace.Text;
                    DM.Config.SaveData();
                    InitCreatedSourceCodeTextBox();
                    break;
            }
        }
    }
}

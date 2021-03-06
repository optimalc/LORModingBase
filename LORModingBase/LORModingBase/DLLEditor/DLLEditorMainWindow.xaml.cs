﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            UpdateCompileButton();
        }

        public DLLEditorMainWindow(string fileName, List<string> autoParaPaths = null, List<CodeBlock> rootCodeBlocksToUse = null)
        {
            InitializeComponent();
            TbxNameSpace.Text = DM.Config.config.nameSpaceToUse;
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);

            string DLL_DIR_SEARCH_PATH = $"{DM.Config.CurrentWorkingDirectory}\\{DLLEditor.GlobalDatas.DLL_SOURCE_FILE_DIR_NAME}";
            if (!Directory.Exists(DLL_DIR_SEARCH_PATH))
                Directory.CreateDirectory(DLL_DIR_SEARCH_PATH);

            string JSON_FILE_PATH = $"{DLL_DIR_SEARCH_PATH}\\{fileName}.json";
            if (!File.Exists(JSON_FILE_PATH))
            {
                File.Create(JSON_FILE_PATH);
                if (rootCodeBlocks != null)
                    rootCodeBlocks = rootCodeBlocksToUse;
                if (autoParaPaths != null)
                    rootCodeBlocks = CodeBlockDataManagement.MakeCodeBlockListWithParameters(autoParaPaths);
            }
            else
            {
                rootCodeBlocks = Tools.JsonFile.LoadJsonFile<List<DLLEditor.CodeBlock>>(JSON_FILE_PATH);
                if (rootCodeBlocks == null)
                    rootCodeBlocks = new List<DLLEditor.CodeBlock>();
            }
            targetSourceFilePath = JSON_FILE_PATH;

            InitCreatedSourceCodeTextBox();
            InitDLLStacks();
            UpdateCompileButton();
        }

        private void UpdateCompileButton()
        {
            if(!string.IsNullOrEmpty(targetSourceFilePath))
            {
                string OUTPUT_DLL_PATH = $"{DM.Config.CurrentWorkingDirectory}\\{targetSourceFilePath.Split('\\').Last().Split('.')[0]}.dll";
                if (File.Exists(OUTPUT_DLL_PATH))
                {
                    BtnCompileDLL.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"%BtnCompileDLL_ToolTip%").Split('$')[0]}({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"YES_COMPILE")})";
                    BtnCompileDLL.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FF2FBD40");
                }
                else
                {
                    BtnCompileDLL.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"%BtnCompileDLL_ToolTip%").Split('$')[0]}({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"NO_COMPILE")})";
                    BtnCompileDLL.Foreground = Tools.ColorTools.GetSolidColorBrushByHexStr("#FFE74438");
                }
            }
        }

        private void InitDLLStacks()
        {
            SqlCodeBlocks.Children.Clear();
            maxDeepth = 0;
            rootCodeBlocks.ForEach((CodeBlock codeBlock) =>
            {
                LooplyInitDLLStacks(codeBlock, null);
            });

            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(
                    new TranslateTransform(-maxDeepth * 28, 0)
            );
            foreach (UIElement uiElement in SqlCodeBlocks.Children)
            {
                uiElement.RenderTransform = myTransformGroup;
            }
        }

        public static int maxDeepth = 0;
        private void LooplyInitDLLStacks(CodeBlock codeBlock, CodeBlock perentBlock, int deepth = 0)
        {
            CodeBlockControls.GlobalCodeBlockControl ADDED_CODE_BLOCK = new CodeBlockControls.GlobalCodeBlockControl(codeBlock, InitCreatedSourceCodeTextBox, InitDLLStacks, perentBlock);
            ADDED_CODE_BLOCK.Margin = new Thickness(deepth * 50, 0, 0, 0);
            SqlCodeBlocks.Children.Add(ADDED_CODE_BLOCK);

            if (maxDeepth < deepth) maxDeepth = deepth;
            codeBlock.subCodeBlocks.ForEach((CodeBlock subCodeBlock) =>
            {
                LooplyInitDLLStacks(subCodeBlock, codeBlock, deepth + 1);
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
                usingList.AddRange(
                    Tools.JsonFile.LoadJsonFile<List<string>>(DS.PROGRAM_RESOURCE_PATHS.RESOURCE_USING_GLOBAL));

                HashSet<string> UNIQUE_USING_HASH_SET = new HashSet<string>(usingList);
                foreach (string UNIQUE_USING_NAME in UNIQUE_USING_HASH_SET)
                    TbxTextEditor.Text += $"using {UNIQUE_USING_NAME};\n";
                #endregion

                GlobalVarValue = 1;
                TbxTextEditor.Text += $"\nnamespace {DM.Config.config.nameSpaceToUse}\n{{";
                rootCodeBlocks.ForEach((CodeBlock rootCodeBlock) =>
                {
                    TbxTextEditor.Text += $"\n{MakeAllCodeBlockStructure(rootCodeBlock)}\n";
                });
                TbxTextEditor.Text += $"}}";
            }
        }

        private int GlobalVarValue = 1;
        private string MakeAllCodeBlockStructure(CodeBlock codeBlockToUse)
        {
            string CODE_TO_USE = codeBlockToUse.codes;
            for (int paraIndex = 0; paraIndex < codeBlockToUse.inputtedParameterList.Count; paraIndex++)
                CODE_TO_USE = CODE_TO_USE.Replace("{{" + paraIndex.ToString() + "}}", codeBlockToUse.inputtedParameterList[paraIndex]);
            
            for (int varIndex = 0; varIndex < 10; varIndex++ )
            {
                if (CODE_TO_USE.Contains($"<<{varIndex}>>"))
                {
                    CODE_TO_USE = CODE_TO_USE.Replace($"<<{varIndex}>>", $"var{GlobalVarValue}");
                    GlobalVarValue++;
                }
                else
                    break;
            }
            CODE_TO_USE = "\t" + CODE_TO_USE.Replace("\n", "\n\t");


            string subCodes = "";
            codeBlockToUse.subCodeBlocks.ForEach((CodeBlock subCodeBlock) =>
            {
                subCodes += MakeAllCodeBlockStructure(subCodeBlock).Replace("\n", "\n\t") + "\n\t";
            });

            int END_INDEX = CODE_TO_USE.IndexOf('}');
            if(END_INDEX != -1)
                CODE_TO_USE = CODE_TO_USE.Insert(CODE_TO_USE.IndexOf('}'), subCodes);

            return CODE_TO_USE;
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

        /// <summary>
        /// Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        UpdateCompileButton();
                        break;
                    case "BtnCompileDLL":
                        if (string.IsNullOrEmpty(targetSourceFilePath))
                        {
                            DLLEditorButtonClickEvents(BtnSetDLLWorkingSpace, null);
                            return;
                        }
                        TbxLogging.Text = CompileDLL.CompileGivenCodeToDLL(TbxTextEditor.Text, targetSourceFilePath.Split('\\').Last().Split('.')[0]);
                        TbxLogging.ScrollToEnd();
                        UpdateCompileButton();
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
                    case "BtnAddCodeBlockPreset":
                        if (string.IsNullOrEmpty(targetSourceFilePath))
                        {
                            DLLEditorButtonClickEvents(BtnSetDLLWorkingSpace, null);
                            return;
                        }
                        new DLLEditorCodeBlockPresetWindow((CodeBlockPresets presetData) =>
                        {
                            List<CodeBlock> copiedCodeBlocks = new List<CodeBlock>();
                            presetData.rootCodeBlocks.ForEach((CodeBlock codeBlockToUse) =>
                            {
                                copiedCodeBlocks.Add(codeBlockToUse.Copy());
                            });
                            rootCodeBlocks.AddRange(copiedCodeBlocks);

                            InitCreatedSourceCodeTextBox();
                            InitDLLStacks();
                        }).ShowDialog();
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

        private void SbrCodeBlockStack_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SqlCodeBlocks_ScaleTransForm == null) return;
            if(SbrCodeBlockStack.Value > 0.25)
            {
                SqlCodeBlocks_ScaleTransForm.ScaleX = SbrCodeBlockStack.Value;
                SqlCodeBlocks_ScaleTransForm.ScaleY = SbrCodeBlockStack.Value;
            }
            else
            {
                SqlCodeBlocks_ScaleTransForm.ScaleX = 0.25;
                SqlCodeBlocks_ScaleTransForm.ScaleY = 0.25;
            }
        }
    }
}

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
    /// DLLEditorGlobalSearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DLLEditorGlobalSearchWindow : Window
    {
        Action<CodeBlock> afterSelectCodeBlock = null;
        List<CodeBlock> codeBlockList = new List<CodeBlock>();

        #region Init controls
        public DLLEditorGlobalSearchWindow(Action<CodeBlock> afterSelectCodeBlock, List<CodeBlock> codeBlockList, List<string> searchTypes)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.codeBlockList = codeBlockList;
            this.afterSelectCodeBlock = afterSelectCodeBlock;
            InitLbxSearchType(searchTypes);
        }

        public DLLEditorGlobalSearchWindow(Action<CodeBlock> afterSelectCodeBlock, List<string> subNodesToShow)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectCodeBlock = afterSelectCodeBlock;
            List<string> searchTypes = new List<string>();

            codeBlockList.Clear();
            if(subNodesToShow == null)
            {
                List<string> RootCodeBlockList = Tools.JsonFile.LoadJsonFile<List<string>>(DS.PROGRAM_RESOURCE_PATHS.RESOURCE_ROOT_CODE_BLOCK_LIST);
                codeBlockList = CodeBlockDataManagement.GetMultipleBaseBlockFromTargetPathListOrTitle(RootCodeBlockList);
            }
            else
                codeBlockList = CodeBlockDataManagement.GetMultipleBaseBlockFromTargetPathListOrTitle(subNodesToShow);

            searchTypes.AddRange(CodeBlockDataManagement.GetALLLocalizedBlockType());
            searchTypes.AddRange(CodeBlockDataManagement.GetALLCodeBlockKeys());
            InitLbxSearchType(searchTypes);
        }

        private void InitLbxSearchType(List<string> searchTypes)
        {
            LbxSearchType.Items.Clear();
            LbxSearchType.Items.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"SEARCH_NO_FILTER"));
            searchTypes.ForEach((string searchType) =>
            {
                LbxSearchType.Items.Add(searchType);
            });

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitSearchItems();
            }
        }

        private void InitSearchItems()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxSourceCodeBlocks.Items.Clear();
                foreach (CodeBlock codeBlock in codeBlockList)
                {
                    string SEARCH_STR = $"{codeBlock.title.ToLower()}{codeBlock.description.ToLower()}{codeBlock.codes.ToLower()}";
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !SEARCH_STR.Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;
                    if (LbxSearchType.SelectedIndex == 0)
                        LbxSourceCodeBlocks.Items.Add(codeBlock.title);
                    else
                    {
                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            default:
                                if (SEARCH_STR.Contains(LbxSearchType.SelectedItem.ToString().ToLower()))
                                    LbxSourceCodeBlocks.Items.Add(codeBlock.title);
                                break;
                        }
                    }
                }
                if (LbxSourceCodeBlocks.Items.Count > 0)
                    LbxSourceCodeBlocks.SelectedIndex = 0;
            }
        }
        #endregion

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxSourceCodeBlocks.SelectedIndex != -1)
            {
                CodeBlock SELECTED_CODE_BLOCK = CodeBlockDataManagement.GetBaseBlockFromTargetPathOrTitle(LbxSourceCodeBlocks.SelectedItem.ToString());
                afterSelectCodeBlock(SELECTED_CODE_BLOCK.Copy());
                this.Close();
            }
        }
        #region Search help methodes
        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitSearchItems();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitSearchItems();
        }
        #endregion

        private void LbxSourceCodeBlocks_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(LbxSourceCodeBlocks.SelectedIndex != -1)
            {
                CodeBlock SELECTED_CODE_BLOCK = CodeBlockDataManagement.GetBaseBlockFromTargetPathOrTitle(LbxSourceCodeBlocks.SelectedItem.ToString());
                TbxSourceCodeDes.Text = SELECTED_CODE_BLOCK.description;
                TbxSourceCodeDetail.Text = SELECTED_CODE_BLOCK.codes;
            }
        }
    }
}

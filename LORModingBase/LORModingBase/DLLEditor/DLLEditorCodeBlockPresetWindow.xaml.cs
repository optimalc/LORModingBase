using LORModingBase.CustomExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// DLLEditorCodeBlockPresetWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DLLEditorCodeBlockPresetWindow : Window
    {
        Action<CodeBlockPresets> afterSelectCodeBlockPreset = null;
        List<CodeBlockPresets> searchedCodeBlockPresets = new List<CodeBlockPresets>();

        #region Constructor and preset
        public DLLEditorCodeBlockPresetWindow(Action<CodeBlockPresets> afterSelectCodeBlockPreset)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectCodeBlockPreset = afterSelectCodeBlockPreset;
            InitLbxSearchType(new List<string>());
        }
        #endregion

        #region Init controls
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
                LbxItems.Items.Clear();
                searchedCodeBlockPresets.Clear();
                foreach (CodeBlockPresets codeBlockPresets in CodeBlockPresetDataManagerment.loadedCodeBlockPresets)
                {
                    string SEARCH_STR = $"{codeBlockPresets.title} - {codeBlockPresets.description}";
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !SEARCH_STR.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;
                    if(LbxSearchType.SelectedIndex == 0)
                    {
                        searchedCodeBlockPresets.Add(codeBlockPresets);
                        LbxItems.Items.Add(SEARCH_STR);
                    }
                    else
                    {
                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            default:
                                if (SEARCH_STR.ToLower().Contains(LbxSearchType.SelectedItem.ToString().ToLower()))
                                {
                                    searchedCodeBlockPresets.Add(codeBlockPresets);
                                    LbxItems.Items.Add(SEARCH_STR);
                                }
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedIndex != -1)
            {
                afterSelectCodeBlockPreset(searchedCodeBlockPresets[LbxItems.SelectedIndex]);
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
    }
}
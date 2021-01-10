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
        Action<string> afterSelectItem = null;
        List<string> selectItems = null;

        #region Init controls
        public DLLEditorGlobalSearchWindow(Action<string> afterSelectItem, List<string> searchTypes, List<string> selectItems)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectItem = afterSelectItem;
            this.selectItems = selectItems;
            InitLbxSearchType(searchTypes);
        }

        public DLLEditorGlobalSearchWindow(Action<string> afterSelectItem, DLLEditorGlobalSearchWindow_PRESET preset)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectItem = afterSelectItem;
            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (preset)
            {
                case DLLEditorGlobalSearchWindow_PRESET.BASE_BLOCK:
                    break;
            }

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
                foreach (string selectItem in selectItems)
                {
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !selectItem.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;
                    if (LbxSearchType.SelectedIndex == 0)
                        LbxSourceCodeBlocks.Items.Add(selectItem);
                    else
                    {
                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            default:
                                if (selectItem.ToLower().Contains(LbxSearchType.SelectedItem.ToString().ToLower()))
                                    LbxSourceCodeBlocks.Items.Add(selectItem);
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxSourceCodeBlocks.SelectedItem != null)
            {
                afterSelectItem(LbxSourceCodeBlocks.SelectedItem.ToString().Split(':').Last());
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

        }
    }

    public enum DLLEditorGlobalSearchWindow_PRESET
    {
        BASE_BLOCK
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_ListSeleteWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_ListSeleteWindow : Window
    {
        Action<string> afterSelect = null;

        public Global_ListSeleteWindow(Action<string> afterSelect, List<string> itemToLoad, string windowTitle = "항목 선택 메뉴")
        {
            InitializeComponent();
            this.afterSelect = afterSelect;
            this.Title = windowTitle;

            itemToLoad.ForEach((string item) =>
            {
                LbxItems.Items.Add(item);
            });
        }

        public Global_ListSeleteWindow(Action<string> afterSelect, Global_ListSeleteWindow_PRESET PRESET)
        {
            InitializeComponent();
            this.afterSelect = afterSelect;
            List<string> itemToLoad = new List<string>();
            switch (PRESET)
            {
                case Global_ListSeleteWindow_PRESET.LANGUAGES:
                    this.afterSelect = (string languageName) =>
                    {
                        try
                        {
                            DM.Config.config.localizeOption = DM.LocalizeCore.GetLocalizeOptionRev()[languageName];
                            DM.Config.SaveData();

                            System.Windows.Forms.Application.Restart();
                            System.Windows.Application.Current.Shutdown();
                        }
                        catch (Exception ex)
                        {
                            Tools.MessageBoxTools.ShowErrorMessageBox(ex, DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"OptionWindowTextBoxLeftButtonDownEvents_Error")); ;
                        }
                    };
                    itemToLoad = DM.LocalizeCore.GetLocalizeOptionRev().Keys.ToList();
                    this.Title = "Double click language you want";
                    break;
            }
            itemToLoad.ForEach((string item) =>
            {
                LbxItems.Items.Add(item);
            });
        }

        private void LbxItems_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterSelect(LbxItems.SelectedItem.ToString());
                this.Close();
            }
        }
    }

    public enum Global_ListSeleteWindow_PRESET
    {
        LANGUAGES
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputLanguageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputLanguageWindow : Window
    {
        Action<string> afterSelect = null;

        public InputLanguageWindow(Action<string> afterSelect)
        {
            InitializeComponent();
            this.afterSelect = afterSelect;

            DM.LocalizeCore.GetLocalizeOptionRev().Keys.ToList().ForEach((string localizeOption) =>
            {
                LbxLanguages.Items.Add(localizeOption);
            });
        }

        private void LbxLanguages_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (LbxLanguages.SelectedItem != null)
            {
                afterSelect(LbxLanguages.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}

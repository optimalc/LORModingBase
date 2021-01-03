using System;
using System.Windows;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputCriticalBookDescription.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputCriticalBookDescription : Window
    {
        Action<string> afterDesInputed = null;

        public InputCriticalBookDescription(Action<string> afterDesInputed, string des = "")
        {
            InitializeComponent();
            this.afterDesInputed = afterDesInputed;
            if(des != DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, "noDescription"))
                TbxDescription.Text = des;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TbxDescription.Text))
            {
                MessageBox.Show("핵심 책장에 대한 설명을 입력하세요", "미입력", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            afterDesInputed(TbxDescription.Text);
            this.Close();
        }
    }
}

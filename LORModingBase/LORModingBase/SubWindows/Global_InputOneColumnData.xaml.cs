using System;
using System.Windows;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_InputOneColumnData.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_InputOneColumnData : Window
    {
        Action<string> afterDataInputed = null;

        public Global_InputOneColumnData(Action<string> afterDataInputed, string prevData = "",
            string windowTitle="항목 입력", string tbxToolTip="항목을 입력합니다")
        {
            InitializeComponent();
            TbxData.Text = prevData;
            this.afterDataInputed = afterDataInputed;

            this.Title = windowTitle;
            TbxData.ToolTip = tbxToolTip;
        }

        private void TbxData_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            afterDataInputed?.Invoke(TbxData.Text);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

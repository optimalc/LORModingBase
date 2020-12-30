using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputBookIconWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputBookIconWindow : Window
    {
        Action<string> afterSelectBookIcon = null;

        #region Init controls
        public InputBookIconWindow(Action<string> afterSelectBookIcon)
        {
            InitializeComponent();
            InitLbxFile();
            this.afterSelectBookIcon = afterSelectBookIcon;
        }

        private void InitLbxFile()
        {
            LbxFile.Items.Clear();
            DM.StaticInfos.bookIconInfos.Keys.ToList().ForEach((string fileName) =>
            {
                LbxFile.Items.Add(fileName);
            });

            if (LbxFile.Items.Count > 0)
            {
                LbxFile.SelectedIndex = 0;
                InitLbxBookIcon();
            }
        }

        private void InitLbxBookIcon()
        {
            if (LbxFile.SelectedItem != null)
            {
                LbxBookIcon.Items.Clear();
                DM.StaticInfos.bookIconInfos[LbxFile.SelectedItem.ToString()].ForEach((string bookIconName) =>
                {
                    LbxBookIcon.Items.Add(bookIconName);
                });
            }
        }
        #endregion

        private void LbxBookIcon_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxBookIcon.SelectedIndex != -1)
            {
                afterSelectBookIcon(LbxBookIcon.SelectedItem.ToString());
                this.Close();
            }
        }

        private void LbxFile_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxBookIcon();
        }
    }
}
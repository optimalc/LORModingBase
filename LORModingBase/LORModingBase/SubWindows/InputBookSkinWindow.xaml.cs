using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputBookSkinWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputBookSkinWindow : Window
    {
        Action<string> afterSelectBookskin = null;

        #region Init controls
        public InputBookSkinWindow(Action<string> afterSelectBookskin)
        {
            InitializeComponent();
            InitLbxFile();
            this.afterSelectBookskin = afterSelectBookskin;
        }

        private void InitLbxFile()
        {
            LbxFile.Items.Clear();
            DM.StaticInfos.skinInfos.Keys.ToList().ForEach((string fileName) =>
            {
                LbxFile.Items.Add(fileName);
            });

            if (LbxFile.Items.Count > 0)
            {
                LbxFile.SelectedIndex = 0;
                InitLbxBookSkin();
            }
        }

        private void InitLbxBookSkin()
        {
            if (LbxFile.SelectedItem != null)
            {
                LbxBookSkin.Items.Clear();
                DM.StaticInfos.skinInfos[LbxFile.SelectedItem.ToString()].ForEach((string bookSkinName) =>
                {
                    LbxBookSkin.Items.Add(bookSkinName);
                });
            }
        }
        #endregion

        private void LbxFile_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxBookSkin();
        }

        private void LbxBookSkin_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxBookSkin.SelectedIndex != -1)
            {
                afterSelectBookskin(LbxBookSkin.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}
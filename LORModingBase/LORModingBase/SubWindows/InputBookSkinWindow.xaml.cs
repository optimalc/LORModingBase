using System;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputBookSkinWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputBookSkinWindow : Window
    {
        /// <summary>
        /// After select atage (chapter, skinName, skinDesc)
        /// </summary>
        Action<string, string, string> afterSelectSkin = null;

        #region Init controls
        public InputBookSkinWindow(Action<string, string, string> afterSelectSkin)
        {
            InitializeComponent();
            InitListBoxes();

            this.afterSelectSkin = afterSelectSkin;
        } 

        private void InitListBoxes()
        {
            DM.GameInfos.bookSkinInfos.ForEach((DS.BookSkinInfo skinInfo) =>
            {
                switch(skinInfo.chapter)
                {
                    case "1":
                        LbxCh1.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "2":
                        LbxCh2.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "3":
                        LbxCh3.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "4":
                        LbxCh4.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "5":
                        LbxCh5.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "6":
                        LbxCh6.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    case "7":
                        LbxCh7.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                    default:
                        LbxEtc.Items.Add($"{skinInfo.skinDesc}:{skinInfo.skinName}");
                        break;
                }
            });
        }
        #endregion

        #region Episode select event
        private void LbxEtc_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxEtc.SelectedItem != null)
            {
                afterSelectSkin("ETC", LbxEtc.SelectedItem.ToString().Split(':')[1], LbxEtc.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LbxCh1.SelectedItem != null)
            {
                afterSelectSkin("1", LbxCh1.SelectedItem.ToString().Split(':')[1], LbxCh1.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh2_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh2.SelectedItem != null)
            {
                afterSelectSkin("2", LbxCh2.SelectedItem.ToString().Split(':')[1], LbxCh2.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh3_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh3.SelectedItem != null)
            {
                afterSelectSkin("3", LbxCh3.SelectedItem.ToString().Split(':')[1], LbxCh3.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh4.SelectedItem != null)
            {
                afterSelectSkin("4", LbxCh4.SelectedItem.ToString().Split(':')[1], LbxCh4.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh5_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh5.SelectedItem != null)
            {
                afterSelectSkin("5", LbxCh5.SelectedItem.ToString().Split(':')[1], LbxCh5.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh6_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh6.SelectedItem != null)
            {
                afterSelectSkin("6", LbxCh6.SelectedItem.ToString().Split(':')[1], LbxCh6.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh7_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh7.SelectedItem != null)
            {
                afterSelectSkin("7", LbxCh7.SelectedItem.ToString().Split(':')[1], LbxCh7.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }
        #endregion

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDirectInput_Click(object sender, RoutedEventArgs e)
        {
            new InputEpisodeDirectlyWindow((string skinCode, string skinDes) =>
            {
                afterSelectSkin("ETC", skinCode, skinDes);
                Close();
            }, upsideLabelInfo: "스킨 이름 >").ShowDialog();
        }
    }
}

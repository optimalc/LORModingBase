using System;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputBookIconWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputBookIconWindow : Window
    {
        /// <summary>
        /// After select atage (chapter, iconName, iconDesc)
        /// </summary>
        Action<string, string, string> afterSelectIcon = null;

        #region Init controls
        public InputBookIconWindow(Action<string, string, string> afterSelectIcon)
        {
            InitializeComponent();
            InitListBoxes();

            this.afterSelectIcon = afterSelectIcon;
        } 

        private void InitListBoxes()
        {
            DM.StaticInfos.bookIconInfos.ForEach((DS.BookIconInfo iconInfo) =>
            {
                switch(iconInfo.chapter)
                {
                    case "1":
                        LbxCh1.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "2":
                        LbxCh2.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "3":
                        LbxCh3.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "4":
                        LbxCh4.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "5":
                        LbxCh5.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "6":
                        LbxCh6.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                    case "7":
                        LbxCh7.Items.Add($"{iconInfo.iconDesc}:{iconInfo.iconName}");
                        break;
                }
            });
        }
        #endregion

        #region Episode select event
        private void LbxCh1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LbxCh1.SelectedItem != null)
            {
                afterSelectIcon("1", LbxCh1.SelectedItem.ToString().Split(':')[1], LbxCh1.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }
        private void Label_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh2.SelectedItem != null)
            {
                afterSelectIcon("2", LbxCh1.SelectedItem.ToString().Split(':')[1], LbxCh1.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh2_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh2.SelectedItem != null)
            {
                afterSelectIcon("2", LbxCh2.SelectedItem.ToString().Split(':')[1], LbxCh2.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh3_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh3.SelectedItem != null)
            {
                afterSelectIcon("3", LbxCh3.SelectedItem.ToString().Split(':')[1], LbxCh3.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh4.SelectedItem != null)
            {
                afterSelectIcon("4", LbxCh4.SelectedItem.ToString().Split(':')[1], LbxCh4.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh5_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh5.SelectedItem != null)
            {
                afterSelectIcon("5", LbxCh5.SelectedItem.ToString().Split(':')[1], LbxCh5.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh6_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh6.SelectedItem != null)
            {
                afterSelectIcon("6", LbxCh6.SelectedItem.ToString().Split(':')[1], LbxCh6.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh7_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh7.SelectedItem != null)
            {
                afterSelectIcon("7", LbxCh7.SelectedItem.ToString().Split(':')[1], LbxCh7.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }
        #endregion
    }
}

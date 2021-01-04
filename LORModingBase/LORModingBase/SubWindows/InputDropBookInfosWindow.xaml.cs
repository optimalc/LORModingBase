using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputDropBookInfosWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputDropBookInfosWindow : Window
    {
        List<string> dropbookInfos = null;

        #region Init controls
        public InputDropBookInfosWindow(List<string> dropbookInfos)
        {
            InitializeComponent();
            this.dropbookInfos = dropbookInfos;

            InitListBoxes();
            InitSelectedDropboxsList();
        } 

        private void InitSelectedDropboxsList()
        {
            LbxSelectedDropbooks.Items.Clear();
            dropbookInfos.ForEach((string dropBookInfo) =>
            {
                LbxSelectedDropbooks.Items.Add(dropBookInfo);
            });
        }

        private void InitListBoxes()
        {
            DM.GameInfos.dropBookInfos.ForEach((DS.DropBookInfo bookDropInfo) =>
            {
                switch(bookDropInfo.chapter)
                {
                    case "1":
                        LbxCh1.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "2":
                        LbxCh2.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "3":
                        LbxCh3.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "4":
                        LbxCh4.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "5":
                        LbxCh5.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "6":
                        LbxCh6.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                    case "7":
                        LbxCh7.Items.Add($"{bookDropInfo.iconDesc}:{bookDropInfo.bookID}");
                        break;
                }
            });
        }
        #endregion

        #region Episode select event
        private void LbxCh1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LbxCh1.SelectedItem != null && !dropbookInfos.Contains(LbxCh1.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh1.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh2_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh2.SelectedItem != null && !dropbookInfos.Contains(LbxCh2.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh2.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh3_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh3.SelectedItem != null && !dropbookInfos.Contains(LbxCh3.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh3.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh4.SelectedItem != null && !dropbookInfos.Contains(LbxCh4.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh4.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh5_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh5.SelectedItem != null && !dropbookInfos.Contains(LbxCh5.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh5.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh6_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh6.SelectedItem != null && !dropbookInfos.Contains(LbxCh6.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh6.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh7_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh7.SelectedItem != null && !dropbookInfos.Contains(LbxCh7.SelectedItem.ToString()))
            {
                dropbookInfos.Add(LbxCh7.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }
        #endregion

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(LbxSelectedDropbooks.SelectedItem != null)
            {
                dropbookInfos.Remove(LbxSelectedDropbooks.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }
    }
}

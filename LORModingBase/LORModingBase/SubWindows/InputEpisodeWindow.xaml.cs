using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputEpisodeWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputEpisodeWindow : Window
    {
        /// <summary>
        /// After select atage (chapter, stageID, stringDoc)
        /// </summary>
        Action<string, string, string> afterSelectEpisode = null;

        #region Init controls
        public InputEpisodeWindow(Action<string, string, string> afterSelectEpisode)
        {
            InitializeComponent();
            InitListBoxes();

            this.afterSelectEpisode = afterSelectEpisode;
        } 

        private void InitListBoxes()
        {
            DM.StaticInfos.stageInfos.ForEach((DS.StageInfo stageInfo) =>
            {
                switch(stageInfo.Chapter)
                {
                    case "1":
                        LbxCh1.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "2":
                        LbxCh2.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "3":
                        LbxCh3.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "4":
                        LbxCh4.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "5":
                        LbxCh5.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "6":
                        LbxCh6.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
                        break;
                    case "7":
                        LbxCh7.Items.Add($"{stageInfo.stageDoc}:{stageInfo.stageID}");
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
                afterSelectEpisode("1", LbxCh1.SelectedItem.ToString().Split(':')[1], LbxCh1.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh2_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh2.SelectedItem != null)
            {
                afterSelectEpisode("2", LbxCh2.SelectedItem.ToString().Split(':')[1], LbxCh2.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh3_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh3.SelectedItem != null)
            {
                afterSelectEpisode("3", LbxCh3.SelectedItem.ToString().Split(':')[1], LbxCh3.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh4.SelectedItem != null)
            {
                afterSelectEpisode("4", LbxCh4.SelectedItem.ToString().Split(':')[1], LbxCh4.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh5_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh5.SelectedItem != null)
            {
                afterSelectEpisode("5", LbxCh5.SelectedItem.ToString().Split(':')[1], LbxCh5.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh6_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh6.SelectedItem != null)
            {
                afterSelectEpisode("6", LbxCh6.SelectedItem.ToString().Split(':')[1], LbxCh6.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }

        private void LbxCh7_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh7.SelectedItem != null)
            {
                afterSelectEpisode("7", LbxCh7.SelectedItem.ToString().Split(':')[1], LbxCh7.SelectedItem.ToString().Split(':')[0]);
                this.Close();
            }
        }
        #endregion

        private void BtnDirectInput_Click(object sender, RoutedEventArgs e)
        {
            new InputEpisodeDirectlyWindow((string episodeCode, string episodeDes) =>
            {
                new InputChapterWindow((string selectedChapter) =>
                {
                    afterSelectEpisode(selectedChapter.Split(':').Last(), episodeCode, episodeDes);
                    Close();
                }).ShowDialog();
            }).ShowDialog();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

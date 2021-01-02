using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputUniqueCardsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputUniqueCardsWindow : Window
    {
        List<string> uniqueCards = null;

        #region Init controls
        public InputUniqueCardsWindow(List<string> uniqueCards)
        {
            InitializeComponent();
            this.uniqueCards = uniqueCards;

            InitListBoxes();
            InitSelectedDropboxsList();
        } 

        private void InitSelectedDropboxsList()
        {
            LbxSelectedUniqueCards.Items.Clear();
            uniqueCards.ForEach((string dropBookInfo) =>
            {
                LbxSelectedUniqueCards.Items.Add(dropBookInfo);
            });
        }

        private void InitListBoxes()
        {
            DM.StaticInfos.gameCardInfos.ForEach((DS.CardInfo cardInfo) =>
            {
                if (cardInfo.option != "OnlyPage")
                    return;

                switch (cardInfo.chapter)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4": 
                        LbxCh1_2_3_4.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                        break;

                    case "5":
                        LbxCh5.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                        break;
                    case "6":
                        LbxCh6.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                        break;
                    case "7":
                        LbxCh7.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                        break;
                    default:
                        LbxEtc.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                        break;
                }
            });

            MainWindow.cardInfos.ForEach((DS.CardInfo cardInfo) =>
            {
                if (cardInfo.option == "OnlyPage")
                {
                    if(!string.IsNullOrEmpty(cardInfo.cardID))
                        LbxEditedCard.Items.Add($"{cardInfo.name}:{cardInfo.cardID}");
                }
            });
        }
        #endregion

        #region Episode select event
        private void LbxEtc_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxEtc.SelectedItem != null && !uniqueCards.Contains(LbxEtc.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxEtc.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh1_2_3_4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh1_2_3_4.SelectedItem != null && !uniqueCards.Contains(LbxCh1_2_3_4.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxCh5.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh5_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh5.SelectedItem != null && !uniqueCards.Contains(LbxCh5.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxCh5.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh6_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh6.SelectedItem != null && !uniqueCards.Contains(LbxCh6.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxCh6.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void LbxCh7_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCh7.SelectedItem != null && !uniqueCards.Contains(LbxCh7.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxCh7.SelectedItem.ToString());
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
            if(LbxSelectedUniqueCards.SelectedItem != null)
            {
                uniqueCards.Remove(LbxSelectedUniqueCards.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }

        private void BtnDirectInput_Click(object sender, RoutedEventArgs e)
        {
            new InputEpisodeDirectlyWindow((string cardCode, string cardDes) =>
            {
                uniqueCards.Add($"{cardDes}:{cardCode}");
                InitSelectedDropboxsList();
            }, upsideLabelInfo: "고유책장 코드 >").ShowDialog();
        }

        private void LbxEditedCard_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxEditedCard.SelectedItem != null && !uniqueCards.Contains(LbxEditedCard.SelectedItem.ToString()))
            {
                uniqueCards.Add(LbxEditedCard.SelectedItem.ToString());
                InitSelectedDropboxsList();
            }
        }
    }
}

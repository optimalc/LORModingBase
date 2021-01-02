using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputExtraDataForCardWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputExtraDataForCardWindow : Window
    {
        DS.CardInfo cardInfoToUse = null;

        public InputExtraDataForCardWindow(DS.CardInfo cardInfoToUse)
        {
            InitializeComponent();
            this.cardInfoToUse = cardInfoToUse;

            TbxChapter.Text = cardInfoToUse.chapter;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            TbxChapter.Text = "1";
            cardInfoToUse.chapter = "1";
        }

        private void TbxChapter_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new InputChapterWindow((string selectedChapter) =>
            {
                string CH_TO_USE = selectedChapter.Split(':').Last();
                TbxChapter.Text = CH_TO_USE;
                cardInfoToUse.chapter = CH_TO_USE;
            }).ShowDialog();
        }
    }
}

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
            TbxPriority.Text = cardInfoToUse.priority;
            TbxSortPriority.Text = cardInfoToUse.sortPriority;
            TbxPriorityScript.Text = cardInfoToUse.priorityScript;
        }

        #region Button events
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            TbxChapter.Text = "1";
            TbxPriority.Text = "1";
            TbxSortPriority.Text = "1";
            TbxPriorityScript.Text = "";

            cardInfoToUse.chapter = "1";
            cardInfoToUse.priority = "1";
            cardInfoToUse.sortPriority = "1";
            cardInfoToUse.priorityScript = "";
        } 
        #endregion

        #region Text change events
        private void TbxChapter_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new InputChapterWindow((string selectedChapter) =>
            {
                string CH_TO_USE = selectedChapter.Split(':').Last();
                TbxChapter.Text = CH_TO_USE;
                cardInfoToUse.chapter = CH_TO_USE;
            }).ShowDialog();
        }

        private void TbxPriority_TextChanged(object sender, TextChangedEventArgs e)
        {
            cardInfoToUse.priority = TbxPriority.Text;
        }

        private void TbxSortPriority_TextChanged(object sender, TextChangedEventArgs e)
        {
            cardInfoToUse.sortPriority = TbxSortPriority.Text;
        }

        private void TbxPriorityScript_TextChanged(object sender, TextChangedEventArgs e)
        {
            cardInfoToUse.priorityScript = TbxPriorityScript.Text;
        } 
        #endregion
    }
}

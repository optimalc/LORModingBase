using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LORModingBase.UC
{
    public class DirectoryListing
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    /// <summary>
    /// EditCriticalPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCriticalPage : UserControl
    {
        #region Init controls
        public EditCriticalPage()
        {
            InitializeComponent();
            ChangeRarityUIInit("Common");
            ChangeChapterUIInit(1);
        }

        private void ChangeRarityUIInit(string rarity)
        {
            BtnRarityCommon.Background = null;
            BtnRarityUncommon.Background = null;
            BtnRarityRare.Background = null;
            BtnRarityUnique.Background = null;

            switch (rarity)
            {
                case "Common":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#5430BF4B");
                    BtnRarityCommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case "Uncommon":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54306ABF");
                    BtnRarityUncommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case "Rare":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#548030BF");
                    BtnRarityRare.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case "Unique":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54F3B530");
                    BtnRarityUnique.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
            }
        }
        
        private void ChangeChapterUIInit(int chapter)
        {
            BtnCh1.Background = null;
            BtnCh2.Background = null;
            BtnCh3.Background = null;
            BtnCh4.Background = null;
            BtnCh5.Background = null;
            BtnCh6.Background = null;
            BtnCh7.Background = null;

            switch (chapter)
            {
                case 1:
                    BtnCh1.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 2:
                    BtnCh2.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 3:
                    BtnCh3.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 4:
                    BtnCh4.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 5:
                    BtnCh5.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 6:
                    BtnCh6.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
                case 7:
                    BtnCh7.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
            }
        }
        
        private void InitResistFromButton(Button targetButton, bool isLeft)
        {
            // Down index
            if(isLeft)
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.IndexOf(targetButton.Content.ToString()) - 1;
                if (RESISTS_INDEX < 0) RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.Count - 1;
                targetButton.Content = DS.GameInfo.resistInfo_Doc[RESISTS_INDEX];
            }
            // Up index
            else
            {
                int RESISTS_INDEX = DS.GameInfo.resistInfo_Doc.IndexOf(targetButton.Content.ToString()) + 1;
                if (RESISTS_INDEX >= DS.GameInfo.resistInfo_Doc.Count) RESISTS_INDEX = 0;
                targetButton.Content = DS.GameInfo.resistInfo_Doc[RESISTS_INDEX];
            }
        }
        #endregion

        #region Button events
        #region Rarity buttons
        private void BtnRarityCommon_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Common");
        }

        private void BtnRarityUncommon_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Uncommon");
        }

        private void BtnRarityRare_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Rare");
        }

        private void BtnRarityUnique_Click(object sender, RoutedEventArgs e)
        {
            ChangeRarityUIInit("Unique");
        }
        #endregion
        #region Chapter buttons
        private void BtnCh1_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(1);
        }

        private void BtnCh2_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(2);
        }

        private void BtnCh3_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(3);
        }

        private void BtnCh4_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(4);
        }

        private void BtnCh5_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(5);
        }

        private void BtnCh6_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(6);
        }

        private void BtnCh7_Click(object sender, RoutedEventArgs e)
        {
            ChangeChapterUIInit(7);
        }
        #endregion

        #region HP resist buttons
        private void BtnSResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnSResist, true);
        }

        private void BtnSResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnSResist, false);
        }

        private void BtnPResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnPResist, true);
        }

        private void BtnPResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnPResist, false);
        }

        private void BtnHResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnHResist, true);
        }

        private void BtnHResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnHResist, false);
        }
        #endregion

        #region Break resist buttons
        private void BtnBSResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBSResist, true);
        }

        private void BtnBSResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBSResist, false);
        }


        private void BtnBPResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBPResist, true);
        }

        private void BtnBPResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBPResist, false);
        }

        private void BtnBHResist_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBHResist, true);
        }

        private void BtnBHResist_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            InitResistFromButton(BtnBHResist, false);
        }
        #endregion

        #endregion
    }
}

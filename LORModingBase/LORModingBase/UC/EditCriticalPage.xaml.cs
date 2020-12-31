using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditCriticalPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCriticalPage : UserControl
    {
        DS.CriticalPageInfo innerCriticalPageInfo = null;
        Action initStack = null;

        #region Init controls
        public EditCriticalPage(DS.CriticalPageInfo criticalPageInfo, Action initStack)
        {
            InitializeComponent();
            this.innerCriticalPageInfo = criticalPageInfo;
            this.initStack = initStack;

            ChangeRarityUIInit(criticalPageInfo.rarity);
            if(!string.IsNullOrEmpty(criticalPageInfo.episodeDes))
            {
                BtnEpisode.Content = criticalPageInfo.episodeDes;
                BtnEpisode.ToolTip = criticalPageInfo.episodeDes;
            }

            TbxPageName.Text = criticalPageInfo.name;
            TbxPageUniqueID.Text = criticalPageInfo.bookID;

            TbxHP.Text = criticalPageInfo.HP;
            TbxBR.Text = criticalPageInfo.breakNum;
            TbxSpeedDiceMin.Text = criticalPageInfo.minSpeedCount;
            TbxSpeedDiceMax.Text = criticalPageInfo.maxSpeedCount;

            if (!string.IsNullOrEmpty(criticalPageInfo.iconDes))
            {
                BtnBookIcon.Content = criticalPageInfo.iconDes;
                BtnBookIcon.ToolTip = criticalPageInfo.iconDes;
            }
            if (!string.IsNullOrEmpty(criticalPageInfo.iconDes))
            {
                BtnSkin.Content = criticalPageInfo.skinDes;
                BtnSkin.ToolTip = criticalPageInfo.skinDes;
            }

            BtnSResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.SResist];
            BtnPResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.PResist];
            BtnHResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.HResist];

            BtnBSResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.BSResist];
            BtnBPResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.BPResist];
            BtnBHResist.Content = DS.GameInfo.resistInfo_Dic[criticalPageInfo.BHResist];

            InitLbxPassives();
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
                    innerCriticalPageInfo.rarity = "Common";
                    break;
                case "Uncommon":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54306ABF");
                    BtnRarityUncommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageInfo.rarity = "Uncommon";
                    break;
                case "Rare":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#548030BF");
                    BtnRarityRare.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageInfo.rarity = "Rare";
                    break;
                case "Unique":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54F3B530");
                    BtnRarityUnique.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCriticalPageInfo.rarity = "Unique";
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

            switch (targetButton.Name)
            {
                case "BtnSResist":
                    innerCriticalPageInfo.SResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;
                case "BtnPResist":
                    innerCriticalPageInfo.PResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;
                case "BtnHResist":
                    innerCriticalPageInfo.HResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;

                case "BtnBSResist":
                    innerCriticalPageInfo.BSResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;
                case "BtnBPResist":
                    innerCriticalPageInfo.BPResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;
                case "BtnBHResist":
                    innerCriticalPageInfo.BHResist = DS.GameInfo.resistInfo_Dic_Rev[targetButton.Content.ToString()];
                    break;
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

        private void BtnEpisode_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputEpisodeWindow((string chapter, string episodeID, string episodeDoc) =>
            {
                string CONTENT_TO_SHOW = $"{DS.GameInfo.chapter_Dic[chapter]} / {episodeDoc}:{episodeID}";
                BtnEpisode.Content = CONTENT_TO_SHOW;
                BtnEpisode.ToolTip = CONTENT_TO_SHOW;

                innerCriticalPageInfo.chapter = chapter;
                innerCriticalPageInfo.episode = episodeID;
                innerCriticalPageInfo.episodeDes = CONTENT_TO_SHOW;
            }).ShowDialog();
        }
        private void BtnBookIcon_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookIconWindow((string chpater, string bookIconName, string bookIconDesc) =>
            {
                string ICON_DESC = $"{bookIconDesc}:{bookIconName}";
                BtnBookIcon.Content = ICON_DESC;
                BtnBookIcon.ToolTip = ICON_DESC;

                innerCriticalPageInfo.iconName = bookIconName;
                innerCriticalPageInfo.iconDes = ICON_DESC;
            }).ShowDialog();
        }
        private void BtnSkin_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookSkinWindow((string chpater, string bookSkinName, string bookSkinDesc) =>
            {
                string SKIN_DESC = $"{bookSkinDesc}:{bookSkinName}";
                BtnSkin.Content = SKIN_DESC;
                BtnSkin.ToolTip = SKIN_DESC;

                innerCriticalPageInfo.skinName = bookSkinName;
                innerCriticalPageInfo.skinDes = SKIN_DESC;
            }).ShowDialog();
        }

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

        #region Passive events
        private void InitLbxPassives()
        {
            LbxPassives.Items.Clear();
            innerCriticalPageInfo.passiveIDs.ForEach((string passiveName) =>
            {
                LbxPassives.Items.Add(passiveName);
            });
        }

        private void BtnAddPassive_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputBookPassiveWindow((string passiveDec) =>
            {
                innerCriticalPageInfo.passiveIDs.Add(passiveDec);
                InitLbxPassives();
            }).ShowDialog();
        }

        private void BtnDeletePassive_Click(object sender, RoutedEventArgs e)
        {
            if(LbxPassives.SelectedItem != null)
            {
                int passiveIndexToDelete = innerCriticalPageInfo.passiveIDs.FindIndex((string passiveName) => {
                    return passiveName == LbxPassives.SelectedItem.ToString();
                });
                if(passiveIndexToDelete != -1)
                {
                    innerCriticalPageInfo.passiveIDs.RemoveAt(passiveIndexToDelete);
                    InitLbxPassives();
                }
            }
        }
        #endregion
        #region Text change events
        private void TbxPageName_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.name = TbxPageName.Text;
        }

        private void TbxPageUniqueID_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.bookID = TbxPageUniqueID.Text;
        }

        private void TbxHP_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.HP = TbxHP.Text;
        }

        private void TbxBR_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.breakNum = TbxBR.Text;
        }

        private void TbxSpeedDiceMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.minSpeedCount = TbxSpeedDiceMin.Text;
        }

        private void TbxSpeedDiceMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCriticalPageInfo.maxSpeedCount = TbxSpeedDiceMax.Text;
        }
        #endregion

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.criticalPageInfos.Remove(innerCriticalPageInfo);
            initStack();
        }
    }
}

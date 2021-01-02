using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditCard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCard : UserControl
    {
        DS.CardInfo innerCardInfo = null;
        Action initStack = null;

        #region Init controls
        public EditCard(DS.CardInfo innerCardInfo, Action initStack)
        {
            this.innerCardInfo = innerCardInfo;
            this.initStack = initStack;
            InitializeComponent();

            InitSqlDices();
            ChangeRarityUIInit(innerCardInfo.rarity);
            UpdateExtrainfoIcon();
            UpdateRangeTypeUI();
            UpdateUniqueTypeUI();
            TbxCardName.Text = innerCardInfo.name;
            TbxCardUniqueID.Text = innerCardInfo.cardID;

            if(!string.IsNullOrEmpty(innerCardInfo.cardImage))
            {
                BtnCardImage.Content = innerCardInfo.cardImage;
                BtnCardImage.ToolTip = innerCardInfo.cardImage;
            }
            if (!string.IsNullOrEmpty(innerCardInfo.cardScript))
            {
                BtnCardEffect.Content = innerCardInfo.cardScript;
                BtnCardEffect.ToolTip = innerCardInfo.cardScript;
            }

            #region 드랍되는 곳 체크
            if (innerCardInfo.dropBooks.Count > 0)
            {
                string extraInfo = "";
                innerCardInfo.dropBooks.ForEach((string dropBookInfo) =>
                {
                    extraInfo += $"{dropBookInfo}\n";
                });
                extraInfo = extraInfo.TrimEnd('\n');

                BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                BtnDropCards.ToolTip = $"이 전투책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
            } 
            #endregion
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
                    innerCardInfo.rarity = "Common";
                    break;
                case "Uncommon":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54306ABF");
                    BtnRarityUncommon.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCardInfo.rarity = "Uncommon";
                    break;
                case "Rare":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#548030BF");
                    BtnRarityRare.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCardInfo.rarity = "Rare";
                    break;
                case "Unique":
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54F3B530");
                    BtnRarityUnique.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    innerCardInfo.rarity = "Unique";
                    break;
            }
        } 
        #endregion

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

        #region Right side buttons
        private void BtnExtraInfo_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputExtraDataForCardWindow(innerCardInfo).ShowDialog();
            UpdateExtrainfoIcon();
        }

        private void UpdateExtrainfoIcon()
        {
            if(!string.IsNullOrEmpty(innerCardInfo.chapter))
            {
                BtnExtraInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                BtnExtraInfo.ToolTip = "중요성이 떨어지는 더 많은 추가적인 정보를 입력합니다 (입력됨)";
            }
            else
            {
                BtnExtraInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNobookInfo.png");
                BtnExtraInfo.ToolTip = "중요성이 떨어지는 더 많은 추가적인 정보를 입력합니다 (미입력)";
            }
        }

        private void BtnDropCards_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputDropBookInfosWindow(innerCardInfo.dropBooks).ShowDialog();

            if (innerCardInfo.dropBooks.Count > 0)
            {
                string extraInfo = "";
                innerCardInfo.dropBooks.ForEach((string dropBookInfo) =>
                {
                    extraInfo += $"{dropBookInfo}\n";
                });
                extraInfo = extraInfo.TrimEnd('\n');

                BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                BtnDropCards.ToolTip = $"이 전투책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
            }
            else
            {
                BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                BtnDropCards.ToolTip = "이 전투책장이 어느 책에서 드랍되는지 입력합니다 (미입력)";
            }
        }

        private void BtnCopyCard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.cardInfos.Add(Tools.DeepCopy.DeepClone(innerCardInfo));
            initStack();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.cardInfos.Remove(innerCardInfo);
            initStack();
        }
        #endregion
        #region Type Change Buttons
        private void BtnRangeType_Click(object sender, RoutedEventArgs e)
        {
            switch (innerCardInfo.rangeType)
            {
                case "Near":
                    innerCardInfo.rangeType = "Far";
                    break;
                case "Far":
                    innerCardInfo.rangeType = "FarArea";
                    break;
                case "FarArea":
                    innerCardInfo.rangeType = "FarAreaEach";
                    break;
                case "FarAreaEach":
                    innerCardInfo.rangeType = "Near";
                    break;
            }
            UpdateRangeTypeUI();
        }

        private void UpdateRangeTypeUI()
        {
            switch (innerCardInfo.rangeType)
            {
                case "Near":
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 근거리 책장)";
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNomal.png");
                    break;
                case "Far":
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 원거리 책장)";
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeRange.png");
                    break;
                case "FarArea":
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 광역 합산 책장)";
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeFarArea.png");
                    break;
                case "FarAreaEach":
                    BtnRangeType.ToolTip = "클릭시 원거리 속성을 변경합니다. (현재 : 광역 개별 책장)";
                    BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeFarAreaEach.png");
                    break;
            }
        }


        private void BtnUnqueType_Click(object sender, RoutedEventArgs e)
        {
            switch (innerCardInfo.option)
            {
                case "OnlyPage":
                    innerCardInfo.option = "";
                    break;
                default:
                    innerCardInfo.option = "OnlyPage";
                    break;
            }
            UpdateUniqueTypeUI();
        }

        private void UpdateUniqueTypeUI()
        {
            switch (innerCardInfo.option)
            {
                case "OnlyPage":
                    BtnUnqueType.ToolTip = "클릭시 고유 책장의 여부를 변경합니다. (현재 : 고유 책장)";
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeUniquePage.png");
                    break;
                default:
                    BtnUnqueType.ToolTip = "클릭시 고유 책장의 여부를 변경합니다. (현재 : 일반 책장)";
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/TypeNotUniquePage.png");
                    break;
            }
        }
        #endregion

        #region Left side buttons
        private void BtnCardImage_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputCardImageWindow((string cardImageDes) =>
            {
                BtnCardImage.Content = cardImageDes;
                BtnCardImage.ToolTip = cardImageDes;
                innerCardInfo.cardImage = cardImageDes;
            }).ShowDialog();
        }

        private void BtnCardEffect_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputEffectWindow((string effectDes) =>
            {
                BtnCardEffect.Content = effectDes;
                BtnCardEffect.ToolTip = effectDes.Replace(".", ".\n");
                innerCardInfo.cardScript = effectDes;
            }, isCardEffect: true).ShowDialog();
        }
        #endregion

        #region Text change events
        private void TbxCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCardInfo.cost = TbxCost.Text;
        }

        private void TbxCardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCardInfo.name = TbxCardName.Text;
        }

        private void TbxCardUniqueID_TextChanged(object sender, TextChangedEventArgs e)
        {
            innerCardInfo.cardID = TbxCardUniqueID.Text;
        }
        #endregion

        #region Controls for dices
        private void InitSqlDices()
        {
            SqlDices.Children.Clear();
            innerCardInfo.dices.ForEach((DS.Dice dice) =>
            {
                SqlDices.Children.Add(new EditDice(innerCardInfo.dices, dice, InitSqlDices));
            });
        }

        private void BtnAddDice_Click(object sender, RoutedEventArgs e)
        {
            innerCardInfo.dices.Add(new DS.Dice());
            InitSqlDices();
        }
        #endregion
    }
}

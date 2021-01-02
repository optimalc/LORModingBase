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
    /// EditDice.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDice : UserControl
    {
        List<DS.Dice> diceListToUse = null;
        DS.Dice innerDice = null;
        Action stackInitFunc = null;

        public EditDice(List<DS.Dice> diceListToUse, DS.Dice innerDice, Action stackInitFunc)
        {
            this.diceListToUse = diceListToUse;
            this.innerDice = innerDice;
            this.stackInitFunc = stackInitFunc;
            InitializeComponent();

            UpdateEffectGrid();
            UpdateDiceTypeUI();
        }

        private void UpdateEffectGrid()
        {
            LblEffect.Content = innerDice.script;
            LblEffect.ToolTip = innerDice.script.Replace(".", "\n.");
            RectAllInfo.Visibility = Visibility.Collapsed;
            RectDiceOnly.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(innerDice.script))
            {
                RectDiceOnly.Visibility = Visibility.Visible;
                GldEffect.Visibility = Visibility.Collapsed;
            }   
            else
            {
                RectAllInfo.Visibility = Visibility.Visible;
                GldEffect.Visibility = Visibility.Visible;
            }
        }

        #region Button events
        private void BtnEffect_Click(object sender, RoutedEventArgs e)
        {
            new SubWindows.InputEffectWindow((string effectDes) =>
            {
                innerDice.script = effectDes;
                UpdateEffectGrid();
            }, isCardEffect: false).ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            diceListToUse.Remove(innerDice);
            stackInitFunc();
        }
        #endregion
        #region Dice type buttons
        private void BtnDiceType_Click(object sender, RoutedEventArgs e)
        {
            GldInfo.Visibility = Visibility.Collapsed;
            GldChangeAttackType.Visibility = Visibility.Visible;
        }

        private void UpdateDiceTypeUI()
        {
            GldInfo.Visibility = Visibility.Visible;
            GldChangeAttackType.Visibility = Visibility.Collapsed;

            switch ($"{innerDice.type}_{innerDice.detail}")
            {
                case "Atk_Slash":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconSlash.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 참격 주사위)";
                    break;
                case "Atk_Penetrate":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconPenetrate.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 관통 주사위)";
                    break;
                case "Atk_Hit":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconHit.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 타격 주사위)";
                    break;

                case "Def_Guard":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconGuard.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 방어 주사위)";
                    break;
                case "Def_Evasion":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconEvasion.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 회피 주사위)";
                    break;

                case "Stadby_Slash":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconStadbySlash.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 반격 참격 주사위)";
                    break;
                case "Stadby_Penetrate":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconStadbyPenetrate.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 반격 관통 주사위)";
                    break;
                case "Stadby_Hit":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconStadbyHit.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 반격 타격 주사위)";
                    break;
                case "Stadby_Guard":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconStadbyGuard.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 반격 방어 주사위)";
                    break;
                case "Stadby_Evasion":
                    BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconStadbyEvasion.png");
                    BtnDiceType.ToolTip = $"클릭시 주사위의 속성을 변경합니다. (현재 : 반격 회피 주사위)";
                    break;
            }
        }


        private void BtnDiceType_Atk_Slash_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Atk";
            innerDice.detail = "Slash";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Atk_Penetrate_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Atk";
            innerDice.detail = "Penetrate";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Atk_Hit_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Atk";
            innerDice.detail = "Hit";
            UpdateDiceTypeUI();
        }


        private void BtnDiceType_Def_Guard_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Def";
            innerDice.detail = "Guard";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Def_Evasion_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Def";
            innerDice.detail = "Evasion";
            UpdateDiceTypeUI();
        }


        private void BtnDiceType_Stadby_Slash_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Stadby";
            innerDice.detail = "Slash";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Stadby_Penetrate_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Stadby";
            innerDice.detail = "Penetrate";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Stadby_Hit_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Stadby";
            innerDice.detail = "Hit";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Stadby_Guard_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Stadby";
            innerDice.detail = "Guard";
            UpdateDiceTypeUI();
        }

        private void BtnDiceType_Stadby_Evasion_Click(object sender, RoutedEventArgs e)
        {
            innerDice.type = "Stadby";
            innerDice.detail = "Evasion";
            UpdateDiceTypeUI();
        }
        #endregion
    }
}

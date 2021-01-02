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
        }

        private void UpdateEffectGrid()
        {
            LblEffect.Content = innerDice.script;
            LblEffect.ToolTip = innerDice.script;
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
            innerDice.script = "체력이 25% 이상일때 속도가 3 증가한다. 하지만 아닐 수도 있다.";
            UpdateEffectGrid();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            diceListToUse.Remove(innerDice);
            stackInitFunc();
        } 
        #endregion
    }
}

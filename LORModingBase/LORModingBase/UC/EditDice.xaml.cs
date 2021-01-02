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
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            diceListToUse.Remove(innerDice);
            stackInitFunc();
        }
    }
}

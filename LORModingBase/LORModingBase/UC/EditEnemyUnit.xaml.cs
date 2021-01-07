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
    /// EditEnemyUnit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditEnemyUnit : UserControl
    {
        DM.XmlDataNode innerEnemyNode = null;
        Action initStack = null;

        public EditEnemyUnit(DM.XmlDataNode innerEnemyNode, Action initStack)
        {
            InitializeComponent();
            this.innerEnemyNode = innerEnemyNode;
            this.initStack = initStack;
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerEnemyNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxNameID":
                    break;
                case "TbxEnemyName":
                    break;
                case "TbxMinHeight":
                    break;
                case "TbxMaxHeight":
                    break;
                case "TbxEnemyUniqueID":
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }

        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnBookID":
                    break;
                case "BtnDeckID":
                    break;

                case "LblDropTable0":
                    break;
                case "LblDropTable1":
                    break;
                case "LblDropTable2":
                    break;
                case "LblDropTable3":
                    break;
            }
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnCopyEnemy":
                    break;
                case "BtnDelete":
                    break;
            }
        }
    }
}

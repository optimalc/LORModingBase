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
    /// EditDeck.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDeck : UserControl
    {
        DM.XmlDataNode innerDeckNode = null;
        Action initStack = null;

        public EditDeck(DM.XmlDataNode innerDeckNode, Action initStack)
        {
            InitializeComponent();
            this.innerDeckNode = innerDeckNode;
            this.initStack = initStack;
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerDeckNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxDeckID":
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }

        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnAddDeck":
                    break;
                case "BtnDeleteDeck":
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
                case "BtnCopyDeck":
                    break;
                case "BtnDelete":
                    break;
            }
        }
    }
}

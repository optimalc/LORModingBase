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
    /// EditCardAbility.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCardAbility : UserControl
    {
        DM.XmlDataNode innerAbilityNode = null;
        Action initStack = null;

        public EditCardAbility(DM.XmlDataNode innerAbilityNode, Action initStack)
        {
            InitializeComponent();
            this.innerAbilityNode = innerAbilityNode;
            this.initStack = initStack;
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerAbilityNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxAbilityID":
                    innerAbilityNode.attribute["ID"] = tbx.Text;
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnCopyAbility":
                    //DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Add(innerDeckNode.Copy());
                    //initStack();
                    //MainWindow.mainWindow.UpdateDebugInfo();
                    //MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;
                case "BtnDelete":
                    //DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Remove(innerDeckNode);
                    //initStack();
                    //MainWindow.mainWindow.UpdateDebugInfo();
                    //MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;
            }
        }
    }
}

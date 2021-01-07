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

        #region Init controls
        public EditDeck(DM.XmlDataNode innerDeckNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DECK_INFO);
            this.innerDeckNode = innerDeckNode;
            this.initStack = initStack;

            TbxDeckID.Text = innerDeckNode.GetAttributesSafe("ID");
            InitLbxCards();
        }

        public void InitLbxCards()
        {
            LbxCards.Items.Clear();
            innerDeckNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
            {
                LbxCards.Items.Add(cardNode.innerText);
            });
        } 
        #endregion

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
                    innerDeckNode.attribute["ID"] = tbx.Text;
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
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerDeckNode.AddXmlInfoByPath("Card", selectedItem);
                        InitLbxCards();
                        
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CARDS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;
                case "BtnDeleteDeck":
                    if(LbxCards.SelectedItem != null)
                    {
                        innerDeckNode.RemoveXmlInfosByPath("Card", LbxCards.SelectedItem.ToString(), deleteOnce:true);
                        InitLbxCards();
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
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
                    DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Add(innerDeckNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;
                case "BtnDelete":
                    DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.subNodes.Remove(innerDeckNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;
            }
        }
    }
}

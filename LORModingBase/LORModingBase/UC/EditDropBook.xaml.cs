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
    /// EditDropBook.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDropBook : UserControl
    {
        DM.XmlDataNode innerBookNode = null;
        Action initStack = null;

        #region Init controls
        public EditDropBook(DM.XmlDataNode innerBookNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DROP_BOOK_INFO);
            this.innerBookNode = innerBookNode;
            this.initStack = initStack;

            TbxBookID.Text = innerBookNode.GetAttributesSafe("ID");

            TbxNameID.Text = innerBookNode.GetInnerTextByPath("TextId");
            TbxBookName.Text = DM.GameInfos.localizeInfos["etc"].rootDataNode.GetInnerTextByAttributeWithPath("text", "id", innerBookNode.GetInnerTextByPath("TextId"));


            innerBookNode.ActionIfInnertTextIsNotNullOrEmpty("BookIcon", (string innerText) =>
            {
                BtnBookIcon.ToolTip = innerText;

                LblBookIcon.Content = innerText;
                BtnBookIcon.Content = "          ";
            });
            innerBookNode.ActionIfInnertTextIsNotNullOrEmpty("Chapter", (string innerText) =>
            {
                BtnChapter.ToolTip = innerText;

                LblChapter.Content = innerText;
                BtnChapter.Content = "          ";
            });

            InitLbxKeyPage();
            InitLbxCards();
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
        }

        public void InitLbxKeyPage()
        {
            LbxKeyPage.Items.Clear();
            innerBookNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
            {
                LbxKeyPage.Items.Add(dropItemNode.innerText);
            });
        }

        public void InitLbxCards()
        {
            LbxCards.Items.Clear();
            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", innerBookNode.GetAttributesSafe("ID"),
                (DM.XmlDataNode cardDropTableNode) => {
                    cardDropTableNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        LbxCards.Items.Add(cardNode.innerText);
                    });
            });
        } 
        #endregion

        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnBookIcon":
                    break;
                case "BtnChapter":
                    break;

                case "BtnSelectKeyPage":
                    break;
                case "BtnSelectCards":
                    break;
            }
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerBookNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxBookID":
                    break;
                case "TbxNameID":
                    break;
                case "TbxBookName":
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
                case "BtnCopyBook":
                    break;
                case "BtnDelete":
                    break;
            }
        }
    }
}

﻿using LORModingBase.CustomExtensions;
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
            TbxBookName.Text = DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.GetInnerTextByAttributeWithPath("text", "id", innerBookNode.GetInnerTextByPath("TextId"));


            innerBookNode.ActionIfInnertTextIsNotNullOrEmpty("BookIcon", (string innerText) =>
            {
                string ICON_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"ICON");

                BtnBookIcon.ToolTip = $"{ICON_WORD} : {innerText}";

                LblBookIcon.Content = $"{ICON_WORD} : {innerText}";
                BtnBookIcon.Content = "          ";
            });
            innerBookNode.ActionIfInnertTextIsNotNullOrEmpty("Chapter", (string innerText) =>
            {
                string CHAPTER_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Chapter");
                string CHPATER_DES = DM.LocalizedGameDescriptions.GetDescriptionForChapter(innerText);
                BtnChapter.ToolTip = $"{CHAPTER_WORD} : {CHPATER_DES}:{innerText}";

                LblChapter.Content = $"{CHAPTER_WORD} : {CHPATER_DES}:{innerText}"; ;
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
                LbxKeyPage.Items.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(dropItemNode.innerText));
            });
        }

        public void InitLbxCards()
        {
            LbxCards.Items.Clear();
            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", innerBookNode.GetAttributesSafe("ID"),
                (DM.XmlDataNode cardDropTableNode) => {
                    cardDropTableNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        LbxCards.Items.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(cardNode.innerText));
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
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        string ICON_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"ICON");

                        innerBookNode.SetXmlInfoByPath("BookIcon", selectedItem);
                        BtnBookIcon.ToolTip = $"{ICON_WORD} : {selectedItem}";

                        LblBookIcon.Content = $"{ICON_WORD} : {selectedItem}";
                        BtnBookIcon.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.BOOK_ICON).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                    break;
                case "BtnChapter":
                    new SubWindows.Global_ListSeleteWindow((string selectedChapterDes) => {
                        string CHAPTER_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Chapter");
                        innerBookNode.SetXmlInfoByPath("Chapter", selectedChapterDes.Split(':').Last());
                        BtnChapter.ToolTip = $"{CHAPTER_WORD} : {selectedChapterDes}";

                        LblChapter.Content = $"{CHAPTER_WORD} : {selectedChapterDes}";
                        BtnChapter.Content = "          ";
                    }, SubWindows.Global_ListSeleteWindow_PRESET.CHAPTERS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                    break;

                case "BtnSelectKeyPage":
                    List<string> keyPageList = new List<string>();
                    innerBookNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                    {
                        keyPageList.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(dropItemNode.innerText));
                    });
                    new SubWindows.Global_AddItemToListWindow((string addedItem) =>
                    {
                        innerBookNode.AddXmlInfoByPath("DropItem", addedItem, 
                            attributePairsToSet: new Dictionary<string, string>() { { "Type","Equip" } });
                    }, (string deletedItem) => {

                        innerBookNode.RemoveXmlInfosByPath("DropItem", deletedItem, deleteOnce: true);
                    }, keyPageList, SubWindows.AddItemToListWindow_PRESET.CRITICAL_BOOKS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                    InitLbxKeyPage();
                    break;
                case "BtnSelectCards":
                    List<string> cardList = new List<string>();
                    DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", innerBookNode.GetAttributesSafe("ID"),
                        (DM.XmlDataNode cardDropTableNode) => {
                            cardDropTableNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                            {
                                cardList.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(cardNode.innerText));
                            });
                        });
                    new SubWindows.Global_AddItemToListWindow((string addedItem) =>
                    {
                       List<DM.XmlDataNode> cardDropTablesToCheck = DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                             attributeToCheck: new Dictionary<string, string>() { { "ID", innerBookNode.GetAttributesSafe("ID") } });
                       if (cardDropTablesToCheck.Count <= 0)
                            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.AddXmlInfoByPath("DropTable",
                                attributePairsToSet: new Dictionary<string, string>() { { "ID", innerBookNode.GetAttributesSafe("ID") } });

                       DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", innerBookNode.GetAttributesSafe("ID"),
                            (DM.XmlDataNode cardDropTableNode) => {
                            cardDropTableNode.AddXmlInfoByPath("Card", addedItem);
                        });
                    }, (string deletedItem) => {
                        DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", innerBookNode.GetAttributesSafe("ID"),
                       (DM.XmlDataNode cardDropTableNode) => {
                           cardDropTableNode.RemoveXmlInfosByPath("Card", deletedItem);
                       });
                    }, cardList, SubWindows.AddItemToListWindow_PRESET.CARDS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD_DROP_TABLE_INFO);
                    InitLbxCards();
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
                    string PREV_BOOK_ID = innerBookNode.GetAttributesSafe("ID");
                    #region Card drop table info ID refrect
                    DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "ID", PREV_BOOK_ID,
                        (DM.XmlDataNode cardTableNode) => {
                            cardTableNode.attribute["ID"] = tbx.Text;
                        });
                    #endregion
                    innerBookNode.attribute["ID"] = tbx.Text;
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD_DROP_TABLE_INFO);
                    break;
                case "TbxNameID":
                    string PREV_TEXT_ID = innerBookNode.GetInnerTextByPath("TextId");
                    innerBookNode.SetXmlInfoByPath("TextId", tbx.Text);
                    #region Make new name ID
                    List<DM.XmlDataNode> foundLocalizeBookNamesNameID = DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("text",
                                attributeToCheck: new Dictionary<string, string>() { { "id", innerBookNode.GetInnerTextByPath("TextId") } });
                    if (foundLocalizeBookNamesNameID.Count <= 0 && !string.IsNullOrEmpty(tbx.Text))
                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.subNodes.Add(DM.EditGameData_DropBookInfo.MakeNewDropBookNameBase(tbx.Text, TbxBookName.Text));
                    #endregion
                    #region Remove prev name ID
                    List<DM.XmlDataNode> foundLocalizeBookNameID = DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse/TextId",
                    PREV_TEXT_ID);
                    if (foundLocalizeBookNameID.Count <= 0 && !string.IsNullOrEmpty(tbx.Text))
                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.RemoveXmlInfosByPath("text",
                            attributeToCheck: new Dictionary<string, string>() { { "id", PREV_TEXT_ID } }); 
                    else if(string.IsNullOrEmpty(tbx.Text))
                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.RemoveXmlInfosByPath("text",
                            attributeToCheck: new Dictionary<string, string>() { { "id", PREV_TEXT_ID } });
                    #endregion
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_DROP_BOOK_NAME);
                    break;
                case "TbxBookName":
                    if(DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("text",
                       attributeToCheck: new Dictionary<string, string>() { { "id", innerBookNode.GetInnerTextByPath("TextId") } }) &&
                       !string.IsNullOrEmpty(innerBookNode.GetInnerTextByPath("TextId")))
                    {
                        DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.ActionXmlDataNodesByAttributeWithPath("text",
                            "id", innerBookNode.GetInnerTextByPath("TextId"), (DM.XmlDataNode nameNode) => {
                                nameNode.innerText = tbx.Text;
                            });
                    }
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_DROP_BOOK_NAME);
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
                    DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.subNodes.Add(innerBookNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                    break;
                case "BtnDelete":
                    if (DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("text",
                         attributeToCheck: new Dictionary<string, string>() { { "id", innerBookNode.GetInnerTextByPath("TextId") } }))
                    {
                        if (DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse/TextId",
                            innerBookNode.GetInnerTextByPath("TextId")).Count == 1)
                            DM.EditGameData_DropBookInfo.LocalizedDropBookName.rootDataNode.RemoveXmlInfosByPath("text",
                                attributeToCheck: new Dictionary<string, string>() { { "id", innerBookNode.GetInnerTextByPath("TextId") } });
                    }
                    if (DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerBookNode.GetAttributesSafe("ID") } }))
                    {
                        if (DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("BookUse",
                            attributeToCheck:new Dictionary<string, string>() { { "ID", innerBookNode.GetAttributesSafe("ID")} }).Count == 1)
                            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo.rootDataNode.RemoveXmlInfosByPath("DropTable",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", innerBookNode.GetAttributesSafe("ID") } });
                    }

                    DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.subNodes.Remove(innerBookNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DROP_BOOK_INFO);
                    break;
            }
        }
    }
}

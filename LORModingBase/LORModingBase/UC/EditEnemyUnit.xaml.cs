﻿using System;
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

        #region Init controls
        public EditEnemyUnit(DM.XmlDataNode innerEnemyNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.ENEMY_INFO);
            this.innerEnemyNode = innerEnemyNode;
            this.initStack = initStack;

            TbxEnemyUniqueID.Text = innerEnemyNode.GetAttributesSafe("ID");
            TbxNameID.Text = innerEnemyNode.GetInnerTextByPath("NameID");
            TbxMinHeight.Text = innerEnemyNode.GetInnerTextByPath("MinHeight");
            TbxMaxHeight.Text = innerEnemyNode.GetInnerTextByPath("MaxHeight");

            List<DM.XmlDataNode> foundLocalizeCharNames = DM.GameInfos.localizeInfos["CharactersName"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetInnerTextByPath("NameID") } });
            if (foundLocalizeCharNames.Count > 0)
                TbxEnemyName.Text = foundLocalizeCharNames[0].innerText;


            innerEnemyNode.ActionIfInnertTextIsNotNullOrEmpty("BookId", (string innerText) =>
            {
                string BOOK_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"BOOK");

                BtnBookID.ToolTip = $"{BOOK_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(innerText)}";

                LblBookID.Content = $"{BOOK_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(innerText)}";
                BtnBookID.Content = "          ";
            });
            innerEnemyNode.ActionIfInnertTextIsNotNullOrEmpty("DeckId", (string innerText) =>
            {
                string DECK_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"DECK");

                BtnDeckID.ToolTip = $"{DECK_WORD} : {DM.LocalizedGameDescriptions.GetDecriptionForDeck(innerText)}";

                LblDeckID.Content = $"{DECK_WORD} : {DM.LocalizedGameDescriptions.GetDecriptionForDeck(innerText)}";
                BtnDeckID.Content = "          ";
            });

            InitRetreatButton();
            InitRewards();
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
        }

        private void InitRetreatButton()
        {
            if(innerEnemyNode.GetInnerTextByPath("Retreat") == "true")
            {
                BtnRetreat.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesRetreat.png");
                BtnRetreat.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"%BtnRetreat_ToolTip%")} (ON)";
            }
            else
            {
                BtnRetreat.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoRetreat.png");
                BtnRetreat.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"%BtnRetreat_ToolTip%")} (OFF)";
            }
        }

        private void InitRewards()
        {
            SqlRewards.Children.Clear();
            innerEnemyNode.ActionXmlDataNodesByPath("DropTable", (DM.XmlDataNode dropTableNode) =>
            {
                string EMOTION_LEVEL = dropTableNode.GetAttributesSafe("Level");
                dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItem) =>
                {
                    SqlRewards.Children.Add(new EditEmotionReward(dropItem, EMOTION_LEVEL));
                });
            });
        } 
        #endregion

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
                    #region Add new name ID if not exist
                    string PREV_NAME_ID = innerEnemyNode.GetInnerTextByPath("NameID");
                    innerEnemyNode.SetXmlInfoByPath("NameID", tbx.Text);
                    List<DM.XmlDataNode> foundLocalizeCharNamesNameID = DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetInnerTextByPath("NameID") } });
                    if (foundLocalizeCharNamesNameID.Count <= 0 && !string.IsNullOrEmpty(tbx.Text))
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.subNodes.Add(DM.EditGameData_EnemyInfo.MakeNewCharactersNameBase(tbx.Text, TbxEnemyName.Text));
                    #endregion
                    #region Remove prev name ID if not exist
                    if(!DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Enemy/NameID", PREV_NAME_ID))
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.RemoveXmlInfosByPath("Name",
                            attributeToCheck: new Dictionary<string, string>() { { "ID" , PREV_NAME_ID } });
                    #endregion


                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_CHAR_NAME);
                    break;
                case "TbxEnemyName":
                    List<DM.XmlDataNode> foundLocalizeCharNames = DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetInnerTextByPath("NameID") } });
                    if(foundLocalizeCharNames.Count > 0)
                        foundLocalizeCharNames[0].innerText = tbx.Text;
                    else if (!string.IsNullOrEmpty(innerEnemyNode.GetInnerTextByPath("NameID")))
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.subNodes.Add(DM.EditGameData_EnemyInfo.MakeNewCharactersNameBase(TbxNameID.Text, tbx.Text));
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_CHAR_NAME);
                    break;
                case "TbxMinHeight":
                    innerEnemyNode.SetXmlInfoByPath("MinHeight", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
                case "TbxMaxHeight":
                    innerEnemyNode.SetXmlInfoByPath("MaxHeight", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
                case "TbxEnemyUniqueID":
                    innerEnemyNode.attribute["ID"] = tbx.Text;
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
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
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerEnemyNode.SetXmlInfoByPath("BookId", selectedItem);
                        string BOOK_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"BOOK");

                        BtnBookID.ToolTip = $"{BOOK_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(selectedItem)}";

                        LblBookID.Content = $"{BOOK_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(selectedItem)}";
                        BtnBookID.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CRITICAL_BOOKS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
                case "BtnDeckID":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerEnemyNode.SetXmlInfoByPath("DeckId", selectedItem);
                        string DECK_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.ENEMY_INFO, $"DECK");

                        BtnDeckID.ToolTip = $"{DECK_WORD} : {DM.LocalizedGameDescriptions.GetDecriptionForDeck(selectedItem)}";

                        LblDeckID.Content = $"{DECK_WORD} : {DM.LocalizedGameDescriptions.GetDecriptionForDeck(selectedItem)}";
                        BtnDeckID.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.DECKS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_DECKS);
                    break;

                case "BtnRewardLevel_0":
                case "BtnRewardLevel_1":
                case "BtnRewardLevel_2":
                case "BtnRewardLevel_3":
                    string DROP_BOOK_LEVEL = btn.Name.Split('_').Last();
                    List<string> dropTableItems = new List<string>();
                    innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", DROP_BOOK_LEVEL, (DM.XmlDataNode dropTableNode) =>
                    {
                        dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                        {
                            dropTableItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(dropItemNode.innerText));
                        });
                    });
                    new SubWindows.Global_AddItemToListWindow((string addedDropBookItemID) =>
                    {
                        if (!innerEnemyNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                            attributeToCheck: new Dictionary<string, string>() { { "Level", DROP_BOOK_LEVEL } }))
                        {
                            innerEnemyNode.AddXmlInfoByPath("DropTable",
                                attributePairsToSet: new Dictionary<string, string>() { { "Level", DROP_BOOK_LEVEL } });
                        }

                        innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", DROP_BOOK_LEVEL, (DM.XmlDataNode dropTableNode) =>
                        {
                            dropTableNode.AddXmlInfoByPath("DropItem", valueToSet: addedDropBookItemID,
                                attributePairsToSet: new Dictionary<string, string>() { { "Prob", "1" } });
                        });

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, (string deletedDropBookItemID) =>
                    {
                        innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", DROP_BOOK_LEVEL, (DM.XmlDataNode dropTableNode) =>
                        {
                            dropTableNode.RemoveXmlInfosByPath("DropItem", deletedDropBookItemID);
                        });

                        List<string> dropIDs = new List<string>();
                        innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", DROP_BOOK_LEVEL, (DM.XmlDataNode dropTableNode) =>
                        {
                            dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                            {
                                dropIDs.Add(dropItemNode.innerText);
                            });
                        });
                        if (dropIDs.Count <= 0)
                            innerEnemyNode.RemoveXmlInfosByPath("DropTable",
                                attributeToCheck: new Dictionary<string, string>() { { "Level", DROP_BOOK_LEVEL } });

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, dropTableItems, SubWindows.AddItemToListWindow_PRESET.DROP_BOOK).ShowDialog();
                    InitRewards();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
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
                case "BtnRetreat":
                    if (innerEnemyNode.CheckIfGivenPathWithXmlInfoExists("Retreat"))
                        innerEnemyNode.RemoveXmlInfosByPath("Retreat");
                    else
                        innerEnemyNode.AddXmlInfoByPath("Retreat", "true");
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    InitRetreatButton();
                    break;
                case "BtnCopyEnemy":
                    DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.subNodes.Add(innerEnemyNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
                case "BtnDelete":
                    if (DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                       attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetAttributesSafe("ID") } }))
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.RemoveXmlInfosByPath("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetAttributesSafe("id") } });
                    DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.subNodes.Remove(innerEnemyNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
            }
        }
    }
}

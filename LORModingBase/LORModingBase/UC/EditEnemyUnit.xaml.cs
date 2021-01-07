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
                BtnBookID.ToolTip = innerText;

                LblBookID.Content = innerText;
                BtnBookID.Content = "          ";
            });
            innerEnemyNode.ActionIfInnertTextIsNotNullOrEmpty("DeckId", (string innerText) =>
            {
                BtnDeckID.ToolTip = innerText;

                LblDeckID.Content = innerText;
                BtnDeckID.Content = "          ";
            });

            UpdateDropTables();
        }

        private void UpdateDropTables()
        {
            innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", "0", (DM.XmlDataNode dropTableNode) =>
            {
                string dropItemToAdd = "";
                dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                {
                    dropItemToAdd += $"{dropItemNode.innerText} /";
                });
                dropItemToAdd.Trim('/');


                BtnDropTable0.ToolTip = dropItemToAdd;

                LblDropTable0.Content = dropItemToAdd;
                BtnDropTable0.Content = "          ";
            });

            innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", "1", (DM.XmlDataNode dropTableNode) =>
            {
                string dropItemToAdd = "";
                dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                {
                    dropItemToAdd += $"{dropItemNode.innerText} /";
                });
                dropItemToAdd.Trim('/');


                BtnDropTable1.ToolTip = dropItemToAdd;

                LblDropTable1.Content = dropItemToAdd;
                BtnDropTable1.Content = "          ";
            });

            innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", "2", (DM.XmlDataNode dropTableNode) =>
            {
                string dropItemToAdd = "";
                dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                {
                    dropItemToAdd += $"{dropItemNode.innerText} /";
                });
                dropItemToAdd.Trim('/');


                BtnDropTable2.ToolTip = dropItemToAdd;

                LblDropTable2.Content = dropItemToAdd;
                BtnDropTable2.Content = "          ";
            });

            innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", "3", (DM.XmlDataNode dropTableNode) =>
            {
                string dropItemToAdd = "";
                dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                {
                    dropItemToAdd += $"{dropItemNode.innerText} /";
                });
                dropItemToAdd.Trim('/');


                BtnDropTable3.ToolTip = dropItemToAdd;

                LblDropTable3.Content = dropItemToAdd;
                BtnDropTable3.Content = "          ";
            });
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
                    innerEnemyNode.SetXmlInfoByPath("NameID", tbx.Text);
                    List<DM.XmlDataNode> foundLocalizeCharNamesNameID = DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetInnerTextByPath("NameID") } });
                    if(foundLocalizeCharNamesNameID.Count <= 0)
                        DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.subNodes.Add(DM.EditGameData_EnemyInfo.MakeNewCharactersNameBase(tbx.Text, TbxNameID.Text));
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_CHAR_NAME);
                    break;
                case "TbxEnemyName":
                    List<DM.XmlDataNode> foundLocalizeCharNames = DM.EditGameData_EnemyInfo.LocalizedCharactersName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerEnemyNode.GetInnerTextByPath("NameID") } });
                    if(foundLocalizeCharNames.Count > 0)
                        foundLocalizeCharNames[0].innerText = tbx.Text;
                    else
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
                        BtnBookID.ToolTip = selectedItem;

                        LblBookID.Content = selectedItem;
                        BtnBookID.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CRITICAL_BOOKS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
                case "BtnDeckID":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerEnemyNode.SetXmlInfoByPath("DeckId", selectedItem);
                        BtnDeckID.ToolTip = selectedItem;

                        LblDeckID.Content = selectedItem;
                        BtnDeckID.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.DECKS).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;

                case "BtnDropTable0":
                case "BtnDropTable1":
                case "BtnDropTable2":
                case "BtnDropTable3":

                    List<string> dropTableItems = new List<string>();
                    innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", btn.Name.Last().ToString(), (DM.XmlDataNode dropTableNode) =>
                    {
                        dropTableNode.ActionXmlDataNodesByPath("DropItem", (DM.XmlDataNode dropItemNode) =>
                        {
                            dropTableItems.Add(dropItemNode.innerText);
                        });
                    });
                    string DROP_BOOK_LEVEL = btn.Name.Last().ToString();
                    new SubWindows.Global_AddItemToListWindow((string addedDropBookItemID) =>
                    {
                        if (!innerEnemyNode.CheckIfGivenPathWithXmlInfoExists("DropTable", 
                            attributeToCheck:new Dictionary<string, string>() { {"Level", DROP_BOOK_LEVEL } }))
                        {
                            innerEnemyNode.AddXmlInfoByPath("DropTable",
                                attributePairsToSet:new Dictionary<string, string>() { { "Level", DROP_BOOK_LEVEL } });
                        }

                        innerEnemyNode.ActionXmlDataNodesByAttributeWithPath("DropTable", "Level", DROP_BOOK_LEVEL, (DM.XmlDataNode dropTableNode) =>
                        {
                            dropTableNode.AddXmlInfoByPath("DropItem", valueToSet: addedDropBookItemID,
                                attributePairsToSet: new Dictionary<string, string>() { { "Prob", "1" } });
                        });

                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, (string deletedDropBookItemID) => {
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
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, dropTableItems, SubWindows.AddItemToListWindow_PRESET.DROP_BOOK).ShowDialog();
                    UpdateDropTables();
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

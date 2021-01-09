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
using LORModingBase.CustomExtensions;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditCard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditCard : UserControl
    {
        DM.XmlDataNode innerCardNode = null;
        Action initStack = null;

        #region Init controls
        public EditCard(DM.XmlDataNode innerCardNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.CARD_INFO);
            this.innerCardNode = innerCardNode;
            this.initStack = initStack;

            innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
            {
                TbxCost.Text = specNode.GetAttributesSafe("Cost");
            });
            switch (innerCardNode.GetInnerTextByPath("Rarity"))
            {
                case "Common":
                    ChangeRarityButtonEvents(BtnRarity_Common, null);
                    break;
                case "Uncommon":
                    ChangeRarityButtonEvents(BtnRarity_Uncommon, null);
                    break;
                case "Rare":
                    ChangeRarityButtonEvents(BtnRarity_Rare, null);
                    break;
                case "Unique":
                    ChangeRarityButtonEvents(BtnRarity_Unique, null);
                    break;
            }

            TbxCardName.Text = innerCardNode.GetInnerTextByPath("Name");
            TbxCardUniqueID.Text = innerCardNode.GetAttributesSafe("ID");

            innerCardNode.ActionIfInnertTextIsNotNullOrEmpty("Artwork", (string innerText) =>
            {
                string IMAGE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"IMAGE");

                BtnCardImage.ToolTip = $"{IMAGE_WORD} : {innerText}";

                BtnCardImage.Content = "          ";
                LblCardImage.Content = $"{IMAGE_WORD} : {innerText}";
            });

            innerCardNode.ActionIfInnertTextIsNotNullOrEmpty("Script", (string innerText) =>
            {
                string ABILITY_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"ABILITY");

                BtnCardEffect.ToolTip = $"{ABILITY_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(innerText)}:{innerText}";

                BtnCardEffect.Content = "          ";
                LblCardEffect.Content = $"{ABILITY_WORD} : {innerText}";
            });
            InitSqlDices();

            UpdateExtrainfoIcon();
            innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) => {
                BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{specNode.GetAttributesSafe("Range")}.png");
                BtnRangeType.Tag = specNode.GetAttributesSafe("Range");
                if(BtnRangeType.Tag != null && !string.IsNullOrEmpty(BtnRangeType.Tag.ToString()))
                    BtnRangeType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnRangeType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Type_{BtnRangeType.Tag}")})";
            });

            List<DM.XmlDataNode> optionNodes = innerCardNode.GetXmlDataNodesByPath("Option");
            if (optionNodes.Count > 0)
            {
                DM.XmlDataNode OPTION_NODE = optionNodes[0];
                if (OPTION_LOOP_LIST.Contains(OPTION_NODE.innerText))
                {
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{OPTION_NODE.innerText}.png");
                    BtnUnqueType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnUnqueType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Type_{BtnUnqueType.Tag}")})";
                }
                else
                {
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/TypeETC.png");
                    BtnUnqueType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnUnqueType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {OPTION_NODE.innerText})";
                }
                BtnUnqueType.Tag = OPTION_NODE.innerText;
            }
            else
            {
                BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/TypeNoOption.png");
                BtnUnqueType.Tag = "";
                BtnUnqueType.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnUnqueType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Type_NoOption")})";
            }

            #region 드랍되는 곳 체크
            List<string> selectedCardDropTables = new List<string>();
            DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEachSafe((DM.XmlDataNode cardDropTableID) =>
            {
                if (cardDropTableID.CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.attribute["ID"]))
                    selectedCardDropTables.Add(cardDropTableID.attribute["ID"]);
            });

            if (selectedCardDropTables.Count > 0)
            {
                string extraInfo = "";
                selectedCardDropTables.ForEach((string dropBookInfo) =>
                {
                    extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(dropBookInfo)}\n";
                });
                extraInfo = extraInfo.TrimEnd('\n');

                BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                BtnDropCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnDropCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
            }
            #endregion
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }

        /// <summary>
        /// Change rarity button events
        /// </summary>
        private void ChangeRarityButtonEvents(object sender, RoutedEventArgs e)
        {
            Button rarityButton = sender as Button;

            BtnRarity_Common.Background = null;
            BtnRarity_Uncommon.Background = null;
            BtnRarity_Rare.Background = null;
            BtnRarity_Unique.Background = null;

            rarityButton.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
            WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr(rarityButton.Tag.ToString());
            innerCardNode.SetXmlInfoByPath("Rarity", rarityButton.Name.Split('_').Last());
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }

        /// <summary>
        /// Init dice stack panel
        /// </summary>
        private void InitSqlDices()
        {
            SqlDices.Children.Clear();
            innerCardNode.GetXmlDataNodesByPath("BehaviourList/Behaviour").ForEachSafe((DM.XmlDataNode behaviourNode) =>
            {
                SqlDices.Children.Add(new EditDice(innerCardNode, behaviourNode, InitSqlDices));
            });
        }
        #endregion

        #region Type change button events
        /// <summary>
        /// Ranege info name
        /// </summary>
        private List<string> RANGE_LOOP_LIST = new List<string>() { "Near", "Far", "FarArea", "FarAreaEach" };

        /// <summary>
        /// Option info name
        /// </summary>
        private List<string> OPTION_LOOP_LIST = new List<string>() { "", "OnlyPage", "Basic", "EGO" };

        /// <summary>
        /// Type loop button events
        /// </summary>
        private void TypeLoopButtonEvents(object sender, MouseButtonEventArgs e)
        {
            Button loopButton = sender as Button;

            List<string> LOOP_LIST = null;
            if (loopButton.Name == "BtnRangeType")
                LOOP_LIST = RANGE_LOOP_LIST;
            else if (loopButton.Name == "BtnUnqueType")
                LOOP_LIST = OPTION_LOOP_LIST;
            if (LOOP_LIST == null)
                return;

            if (loopButton.Tag == null || LOOP_LIST.IndexOf(loopButton.Tag.ToString()) < 0)
                loopButton.Tag = LOOP_LIST[0];
            // Down index
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                int LEFT_INDEX = LOOP_LIST.IndexOf(loopButton.Tag.ToString()) - 1;
                if (LEFT_INDEX < 0) LEFT_INDEX = LOOP_LIST.Count - 1;
                loopButton.Tag = LOOP_LIST[LEFT_INDEX];
            }
            // Up index
            else
            {
                int RIGHT_INDEX = LOOP_LIST.IndexOf(loopButton.Tag.ToString()) + 1;
                if (RIGHT_INDEX >= LOOP_LIST.Count) RIGHT_INDEX = 0;
                loopButton.Tag = LOOP_LIST[RIGHT_INDEX];
            }

            if (loopButton.Name == "BtnRangeType")
            {
                loopButton.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{loopButton.Tag}.png");
                loopButton.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnRangeType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Type_{loopButton.Tag}")})";
                innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) => {
                    specNode.attribute["Range"] = loopButton.Tag.ToString();
                });

                switch (loopButton.Tag.ToString())
                {
                    case "FarArea":
                    case "FarAreaEach":
                        innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
                        {
                            specNode.attribute["Affection"] = "Team";
                        });
                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                        break;
                    default:
                        innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
                        {
                            if(specNode.attribute.ContainsKey("Affection"))
                                specNode.attribute.Remove("Affection");
                        });
                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                        break;
                }
            }
            else if (loopButton.Name == "BtnUnqueType")
            {
                string UNIQUE_NAME = (string.IsNullOrEmpty(loopButton.Tag.ToString()) ? "NoOption" : loopButton.Tag.ToString());
                loopButton.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{UNIQUE_NAME}.png");
                loopButton.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnUnqueType_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Current")} : {DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Type_{UNIQUE_NAME}")})";
                innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("Option", loopButton.Tag.ToString());
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }
        #endregion

        #region Right side buttons
        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnExtraInfo":
                    string AffectionPrev = "";
                    innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
                    {
                        AffectionPrev = specNode.GetAttributesSafe("Affection");
                    });

                    new SubWindows.Global_MultipleValueInputed(new Dictionary<string, string>() {
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"Chapter"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnDropCards_ToolTip%")},
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityEnemy"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityEnemy_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityUser"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityUser_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityScript"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"PriorityScript_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"EmotionLimit"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"EmotionLimit_ToolTip") },
                        { DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"WideAreaCode"), DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"WideAreaCode_ToolTip") }
                    }, new List<string>()
                    {
                        innerCardNode.GetInnerTextByPath("Chapter"),
                        innerCardNode.GetInnerTextByPath("Priority"),
                        innerCardNode.GetInnerTextByPath("SortPriority"),
                        innerCardNode.GetInnerTextByPath("PriorityScript"),
                        innerCardNode.GetInnerTextByPath("EmotionLimit"),
                        AffectionPrev
                    }, new List<Action<string>>()
                    {
                        (string inputedVar) => {
                            innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("Chapter", inputedVar);},
                        (string inputedVar) => {
                            innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("Priority", inputedVar);},
                        (string inputedVar) => {
                            innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("SortPriority", inputedVar);},
                        (string inputedVar) => {
                            innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("PriorityScript", inputedVar);},
                        (string inputedVar) => {
                            innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("EmotionLimit", inputedVar);},
                        (string inputedVar) => {
                            innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
                            {
                                specNode.attribute["Affection"] = inputedVar;
                                MainWindow.mainWindow.UpdateDebugInfo();
                            });}
                    }).ShowDialog();
                    UpdateExtrainfoIcon();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "BtnDropCards":
                    List<string> selectedCardDropTables = new List<string>();
                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEachSafe((DM.XmlDataNode cardDropTableID) =>
                    {
                        if (cardDropTableID.CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.GetAttributesSafe("ID")))
                            selectedCardDropTables.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(cardDropTableID.attribute["ID"]));
                    });

                    new SubWindows.Global_AddItemToListWindow((string addedDropTableItemID) =>
                    {
                        if (DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", addedDropTableItemID } }))
                        {
                            List<DM.XmlDataNode> foundCardDropTableNode = (DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                             attributeToCheck: new Dictionary<string, string>() { { "ID", addedDropTableItemID } }));
                            if (foundCardDropTableNode.Count > 0
                               && !foundCardDropTableNode[0].CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.GetAttributesSafe("ID")))
                                foundCardDropTableNode[0].AddXmlInfoByPath("Card", innerCardNode.GetAttributesSafe("ID"));
                        }
                        else
                        {
                            DM.XmlDataNode madeDropTableNode = DM.EditGameData_CardInfos.MakeNewStaticCardDropTableBase(addedDropTableItemID);
                            if (!madeDropTableNode.CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.GetAttributesSafe("ID")))
                            {
                                madeDropTableNode.AddXmlInfoByPath("Card", innerCardNode.GetAttributesSafe("ID"));
                                DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.subNodes.Add(madeDropTableNode);
                            }
                        }
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, (string deletedDropTableItemID) => {
                        if (DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.CheckIfGivenPathWithXmlInfoExists("DropTable",
                          attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropTableItemID } }))
                        {
                            List<DM.XmlDataNode> foundCardDropTableNode = (DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                             attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropTableItemID } }));
                            if (foundCardDropTableNode.Count > 0)
                            {
                                DM.XmlDataNode FOUND_CARD_DROP_TABLE_NODE = foundCardDropTableNode[0];

                                List<DM.XmlDataNode> baseCardDropTableNode = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropTableItemID } });
                                if (baseCardDropTableNode.Count > 0)
                                {
                                    DM.XmlDataNode FOUND_CARD_DROP_TABLE_IN_GAME = baseCardDropTableNode[0];

                                    List<string> foundCardDropTablesInGameItems = new List<string>();
                                    FOUND_CARD_DROP_TABLE_IN_GAME.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                    {
                                        foundCardDropTablesInGameItems.Add(cardNode.innerText);
                                    });


                                    if (DM.EditGameData_CardInfos.StaticCard.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                             attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }).Count == 1
                                             && !foundCardDropTablesInGameItems.Contains(innerCardNode.GetAttributesSafe("ID")))
                                        FOUND_CARD_DROP_TABLE_NODE.RemoveXmlInfosByPath("Card", innerCardNode.GetAttributesSafe("ID"));


                                    List<string> foundCardDropTables = new List<string>();
                                    FOUND_CARD_DROP_TABLE_NODE.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                    {
                                        foundCardDropTables.Add(cardNode.innerText);
                                    });


                                    if (foundCardDropTables.Count == foundCardDropTablesInGameItems.Count
                                        && foundCardDropTables.Except(foundCardDropTablesInGameItems).Count() == 0
                                        && DM.EditGameData_CardInfos.StaticCard.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }).Count == 1)
                                    {
                                        bool isContainAnyExistCard = false;
                                        DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                        {
                                            if (foundCardDropTables.Contains(cardNode.GetAttributesSafe("ID")) &&
                                                    cardNode.GetAttributesSafe("ID") != innerCardNode.GetAttributesSafe("ID"))
                                                isContainAnyExistCard = true;
                                        });
                                        if (!isContainAnyExistCard)
                                            DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.RemoveXmlInfosByPath("DropTable",
                                                attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropTableItemID } }, deleteOnce: true);
                                    }
                                }
                            }
                            MainWindow.mainWindow.UpdateDebugInfo();
                        }
                    }, selectedCardDropTables, SubWindows.AddItemToListWindow_PRESET.DROP_TABLE).ShowDialog();

                    List<string> selectedCardDropTablesToCheck = new List<string>();
                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEachSafe((DM.XmlDataNode cardDropTableID) =>
                    {
                        if (cardDropTableID.CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.attribute["ID"]))
                            selectedCardDropTablesToCheck.Add(cardDropTableID.attribute["ID"]);
                    });

                    if (selectedCardDropTablesToCheck.Count > 0)
                    {
                        string extraInfo = "";
                        selectedCardDropTablesToCheck.ForEach((string dropBookInfo) =>
                        {
                            extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(dropBookInfo)}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                        BtnDropCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnDropCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                    }
                    else
                    {
                        BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                        BtnDropCards.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"%BtnDropCards_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD_DROP_TABLE);
                    break;
                case "BtnCopyCard":
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Add(innerCardNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "BtnDelete":
                    if (DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.CheckIfGivenPathWithXmlInfoExists("cardDescList/BattleCardDesc",
                      attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }))
                        DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.RemoveXmlInfosByPath("cardDescList/BattleCardDesc",
                       attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }, deleteOnce:true);

                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEach((DM.XmlDataNode dropTableNode) =>
                    {
                        List<DM.XmlDataNode> baseCardDropTableNode = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", dropTableNode.GetAttributesSafe("ID") } });
                        if (baseCardDropTableNode.Count > 0)
                        {
                            DM.XmlDataNode FOUND_CARD_DROP_TABLE_IN_GAME = baseCardDropTableNode[0];

                            List<string> foundCardDropTablesInGameItems = new List<string>();
                            FOUND_CARD_DROP_TABLE_IN_GAME.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                            {
                                foundCardDropTablesInGameItems.Add(cardNode.innerText);
                            });

                            if (DM.EditGameData_CardInfos.StaticCard.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }).Count == 1
                                && !foundCardDropTablesInGameItems.Contains(innerCardNode.GetAttributesSafe("ID")))
                            {
                                dropTableNode.RemoveXmlInfosByPath("Card", innerCardNode.GetAttributesSafe("ID"));
                            }

                            List<string> foundCardDropTables = new List<string>();
                            dropTableNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                            {
                                foundCardDropTables.Add(cardNode.innerText);
                            });

                            if (foundCardDropTables.Count == foundCardDropTablesInGameItems.Count
                                && foundCardDropTables.Except(foundCardDropTablesInGameItems).Count() == 0 &&
                                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }).Count == 1)
                            {
                                bool isContainAnyExistCard = false;
                                DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                {
                                    if (foundCardDropTables.Contains(cardNode.GetAttributesSafe("ID")) &&
                                        cardNode.GetAttributesSafe("ID") != innerCardNode.GetAttributesSafe("ID"))
                                        isContainAnyExistCard = true;
                                });
                                if(!isContainAnyExistCard)
                                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.RemoveXmlInfosByPath("DropTable",
                                        attributeToCheck: new Dictionary<string, string>() { { "ID", dropTableNode.GetAttributesSafe("ID") } }, deleteOnce: true);
                            }
                        }
                    });

                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Remove(innerCardNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
            }
        }

        private void UpdateExtrainfoIcon()
        {
            string CHAPTER = innerCardNode.GetInnerTextByPath("Chapter");
            string PRIORITY = innerCardNode.GetInnerTextByPath("Priority");
            string SORT_PRIORITY = innerCardNode.GetInnerTextByPath("SortPriority");
            string PRIORITY_SCRIPT = innerCardNode.GetInnerTextByPath("PriorityScript");
            string EMOTION_LIMIT = innerCardNode.GetInnerTextByPath("EmotionLimit");

            string AFFECTION = "";
            innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) =>
            {
                AFFECTION = specNode.GetAttributesSafe("Affection");
            });

            if (!string.IsNullOrEmpty(CHAPTER) ||
                !string.IsNullOrEmpty(PRIORITY) ||
                !string.IsNullOrEmpty(SORT_PRIORITY) ||
                !string.IsNullOrEmpty(PRIORITY_SCRIPT) ||
                !string.IsNullOrEmpty(EMOTION_LIMIT) ||
                !string.IsNullOrEmpty(AFFECTION))
            {
                BtnExtraInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png");
                BtnExtraInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"ExtraInput")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})";
            }
            else
            {
                BtnExtraInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNobookInfo.png");
                BtnExtraInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"ExtraInput")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
            }
        }
        #endregion

        /// <summary>
        /// Button events that need search window
        /// </summary>
        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnCardImage":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        string IMAGE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"IMAGE");

                        innerCardNode.SetXmlInfoByPath("Artwork", selectedItem);
                        BtnCardImage.ToolTip = $"{IMAGE_WORD} : {selectedItem}";

                        BtnCardImage.Content = "          ";
                        LblCardImage.Content = $"{IMAGE_WORD} : {selectedItem}";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CARD_ARTWORK).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "BtnCardEffect":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        string ABILITY_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, $"ABILITY");

                        innerCardNode.SetXmlInfoByPath("Script", selectedItem);
                        BtnCardEffect.ToolTip = $"{ABILITY_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(selectedItem)}:{selectedItem}";

                        BtnCardEffect.Content = "          ";
                        LblCardEffect.Content = $"{ABILITY_WORD} : {selectedItem}";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CARD_ABILITES).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "BtnAddDice":
                    innerCardNode.GetXmlDataNodesByPath("BehaviourList").ForEachSafe((DM.XmlDataNode behaviourListNode) =>
                    {
                        behaviourListNode.subNodes.Add(DM.EditGameData_CardInfos.MakeNewDiceBase());
                    });
                    InitSqlDices();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
            }
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerCardNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxCost":
                    innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) => {
                        specNode.attribute["Cost"] = tbx.Text;
                        MainWindow.mainWindow.UpdateDebugInfo();
                    });
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "TbxCardName":
                    innerCardNode.SetXmlInfoByPath("Name", tbx.Text);

                    if (DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.CheckIfGivenPathWithXmlInfoExists("cardDescList/BattleCardDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].SetXmlInfoByPath("LocalizedName", tbx.Text);
                        }
                    }
                    else
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.GetXmlDataNodesByPath("cardDescList");

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].subNodes.Add(DM.EditGameData_CardInfos.MakeNewLocalizedBattleCardsBase(
                               innerCardNode.GetAttributesSafe("ID"),
                               innerCardNode.GetInnerTextByPath("Name")));
                        }
                        else
                        {
                            DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.MakeEmptyNodeGivenPathIfNotExist("cardDescList")
                                .subNodes.Add(DM.EditGameData_CardInfos.MakeNewLocalizedBattleCardsBase(
                               innerCardNode.GetAttributesSafe("ID"),
                               innerCardNode.GetInnerTextByPath("Name")));
                        }
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BATTLE_CARDS);
                    break;
                case "TbxCardUniqueID":
                    string PREV_CARD_ID = innerCardNode.GetAttributesSafe("ID");
                    #region Card info locailizing refrect
                    if (DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.CheckIfGivenPathWithXmlInfoExists("cardDescList/BattleCardDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_CARD_ID } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("cardDescList/BattleCardDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_CARD_ID } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].attribute["ID"] = tbx.Text;
                        }
                    }
                    #endregion
                    #region Card Drop Table Reflect
                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEachSafe((DM.XmlDataNode dropTableNode) =>
                    {
                        dropTableNode.GetXmlDataNodesByPathWithXmlInfo("Card").ForEachSafe((DM.XmlDataNode cardNode) =>
                        {
                            if (cardNode.innerText == PREV_CARD_ID)
                                cardNode.innerText = tbx.Text;
                        });
                    });
                    #endregion
                    innerCardNode.attribute["ID"] = tbx.Text;
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                default:
                    List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                    if (SPLIT_NAME.Count == 2)
                        innerCardNode.SetXmlInfoByPath(SPLIT_NAME.Last(), tbx.Text);
                    else if (SPLIT_NAME.Count > 2)
                        innerCardNode.SetXmlInfoByPath(String.Join("/", SPLIT_NAME.Skip(1)), tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
            }
        }
    }
}

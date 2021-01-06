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
                BtnCardImage.Content = innerText;
                BtnCardImage.ToolTip = innerText;
            });

            innerCardNode.ActionIfInnertTextIsNotNullOrEmpty("Script", (string innerText) =>
            {
                BtnCardEffect.Content = innerText;
                BtnCardEffect.ToolTip = innerText;
            });
            InitSqlDices();

            UpdateExtrainfoIcon();
            innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) => {
                BtnRangeType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{specNode.GetAttributesSafe("Range")}.png");
                BtnRangeType.Tag = specNode.GetAttributesSafe("Range");
            });

            List<DM.XmlDataNode> optionNodes = innerCardNode.GetXmlDataNodesByPath("Option");
            if (optionNodes.Count > 0)
            {
                DM.XmlDataNode OPTION_NODE = optionNodes[0];
                if (OPTION_LOOP_LIST.Contains(OPTION_NODE.innerText))
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{OPTION_NODE.innerText}.png");
                else
                    BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/TypeETC.png");
                BtnUnqueType.Tag = OPTION_NODE.innerText;
            }
            else
            {
                BtnUnqueType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/TypeNoOption.png");
                BtnUnqueType.Tag = "";
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
                    extraInfo += $"{dropBookInfo}\n";
                });
                extraInfo = extraInfo.TrimEnd('\n');

                BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                BtnDropCards.ToolTip = $"이 전투책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
            }
            #endregion
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
                innerCardNode.ActionXmlDataNodesByPath("Spec", (DM.XmlDataNode specNode) => {
                    specNode.attribute["Range"] = loopButton.Tag.ToString();
                });
            }
            else if (loopButton.Name == "BtnUnqueType")
            {
                string UNIQUE_NAME = (string.IsNullOrEmpty(loopButton.Tag.ToString()) ? "NoOption" : loopButton.Tag.ToString());
                loopButton.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/Type{UNIQUE_NAME}.png");
                innerCardNode.SetXmlInfoByPathAndEmptyWillRemove("Option", loopButton.Tag.ToString());
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
                        { "챕터", "카드가 속하는 챕터 번호입니다"},
                        { "책장 우선 순위(적)", "적이 책장을 선택할때 참조하게 되는 우선순위를 설정합니다" },
                        { "책장 우선 순위(유저)", "유저가 책장을 선택할때 참조하게 되는 우선순위를 설정합니다" },
                        { "우선순위 스크립트 명", "이 스크립트는 우선순위를 지정할때 사용됩니다" },
                        { "특정 감정이상에서만 사용가능", "특정 감정레벨 이상에서만 사용이 가능하도록 만듭니다" },
                        { "광역 전투책장 코드", "광역 전투책장에서 주로 사용하며, 광역 전투책장 적용시 자동으로 설정됩니다" }
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
                    break;
                case "BtnDropCards":
                    List<string> selectedCardDropTables = new List<string>();
                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEachSafe((DM.XmlDataNode cardDropTableID) =>
                    {
                        if (cardDropTableID.CheckIfGivenPathWithXmlInfoExists("Card", innerCardNode.GetAttributesSafe("ID")))
                            selectedCardDropTables.Add(cardDropTableID.attribute["ID"]);
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
                                FOUND_CARD_DROP_TABLE_NODE.RemoveXmlInfosByPath("Card", innerCardNode.GetAttributesSafe("ID"));

                                List<DM.XmlDataNode> baseCardDropTableNode = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", deletedDropTableItemID } });
                                if (baseCardDropTableNode.Count > 0)
                                {
                                    DM.XmlDataNode FOUND_CARD_DROP_TABLE_IN_GAME = baseCardDropTableNode[0];

                                    List<string> foundCardDropTables = new List<string>();
                                    FOUND_CARD_DROP_TABLE_NODE.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                    {
                                        foundCardDropTables.Add(cardNode.innerText);
                                    });

                                    List<string> foundCardDropTablesInGameItems = new List<string>();
                                    FOUND_CARD_DROP_TABLE_IN_GAME.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                                    {
                                        foundCardDropTablesInGameItems.Add(cardNode.innerText);
                                    });


                                    if (foundCardDropTables.Count == foundCardDropTablesInGameItems.Count
                                        && foundCardDropTables.Except(foundCardDropTablesInGameItems).Count() == 0)
                                    {
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
                            extraInfo += $"{dropBookInfo}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconYesDropBook.png");
                        BtnDropCards.ToolTip = $"이 전투책장이 어느 책에서 드랍되는지 입력합니다 (입력됨)\n{extraInfo}";
                    }
                    else
                    {
                        BtnDropCards.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/iconNoDropBook.png");
                        BtnDropCards.ToolTip = $"이 전투책장이 어느 책에서 드랍되는지 입력합니다 (미입력)";
                    }
                    break;
                case "BtnCopyCard":
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Add(innerCardNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    break;
                case "BtnDelete":
                    if (DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.CheckIfGivenPathWithXmlInfoExists("cardDescList/BattleCardDesc",
                      attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }))
                        DM.EditGameData_CardInfos.LocalizedBattleCards.rootDataNode.RemoveXmlInfosByPath("cardDescList/BattleCardDesc",
                       attributeToCheck: new Dictionary<string, string>() { { "ID", innerCardNode.GetAttributesSafe("ID") } }, deleteOnce:true);

                    DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.GetXmlDataNodesByPath("DropTable").ForEach((DM.XmlDataNode dropTableNode) =>
                    {
                        dropTableNode.RemoveXmlInfosByPath("Card", innerCardNode.GetAttributesSafe("ID"));

                        List<DM.XmlDataNode> baseCardDropTableNode = DM.GameInfos.staticInfos["CardDropTable"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("DropTable",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", dropTableNode.GetAttributesSafe("ID") } });
                        if (baseCardDropTableNode.Count > 0)
                        {
                            DM.XmlDataNode FOUND_CARD_DROP_TABLE_IN_GAME = baseCardDropTableNode[0];

                            List<string> foundCardDropTables = new List<string>();
                            dropTableNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                            {
                                foundCardDropTables.Add(cardNode.innerText);
                            });

                            List<string> foundCardDropTablesInGameItems = new List<string>();
                            FOUND_CARD_DROP_TABLE_IN_GAME.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                            {
                                foundCardDropTablesInGameItems.Add(cardNode.innerText);
                            });


                            if (foundCardDropTables.Count == foundCardDropTablesInGameItems.Count
                                && foundCardDropTables.Except(foundCardDropTablesInGameItems).Count() == 0)
                            {
                                DM.EditGameData_CardInfos.StaticCardDropTable.rootDataNode.RemoveXmlInfosByPath("DropTable",
                                    attributeToCheck: new Dictionary<string, string>() { { "ID", dropTableNode.GetAttributesSafe("ID") } }, deleteOnce: true);
                            }
                        }
                    });

                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.subNodes.Remove(innerCardNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
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
                BtnExtraInfo.ToolTip = "중요성이 떨어지는 더 많은 추가적인 정보를 입력합니다 (입력됨)";
            }
            else
            {
                BtnExtraInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNobookInfo.png");
                BtnExtraInfo.ToolTip = "중요성이 떨어지는 더 많은 추가적인 정보를 입력합니다 (미입력)";
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
                        innerCardNode.SetXmlInfoByPath("Artwork", selectedItem);
                        BtnCardImage.Content = selectedItem;
                        BtnCardImage.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CARD_ARTWORK).ShowDialog();
                    break;
                case "BtnCardEffect":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerCardNode.SetXmlInfoByPath("Script", selectedItem);
                        BtnCardEffect.Content = selectedItem;
                        BtnCardEffect.ToolTip = selectedItem;
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.CARD_ABILITES).ShowDialog();
                    break;
                case "BtnAddDice":
                    innerCardNode.GetXmlDataNodesByPath("BehaviourList").ForEachSafe((DM.XmlDataNode behaviourListNode) =>
                    {
                        behaviourListNode.subNodes.Add(DM.EditGameData_CardInfos.MakeNewDiceBase());
                    });
                    InitSqlDices();
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
                    break;
                default:
                    List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                    if (SPLIT_NAME.Count == 2)
                        innerCardNode.SetXmlInfoByPath(SPLIT_NAME.Last(), tbx.Text);
                    else if (SPLIT_NAME.Count > 2)
                        innerCardNode.SetXmlInfoByPath(String.Join("/", SPLIT_NAME.Skip(1)), tbx.Text);
                    break;
            }
        }
    }
}

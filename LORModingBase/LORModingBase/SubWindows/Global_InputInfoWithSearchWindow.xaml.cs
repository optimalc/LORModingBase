using LORModingBase.CustomExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_InputInfoWithSearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_InputInfoWithSearchWindow : Window
    {
        Action<string> afterSelectItem = null;
        List<string> selectItems = null;

        #region Constructor and preset
        public Global_InputInfoWithSearchWindow(Action<string> afterSelectItem, List<string> searchTypes, List<string> selectItems,
            string windowTitle= "사용할 내용을 더블클릭", string helpMessage="※ 사용할 내용을 더블클릭 하세요", string ItemHelpMessage="얻어진 내용들" )
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectItem = afterSelectItem;
            this.selectItems = selectItems;
            InitLbxSearchType(searchTypes);

            this.Title = windowTitle;
            LblHelpMessage.Content = helpMessage;
            LblItemHelpMessage.Content = ItemHelpMessage;
        }

        public Global_InputInfoWithSearchWindow(Action<string> afterSelectItem, InputInfoWithSearchWindow_PRESET preset)
        {
            InitializeComponent();
            string CUSTOM_ITEM_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM");
            string IMAGE_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\ArtWork";
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectItem = afterSelectItem;
            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (preset)
            {
                case InputInfoWithSearchWindow_PRESET.EPISODE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"EPISODE_TITLE");
                    #region Add custom items
                    DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.ActionXmlDataNodesByPath("Stage", (DM.XmlDataNode customNode) =>
                    {
                        string STAGE_ID = customNode.GetAttributesSafe("id");
                        if (!string.IsNullOrEmpty(STAGE_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(STAGE_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["StageInfo"].rootDataNode.ActionXmlDataNodesByPath("Stage", (DM.XmlDataNode stageNode) =>
                    {
                        string STAGE_ID = stageNode.GetAttributesSafe("id");
                        if (!string.IsNullOrEmpty(STAGE_ID) && Convert.ToInt32(STAGE_ID) < DS.FilterDatas.STAGEINFO_DIV_NOT_CREATURE && STAGE_ID != "1")
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(STAGE_ID));
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case InputInfoWithSearchWindow_PRESET.BOOK_ICON:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"BOOK_ICON_TITLE");
                    #region Add custom items
                    if (Directory.Exists(IMAGE_DIRECTORY))
                    {
                        Directory.GetFiles(IMAGE_DIRECTORY).ForEachSafe((string imagePath) =>
                        {
                            if (imagePath.Split('.').Last().ToLower() == "png" || imagePath.Split('.').Last().ToLower() == "jpg")
                            {
                                string CUSTOM_FILTER_DES = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM");
                                string CUSTOM_NAME = imagePath.Split('\\').Last().Split('.')[0];

                                selectItems.Add($"{CUSTOM_FILTER_DES} {CUSTOM_NAME}:{CUSTOM_NAME}");
                            }
                        });
                    }
                    searchTypes.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM"));
                    #endregion
                    DM.GameInfos.staticInfos["DropBook"].rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode bookUseNode) =>
                    {
                        string BOOK_ICON_NAME = bookUseNode.GetInnerTextByPath("BookIcon");
                        string BOOK_ICON_DES = DM.LocalizedGameDescriptions.GetDescriptionForETC(bookUseNode.GetInnerTextByPath("TextId"));
                        string CHPATER_NAME = DM.LocalizedGameDescriptions.GetDescriptionForChapter(bookUseNode.GetInnerTextByPath("Chapter"));
                        if (!string.IsNullOrEmpty(BOOK_ICON_NAME))
                            selectItems.Add($"{CHPATER_NAME} / {BOOK_ICON_DES}:{BOOK_ICON_NAME}");
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case InputInfoWithSearchWindow_PRESET.CHARACTER_SKIN:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CHARACTER_SKIN_TITLE");
                    DM.GameInfos.staticInfos["EquipPage"].rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode bookNode) =>
                    {
                        string CHPATER_NAME = DM.LocalizedGameDescriptions.GetDescriptionForChapter(bookNode.GetInnerTextByPath("Chapter"));
                        string BOOK_DES = DM.LocalizedGameDescriptions.GetDescriptionForBooks(bookNode.GetInnerTextByPath("TextId"));
                        string SKIN_NAME = bookNode.GetInnerTextByPath("CharacterSkin");
                        if (!string.IsNullOrEmpty(SKIN_NAME))
                            selectItems.Add($"{CHPATER_NAME} / {BOOK_DES}:{SKIN_NAME}");
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case InputInfoWithSearchWindow_PRESET.PASSIVE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"PASSIVE_TITLE");
                    LblHelpMessage.Content = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"PASSIVE_HELP");
                    #region Add custom items
                    DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.ActionXmlDataNodesByPath("Passive", (DM.XmlDataNode customNode) =>
                    {
                        string PASSIVE_ID = customNode.GetAttributesSafe("ID");
                        string PASSIVE_DES = DM.LocalizedGameDescriptions.GetDescriptionForPassive(customNode.GetAttributesSafe("ID"));
                        selectItems.Add($"{CUSTOM_ITEM_WORD} {PASSIVE_DES}:{PASSIVE_ID}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["PassiveList"].rootDataNode.ActionXmlDataNodesByPath("Passive", (DM.XmlDataNode passiveNode) =>
                    {
                        string PASSIVE_ID = passiveNode.GetAttributesSafe("ID");
                        string PASSIVE_DES = DM.LocalizedGameDescriptions.GetDescriptionForPassive(passiveNode.GetAttributesSafe("ID"));

                        if(DM.EditGameData_BookInfos.IsPassiveAbliliable(PASSIVE_ID))
                            selectItems.Add($"{PASSIVE_DES}:{PASSIVE_ID}");
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;
                case InputInfoWithSearchWindow_PRESET.CRITICAL_BOOKS:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CRITICAL_BOOKS_TITLE");
                    #region Add custom items
                    DM.EditGameData_BookInfos.StaticEquipPage.rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode customNode) =>
                    {
                        string EQ_BOOK_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(EQ_BOOK_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(EQ_BOOK_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["EquipPage"].rootDataNode.ActionXmlDataNodesByPath("Book", (DM.XmlDataNode eqNode) =>
                    {
                        string EQ_BOOK_ID = eqNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(EQ_BOOK_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForKeyBook(EQ_BOOK_ID));
                    });
                    searchTypes.AddRange(DM.GetDivideInfo.GetAllDividedKeyPageFilterInfo());
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;


                case InputInfoWithSearchWindow_PRESET.CARD_ARTWORK:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CARD_ARTWORK_TITLE");
                    #region Add custom items
                    if (Directory.Exists(IMAGE_DIRECTORY))
                    {
                        Directory.GetFiles(IMAGE_DIRECTORY).ForEachSafe((string imagePath) =>
                        {
                            if (imagePath.Split('.').Last().ToLower() == "png" || imagePath.Split('.').Last().ToLower() == "jpg")
                            {
                                string CUSTOM_FILTER_DES = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM");
                                string CUSTOM_NAME = imagePath.Split('\\').Last().Split('.')[0];

                                selectItems.Add($"{CUSTOM_FILTER_DES} {CUSTOM_NAME}:{CUSTOM_NAME}");
                            }
                        });
                    }
                    searchTypes.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM"));
                    #endregion
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        string ARTWORK_NAME = cardNode.GetInnerTextByPath("Artwork");
                        string CARD_DES = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(cardNode.GetAttributesSafe("ID"));
                        if (!string.IsNullOrEmpty(ARTWORK_NAME))
                            selectItems.Add($"{CARD_DES}:{ARTWORK_NAME}");
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;

                case InputInfoWithSearchWindow_PRESET.CARD_ABILITES:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CARD_ABILITES_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode customNode) =>
                    {
                        string ABILITIY_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode abilityNode) =>
                    {
                        string ABILITIY_ID = abilityNode.GetAttributesSafe("ID"); 
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            if(ABILITY_DES.Contains(DM.GetLocalizedFilterList.GetOnUseString()))
                                selectItems.Add($"{ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;
                case InputInfoWithSearchWindow_PRESET.DICE_ABILITES:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DICE_ABILITES_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode customNode) =>
                    {
                        string ABILITIY_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode abilityNode) =>
                    {
                        string ABILITIY_ID = abilityNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            if (!ABILITY_DES.Contains(DM.GetLocalizedFilterList.GetOnUseString()))
                                selectItems.Add($"{ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;
                case InputInfoWithSearchWindow_PRESET.ALL_ABILITES:
                    #region Add custom items
                    DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode customNode) =>
                    {
                        string ABILITIY_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.localizeInfos["BattleCardAbilities"].rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode abilityNode) =>
                    {
                        string ABILITIY_ID = abilityNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            selectItems.Add($"{ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;

                case InputInfoWithSearchWindow_PRESET.CARDS:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CARDS_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode customNode) =>
                    {
                        string CARD_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(CARD_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        string CARD_ID = cardNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(CARD_ID));
                    });
                    searchTypes.AddRange(DM.GetDivideInfo.GetAllDividedCardFilterInfo());
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case InputInfoWithSearchWindow_PRESET.MAP_INFO:
                    DM.GameInfos.staticInfos["StageInfo"].rootDataNode.ActionXmlDataNodesByPath("Stage/MapInfo", (DM.XmlDataNode mapInfoNode) =>
                    {
                        string MAP_INFO_ID = mapInfoNode.innerText;
                        if (!string.IsNullOrEmpty(MAP_INFO_ID))
                            selectItems.Add(DM.LocalizedGameDescriptions.GetDescriptionForMapInfo(MAP_INFO_ID));
                    });
                    break;

                case InputInfoWithSearchWindow_PRESET.FORMATION:
                    DM.GameInfos.staticInfos["FormationInfo"].rootDataNode.ActionXmlDataNodesByPath("Formation", (DM.XmlDataNode formationNode) =>
                    {
                        string formationID = formationNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(formationID))
                            selectItems.Add(DM.LocalizedGameDescriptions.GetDescriptionForFormation(formationID));
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.ENEMIES:
                    #region Add custom items
                    DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo.rootDataNode.ActionXmlDataNodesByPath("Enemy", (DM.XmlDataNode customNode) =>
                    {
                        string ENEMY_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ENEMY_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.LocalizedGameDescriptions.GetDescriptionForEnemy(ENEMY_ID)}:{ENEMY_ID}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["EnemyUnitInfo"].rootDataNode.ActionXmlDataNodesByPath("Enemy", (DM.XmlDataNode enemyID) =>
                    {
                        string ENEMY_ID = enemyID.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ENEMY_ID))
                            selectItems.Add($"{DM.LocalizedGameDescriptions.GetDescriptionForEnemy(ENEMY_ID)}:{ENEMY_ID}");
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
                case InputInfoWithSearchWindow_PRESET.DECKS:
                    #region Add custom items
                    DM.EditGameData_DeckInfo.StaticDeckInfo.rootDataNode.ActionXmlDataNodesByPath("Deck", (DM.XmlDataNode customNode) =>
                    {
                        string DECK_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(DECK_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.LocalizedGameDescriptions.GetDecriptionForDeck(DECK_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["Deck"].rootDataNode.ActionXmlDataNodesByPath("Deck", (DM.XmlDataNode deckID) =>
                    {
                        string DECK_ID = deckID.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(DECK_ID))
                            selectItems.Add(DM.LocalizedGameDescriptions.GetDecriptionForDeck(DECK_ID));
                    });
                    break;
                case InputInfoWithSearchWindow_PRESET.DROP_BOOK:
                    #region Add custom items
                    DM.EditGameData_DropBookInfo.StaticDropBookInfo.rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode customNode) =>
                    {
                        string BOOK_USE_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(BOOK_USE_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(BOOK_USE_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["DropBook"].rootDataNode.ActionXmlDataNodesByPath("BookUse", (DM.XmlDataNode bookUseNode) =>
                    {
                        string BOOK_USE_ID = bookUseNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(BOOK_USE_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(BOOK_USE_ID));
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;

                case InputInfoWithSearchWindow_PRESET.BUFFS:
                    #region Add custom items
                    DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList/BattleEffectText", (DM.XmlDataNode customNode) =>
                    {
                        string BUFF_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(BUFF_ID))
                        {
                            string BUFF_DES = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForBuff(BUFF_ID);
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {BUFF_DES}");
                        }
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.localizeInfos["EffectTexts"].rootDataNode.ActionXmlDataNodesByPath("effectTextList/BattleEffectText", (DM.XmlDataNode buffNode) =>
                    {
                        string BUFF_ID = buffNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(BUFF_ID))
                        {
                            string BUFF_DES = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForBuff(BUFF_ID);
                            selectItems.Add(BUFF_DES);
                        }
                    });
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;
            }
            InitLbxSearchType(searchTypes);
        }
        
        public Global_InputInfoWithSearchWindow(Action<string> afterSelectItem, string DLL_PRESET)
        {
            InitializeComponent();
            string CUSTOM_ITEM_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CUSTOM_ITEM");
            string IMAGE_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\ArtWork";
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelectItem = afterSelectItem;
            List<string> searchTypes = new List<string>();
            selectItems = new List<string>();

            switch (DLL_PRESET)
            {
                case DLL_EDITOR_SELECT_PRESET.CUSTOM_PASSIVE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"PASSIVE_TITLE");
                    LblHelpMessage.Content = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"PASSIVE_HELP");
                    #region Add custom items
                    DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.ActionXmlDataNodesByPath("Passive", (DM.XmlDataNode customNode) =>
                    {
                        string PASSIVE_ID = customNode.GetAttributesSafe("ID");
                        string PASSIVE_DES = DM.LocalizedGameDescriptions.GetDescriptionForPassive(customNode.GetAttributesSafe("ID"));
                        selectItems.Add($"{CUSTOM_ITEM_WORD} {PASSIVE_DES}:{PASSIVE_ID}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedPassives());
                    break;
                case DLL_EDITOR_SELECT_PRESET.CUSTOM_ABILITY:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CARD_ABILITES_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode.ActionXmlDataNodesByPath("BattleCardAbility", (DM.XmlDataNode customNode) =>
                    {
                        string ABILITIY_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(ABILITIY_ID))
                        {
                            string ABILITY_DES = DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(ABILITIY_ID);
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {ABILITY_DES}:{ABILITIY_ID}");
                        }
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    break;
                case DLL_EDITOR_SELECT_PRESET.PERSONAL_EGO_CARD:
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode customCardNode) =>
                    {
                        string CUSTOM_CARD_OPTION = customCardNode.GetInnerTextByPath("Option");
                        if (CUSTOM_CARD_OPTION == "EgoPersonal")
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(customCardNode.GetAttributesSafe("ID"))}");
                    });
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode customCardNode) =>
                    {
                        string CUSTOM_CARD_OPTION = customCardNode.GetInnerTextByPath("Option");
                        if (CUSTOM_CARD_OPTION == "EgoPersonal")
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(customCardNode.GetAttributesSafe("ID")));
                    });
                    break;
                case DLL_EDITOR_SELECT_PRESET.ALL_CARDS:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"CARDS_TITLE");
                    #region Add custom items
                    DM.EditGameData_CardInfos.StaticCard.rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode customNode) =>
                    {
                        string CARD_ID = customNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add($"{CUSTOM_ITEM_WORD} {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(CARD_ID)}");
                    });
                    searchTypes.Add(CUSTOM_ITEM_WORD);
                    #endregion
                    DM.GameInfos.staticInfos["Card"].rootDataNode.ActionXmlDataNodesByPath("Card", (DM.XmlDataNode cardNode) =>
                    {
                        string CARD_ID = cardNode.GetAttributesSafe("ID");
                        if (!string.IsNullOrEmpty(CARD_ID))
                            selectItems.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(CARD_ID));
                    });
                    searchTypes.AddRange(DM.GetDivideInfo.GetAllDividedCardFilterInfo());
                    searchTypes.AddRange(DM.GetLocalizedFilterList.GetLocalizedChapters());
                    break;
            }

            InitLbxSearchType(searchTypes);
        }
        #endregion

        #region Init controls
        private void InitLbxSearchType(List<string> searchTypes)
        {
            LbxSearchType.Items.Clear();
            LbxSearchType.Items.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"SEARCH_NO_FILTER"));
            searchTypes.ForEach((string searchType) =>
            {
                LbxSearchType.Items.Add(searchType);
            });

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitSearchItems();
            }
        }

        private void InitSearchItems()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxItems.Items.Clear();
                foreach (string selectItem in selectItems)
                {
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !selectItem.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;
                    if(LbxSearchType.SelectedIndex == 0)
                        LbxItems.Items.Add(selectItem);
                    else
                    {
                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            default:
                                if (selectItem.ToLower().Contains(LbxSearchType.SelectedItem.ToString().ToLower()))
                                    LbxItems.Items.Add(selectItem);
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        private void LbxItems_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterSelectItem(LbxItems.SelectedItem.ToString().Split(':').Last());
                this.Close();
            }
        }
        #region Search help methodes
        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitSearchItems();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitSearchItems();
        } 
        #endregion
    }

    public enum InputInfoWithSearchWindow_PRESET
    {
        EPISODE,
        BOOK_ICON,
        CHARACTER_SKIN,
        PASSIVE,
        CRITICAL_BOOKS,

        CARD_ARTWORK,
        CARD_ABILITES,
        DICE_ABILITES,
        ALL_ABILITES,
        CARDS,

        MAP_INFO,

        FORMATION,
        ENEMIES,
        DECKS,

        DROP_BOOK,
        BUFFS
    };
    public class DLL_EDITOR_SELECT_PRESET
    {
        public const string CUSTOM_PASSIVE = "CUSTOM_PASSIVE";
        public const string CUSTOM_ABILITY = "CUSTOM_ABILITY";
        public const string PERSONAL_EGO_CARD = "PERSONAL_EGO_CARD";
        public const string ALL_CARDS = "ALL_CARDS";
    }
}
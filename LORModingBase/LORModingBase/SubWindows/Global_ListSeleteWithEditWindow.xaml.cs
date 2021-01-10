using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LORModingBase.CustomExtensions;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_ListSeleteWithEditWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_ListSeleteWithEditWindow : Window
    {
        Action<string> afterSelect = null;
        Action afterAdd = null;
        Action<string> afterEdit = null;
        Action<string> afterDelete = null;

        public Global_ListSeleteWithEditWindow(Action<string> afterSelect, Action afterAdd, Action<string> afterEdit, Action<string> afterDelete, 
            List<string> itemToLoad, string windowTitle = "항목 선택 메뉴")
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelect = afterSelect;
            this.afterAdd = afterAdd;
            this.afterEdit = afterEdit;
            this.afterDelete = afterDelete;
            this.Title = windowTitle;

            itemToLoad.ForEach((string item) =>
            {
                LbxItems.Items.Add(item);
            });
        }

        public Global_ListSeleteWithEditWindow(Action<string> afterSelect, Action afterAdd, Action<string> afterEdit, Action<string> afterDelete, Global_ListSeleteWithEditWindow_PRESET PRESET)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
            this.afterSelect = afterSelect;
            this.afterAdd = afterAdd;
            this.afterEdit = afterEdit;
            this.afterDelete = afterDelete;
            List<string> itemToLoad = new List<string>();
            switch (PRESET)
            {
                case Global_ListSeleteWithEditWindow_PRESET.WORKING_SPACE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"WORKING_SPACE_TITLE");
                    string DirToSearch = "";
                    if (DM.Config.config.isDirectBaseModeExport)
                        DirToSearch = $"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\BaseMods";
                    else
                        DirToSearch = $"{DS.PROGRAM_PATHS.DIC_EXPORT_DATAS}";
                    Directory.GetDirectories(DirToSearch).ForEachSafe((string eachDir) =>
                    {
                        itemToLoad.Add(eachDir.Split('\\').Last());
                    });

                    this.afterSelect = (string selectedDirName) =>
                    {
                        DM.Config.ChangeWorkingDirectory(selectedDirName);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticEquipPage, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_BookInfos.StaticEquipPage = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticEquipPage, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_BookInfos.StaticEquipPage = new DM.XmlData(DM.GameInfos.staticInfos["EquipPage"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticDropBook, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_BookInfos.StaticDropBook = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticDropBook, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_BookInfos.StaticDropBook = new DM.XmlData(DM.GameInfos.staticInfos["DropBook"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_BookInfos.LocalizedBooks, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_BookInfos.LocalizedBooks = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_BookInfos.LocalizedBooks, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_BookInfos.LocalizedBooks = new DM.XmlData(DM.GameInfos.localizeInfos["Books"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCard, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.StaticCard = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCard, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_CardInfos.StaticCard = new DM.XmlData(DM.GameInfos.staticInfos["Card"]);
                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCardDropTable, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.StaticCardDropTable = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCardDropTable, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_CardInfos.StaticCardDropTable = new DM.XmlData(DM.GameInfos.staticInfos["CardDropTable"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardInfos.LocalizedBattleCards, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.LocalizedBattleCards = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardInfos.LocalizedBattleCards, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_CardInfos.LocalizedBattleCards = new DM.XmlData(DM.GameInfos.localizeInfos["BattlesCards"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_StageInfo.StaticStageInfo, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_StageInfo.StaticStageInfo = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_StageInfo.StaticStageInfo, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_StageInfo.StaticStageInfo = new DM.XmlData(DM.GameInfos.staticInfos["StageInfo"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_StageInfo.LocalizedStageName, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_StageInfo.LocalizedStageName = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_StageInfo.LocalizedStageName, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_StageInfo.LocalizedStageName = new DM.XmlData(DM.GameInfos.localizeInfos["StageName"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_EnemyInfo.StaticEnemyUnitInfo = new DM.XmlData(DM.GameInfos.staticInfos["EnemyUnitInfo"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_EnemyInfo.LocalizedCharactersName, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_EnemyInfo.LocalizedCharactersName = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_EnemyInfo.LocalizedCharactersName, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_EnemyInfo.LocalizedCharactersName = new DM.XmlData(DM.GameInfos.localizeInfos["CharactersName"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_DeckInfo.StaticDeckInfo, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_DeckInfo.StaticDeckInfo = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_DeckInfo.StaticDeckInfo, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_DeckInfo.StaticDeckInfo = new DM.XmlData(DM.GameInfos.staticInfos["Deck"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticDropBookInfo, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_DropBookInfo.StaticDropBookInfo = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticDropBookInfo, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_DropBookInfo.StaticDropBookInfo = new DM.XmlData(DM.GameInfos.staticInfos["DropBook"]);
                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticCardDropTableInfo, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_DropBookInfo.StaticCardDropTableInfo, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_DropBookInfo.StaticCardDropTableInfo = new DM.XmlData(DM.GameInfos.staticInfos["CardDropTable"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_DropBookInfo.LocalizedDropBookName, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_DropBookInfo.LocalizedDropBookName = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_DropBookInfo.LocalizedDropBookName, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_DropBookInfo.LocalizedDropBookName = new DM.XmlData(DM.GameInfos.localizeInfos["etc"]);

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_PassiveInfo.StaticPassiveList, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_PassiveInfo.StaticPassiveList = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_PassiveInfo.StaticPassiveList, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_PassiveInfo.StaticPassiveList = new DM.XmlData(DM.GameInfos.staticInfos["PassiveList"]);
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_PassiveInfo.LocalizedPassiveDesc, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_PassiveInfo.LocalizedPassiveDesc = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_PassiveInfo.LocalizedPassiveDesc, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_PassiveInfo.LocalizedPassiveDesc = new DM.XmlData(DM.GameInfos.localizeInfos["PassiveDesc"]);

                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardAbilityInfo.LocalizedCardAbility, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardAbilityInfo.LocalizedCardAbility = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardAbilityInfo.LocalizedCardAbility, DM.Config.CurrentWorkingDirectory));
                        else
                            DM.EditGameData_CardAbilityInfo.LocalizedCardAbility = new DM.XmlData(DM.GameInfos.localizeInfos["BattleCardAbilities"]);
                        this.Close();
                    };

                    this.afterAdd = () =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose:(string inputedName) =>
                        {
                            if(!string.IsNullOrEmpty(inputedName) && !Directory.Exists($"{DirToSearch}\\{inputedName}"))
                            {
                                Directory.CreateDirectory($"{DirToSearch}\\{inputedName}");
                                LbxItems.Items.Add(inputedName);
                            }
                        }, windowTitle: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"WORKING_SPACE_TITLE_ADD_TITLE"), 
                        tbxToolTip: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"WORKING_SPACE_TITLE_ADD_TOOLTIP")).ShowDialog();
                    };

                    this.afterEdit = (string selectedName) =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
                        {
                            if (!string.IsNullOrEmpty(inputedName) && Directory.Exists($"{DirToSearch}\\{selectedName}"))
                            {
                                if($"{DirToSearch}\\{selectedName}" != $"{DirToSearch}\\{inputedName}")
                                {
                                    Directory.Move($"{DirToSearch}\\{selectedName}", $"{DirToSearch}\\{inputedName}");
                                    LbxItems.Items[LbxItems.Items.IndexOf(selectedName)] = inputedName;
                                }
                            }
                        }, prevData: selectedName,  windowTitle: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"WORKING_SPACE_TITLE_EDIT_TITLE"), 
                        tbxToolTip: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"WORKING_SPACE_TITLE_EDIT_TOOLTIP")).ShowDialog();
                    };

                    this.afterDelete = (string selectedName) =>
                    {
                        Directory.Delete($"{DirToSearch}\\{selectedName}", true);
                        LbxItems.Items.Remove(selectedName);
                    };

                    break;

                case Global_ListSeleteWithEditWindow_PRESET.DLL_WORKING_SPACE:
                    this.Title = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DLL_WORKING_SPACE_TITLE");
                    string DLL_DIR_SEARCH_PATH = $"{DM.Config.CurrentWorkingDirectory}\\{DLLEditor.GlobalDatas.DLL_SOURCE_FILE_DIR_NAME}";
                    if (!Directory.Exists(DLL_DIR_SEARCH_PATH))
                        Directory.CreateDirectory(DLL_DIR_SEARCH_PATH);

                    Directory.GetFiles(DLL_DIR_SEARCH_PATH).ForEachSafe((string eachTxtFile) =>
                    {
                        itemToLoad.Add(eachTxtFile.Split('\\').Last().Replace(".json", ".dll"));
                    });

                    this.afterSelect = (string selectedFileName) =>
                    {
                        DLLEditor.DLLEditorMainWindow.targetSourceFilePath = $"{DLL_DIR_SEARCH_PATH}\\{selectedFileName.Replace(".dll", ".json")}";
                        if (File.Exists(DLLEditor.DLLEditorMainWindow.targetSourceFilePath))
                        {
                            DLLEditor.DLLEditorMainWindow.rootCodeBlocks = Tools.JsonFile.LoadJsonFile<List<DLLEditor.CodeBlock>>(DLLEditor.DLLEditorMainWindow.targetSourceFilePath);
                            if (DLLEditor.DLLEditorMainWindow.rootCodeBlocks == null)
                                DLLEditor.DLLEditorMainWindow.rootCodeBlocks = new List<DLLEditor.CodeBlock>();
                        }
                        this.Close();
                    };

                    this.afterAdd = () =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
                        {
                            if (!string.IsNullOrEmpty(inputedName) && !File.Exists($"{DLL_DIR_SEARCH_PATH}\\{inputedName}.json"))
                            {
                                File.Create($"{DLL_DIR_SEARCH_PATH}\\{inputedName}.json");
                                Tools.JsonFile.SaveJsonFile($"{DLL_DIR_SEARCH_PATH}\\{inputedName}.json", new List<DLLEditor.CodeBlock>());
                                LbxItems.Items.Add($"{inputedName}.dll");
                            }
                        }, windowTitle: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DLL_WORKING_SPACE_TITLE_ADD_TITLE"),
                        tbxToolTip: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DLL_WORKING_SPACE_TITLE_ADD_TOOLTIP")).ShowDialog();
                    };

                    this.afterEdit = (string selectedName) =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
                        {
                            if (!string.IsNullOrEmpty(inputedName) && File.Exists($"{DLL_DIR_SEARCH_PATH}\\{selectedName.Split('.')[0]}.json"))
                            {
                                if (selectedName.Split('.')[0] != inputedName)
                                {
                                    File.Move($"{DLL_DIR_SEARCH_PATH}\\{selectedName.Split('.')[0]}.json", $"{DLL_DIR_SEARCH_PATH}\\{inputedName}.json");
                                    LbxItems.Items[LbxItems.Items.IndexOf(selectedName)] = $"{inputedName}.dll";
                                }
                            }
                            if (!string.IsNullOrEmpty(inputedName) && File.Exists($"{DM.Config.CurrentWorkingDirectory}\\{selectedName}"))
                            {
                                if (selectedName.Split('.')[0] != inputedName)
                                {
                                    File.Move($"{DLL_DIR_SEARCH_PATH}\\{selectedName}", $"{DLL_DIR_SEARCH_PATH}\\{inputedName}.dll");
                                }
                            }
                        }, prevData: selectedName.Split('.')[0], windowTitle: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DLL_WORKING_SPACE_TITLE_EDIT_TITLE"),
                        tbxToolTip: DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"DLL_WORKING_SPACE_TITLE_EDIT_TOOLTIP")).ShowDialog();
                    };

                    this.afterDelete = (string selectedName) =>
                    {
                        File.Delete($"{DLL_DIR_SEARCH_PATH}\\{selectedName.Split('.')[0]}.json");
                        File.Delete($"{DM.Config.CurrentWorkingDirectory}\\{selectedName}");
                        LbxItems.Items.Remove(selectedName);
                    };

                    break;
            }
            itemToLoad.ForEach((string item) =>
            {
                LbxItems.Items.Add(item);
            });
        }

        private void LbxItems_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (LbxItems.SelectedItem != null)
            {
                afterSelect(LbxItems.SelectedItem.ToString());
                this.Close();
            }
        }

        private void EditButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            switch (editButton.Name)
            {
                case "BtnAdd":
                    afterAdd();
                    break;
                case "BtnEdit":
                    if(LbxItems.SelectedItem != null)
                        afterEdit(LbxItems.SelectedItem.ToString());
                    break;
                case "BtnDelete":
                    if (LbxItems.SelectedItem != null)
                        afterDelete(LbxItems.SelectedItem.ToString());
                    break;
            }
        }
    }

    public enum Global_ListSeleteWithEditWindow_PRESET
    {
        WORKING_SPACE,
        DLL_WORKING_SPACE
    }
}

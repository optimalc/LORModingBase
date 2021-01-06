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
            this.afterSelect = afterSelect;
            this.afterAdd = afterAdd;
            this.afterEdit = afterEdit;
            this.afterDelete = afterDelete;
            List<string> itemToLoad = new List<string>();
            switch (PRESET)
            {
                case Global_ListSeleteWithEditWindow_PRESET.WORKING_SPACE:
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
                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticDropBook, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_BookInfos.StaticDropBook = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_BookInfos.StaticDropBook, DM.Config.CurrentWorkingDirectory));
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_BookInfos.LocalizedBooks, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_BookInfos.LocalizedBooks = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_BookInfos.LocalizedBooks, DM.Config.CurrentWorkingDirectory));

                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCard, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.StaticCard = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCard, DM.Config.CurrentWorkingDirectory));
                        if (File.Exists(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCardDropTable, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.StaticCardDropTable = new DM.XmlData(DM.Config.GetStaticPathToSave(DM.EditGameData_CardInfos.StaticCardDropTable, DM.Config.CurrentWorkingDirectory));
                        if (File.Exists(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardInfos.LocalizedBattleCards, DM.Config.CurrentWorkingDirectory)))
                            DM.EditGameData_CardInfos.LocalizedBattleCards = new DM.XmlData(DM.Config.GetLocalizePathToSave(DM.EditGameData_CardInfos.LocalizedBattleCards, DM.Config.CurrentWorkingDirectory));

                        this.Close();
                    };

                    this.afterAdd = () =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose:(string inputedName) =>
                        {
                            if(!Directory.Exists($"{DirToSearch}\\{inputedName}"))
                            {
                                Directory.CreateDirectory($"{DirToSearch}\\{inputedName}");
                                LbxItems.Items.Add(inputedName);
                            }
                        }, windowTitle: "생성할 모드명을 입력", tbxToolTip: "생성시킬 모드명을 입력합니다.").ShowDialog();
                    };

                    this.afterEdit = (string selectedName) =>
                    {
                        new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
                        {
                            if (Directory.Exists($"{DirToSearch}\\{selectedName}"))
                            {
                                Directory.Move($"{DirToSearch}\\{selectedName}", $"{DirToSearch}\\{inputedName}");
                                LbxItems.Items[LbxItems.Items.IndexOf(selectedName)] = inputedName;
                            }
                        }, prevData: selectedName,  windowTitle: "편집된 모드명을 입력", tbxToolTip: "편집된 모드명을 입력합니다.").ShowDialog();
                    };

                    this.afterDelete = (string selectedName) =>
                    {
                        Directory.Delete($"{DirToSearch}\\{selectedName}", true);
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
        WORKING_SPACE
    }
}

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
    /// CustomSkinWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomSkinWindow : Window
    {
        string SKIN_DIRECTORY = "";
        public static string prevSelectedPath = DM.Config.config.LORFolderPath;

        public CustomSkinWindow()
        {
            InitializeComponent();

            SKIN_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\Char";
            if (!Directory.Exists(SKIN_DIRECTORY))
                Directory.CreateDirectory(SKIN_DIRECTORY);
            InitSqlSkins();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitSqlSkins()
        {
            SqlSkins.Children.Clear();
            Directory.GetDirectories(SKIN_DIRECTORY).ForEachSafe((string dirPath) =>
            {
                SqlSkins.Children.Add(new UC.EditSkin(dirPath, InitSqlSkins));
            });
        }

        private void SkinWindowButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnAddSkin":
                        new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
                        {
                            if (!string.IsNullOrEmpty(inputedName) && !Directory.Exists($"{SKIN_DIRECTORY}\\{inputedName}"))
                            {
                                Directory.CreateDirectory($"{SKIN_DIRECTORY}\\{inputedName}");
                                Directory.CreateDirectory($"{SKIN_DIRECTORY}\\{inputedName}\\{DS.SkinRelativePaths.ClothCustomFolder}");
                                File.Copy(DS.PROGRAM_RESOURCE_PATHS.RESOURCE_SKIN_ITEMINFO, $"{SKIN_DIRECTORY}\\{inputedName}\\{DS.SkinRelativePaths.ItemInfoPath}");
                                File.Copy(DS.PROGRAM_RESOURCE_PATHS.RESOURCE_SKIN_MODEINFO, $"{SKIN_DIRECTORY}\\{inputedName}\\{DS.SkinRelativePaths.ModInfoPath}");
                                InitSqlSkins();
                            }

                        }, windowTitle: "생성할 스킨 이름 입력",
                            tbxToolTip: "스킨 이름을 입력").ShowDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex);
            }
        }
    }
}

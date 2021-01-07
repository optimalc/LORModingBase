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
    /// ResourceWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResourceWindow : Window
    {
        string IMAGE_DIRECTORY = "";
        #region Init controls
        public ResourceWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);

            IMAGE_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\ArtWork";
            if (!Directory.Exists($"{DM.Config.CurrentWorkingDirectory}\\ArtWork"))
                Directory.CreateDirectory(IMAGE_DIRECTORY);
            InitLbxImages();
        }

        private void InitLbxImages()
        {
            LbxImages.Items.Clear();
            Directory.GetFiles(IMAGE_DIRECTORY).ForEachSafe((string imagePath) =>
            {
                int FOUND_INDEX = imagePath.IndexOf("ArtWork");
                if (FOUND_INDEX < 0) return;

                LbxImages.Items.Add(imagePath.Substring(FOUND_INDEX + 8));
            });
        } 
        #endregion

        private void EditButtonClickEvents_Images(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            switch (editButton.Name)
            {
                case "BtnAddImage":
                    Tools.Dialog.ShowOpenFileDialog(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"RESOURCE_DIALOG_TITLE"),
                        "Image Files|*.png;*.jpg", (string selectedFile) =>
                        {
                            if(File.Exists(selectedFile))
                            {
                                File.Copy(selectedFile, $"{IMAGE_DIRECTORY}\\{selectedFile.Split('\\').Last()}");
                                InitLbxImages();
                            }
                        }, DM.Config.config.LORFolderPath);
                    break;
                case "BtnDeleteImage":
                    if (LbxImages.SelectedItem != null)
                    {
                        if (File.Exists($"{IMAGE_DIRECTORY}\\{LbxImages.SelectedItem}"))
                        {
                            File.Delete($"{IMAGE_DIRECTORY}\\{LbxImages.SelectedItem}");
                            InitLbxImages();
                        }
                    }
                    break;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

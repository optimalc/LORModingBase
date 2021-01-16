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
        string SOUND_DIRECTORY = "";

        public ResourceWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);

            IMAGE_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\ArtWork";
            if (!Directory.Exists(IMAGE_DIRECTORY))
                Directory.CreateDirectory(IMAGE_DIRECTORY);
            InitLbxImages();

            SOUND_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\Sounds";
            if (!Directory.Exists(SOUND_DIRECTORY))
                Directory.CreateDirectory(SOUND_DIRECTORY);
            InitLbxSounds();
        }

        #region Controls for images
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

        private void EditButtonClickEvents_Images(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            switch (editButton.Name)
            {
                case "BtnAddImage":
                    Tools.Dialog.ShowOpenFileDialog(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"RESOURCE_DIALOG_TITLE"),
                        "Image Files|*.png;*.jpg", (string selectedFile) =>
                        {
                            if (File.Exists(selectedFile))
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
        #endregion

        #region Controls for sounds
        private void InitLbxSounds()
        {
            LbxSounds.Items.Clear();
            Directory.GetFiles(SOUND_DIRECTORY).ForEachSafe((string soundPath) =>
            {
                int FOUND_INDEX = soundPath.IndexOf("Sounds");
                if (FOUND_INDEX < 0) return;

                LbxSounds.Items.Add(soundPath.Substring(FOUND_INDEX + 7));
            });
        }

        private void EditButtonClickEvents_Sounds(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            switch (editButton.Name)
            {
                case "BtnAddSound":
                    Tools.Dialog.ShowOpenFileDialog(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"RESOURCE_DIALOG_TITLE2"),
                        "Sound Files|*.wav", (string selectedFile) =>
                        {
                            if (File.Exists(selectedFile))
                            {
                                File.Copy(selectedFile, $"{SOUND_DIRECTORY}\\{selectedFile.Split('\\').Last()}");
                                InitLbxSounds();
                            }
                        }, DM.Config.config.LORFolderPath);
                    break;
                case "BtnDeleteSound":
                    if (LbxSounds.SelectedItem != null)
                    {
                        if (File.Exists($"{SOUND_DIRECTORY}\\{LbxSounds.SelectedItem}"))
                        {
                            File.Delete($"{SOUND_DIRECTORY}\\{LbxSounds.SelectedItem}");
                            InitLbxSounds();
                        }
                    }
                    break;
            }
        } 
        #endregion

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
        public ResourceWindow()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW);
        }

        private void EditButtonClickEvents_Images(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            switch (editButton.Name)
            {
                case "BtnAddImage":
                    break;
                case "BtnDeleteImage":
                    if (LbxImages.SelectedItem != null)
                    {

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

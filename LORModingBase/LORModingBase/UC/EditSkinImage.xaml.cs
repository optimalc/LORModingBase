using System;
using System.Collections.Generic;
using System.IO;
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
    /// EditSkinImage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditSkinImage : UserControl
    {
        string imagePath = "";

        public EditSkinImage(string imageName, string imagePath)
        {
            InitializeComponent();
            this.imagePath = imagePath;
            LblSkinImage.Content = imageName;

            if (File.Exists(imagePath))
                ImgSkinImage.Source = Tools.ImageTools.ByStream(imagePath);
        }

        private void ImgSkinImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Tools.Dialog.ShowOpenFileDialog(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.GLOBAL_WINDOW, $"RESOURCE_DIALOG_TITLE"),
               "Image Files|*.png;*.jpg", (string selectedFile) =>
               {
                   List<string> PATH_SPLIT = selectedFile.Split('\\').ToList();
                   PATH_SPLIT.RemoveAt(PATH_SPLIT.Count - 1);
                   SubWindows.CustomSkinWindow.prevSelectedPath = String.Join("\\", PATH_SPLIT.ToArray());

                   if (File.Exists(imagePath))
                       File.Delete(imagePath);

                   if (File.Exists(selectedFile))
                   {
                       File.Copy(selectedFile, imagePath);
                       ImgSkinImage.Source = Tools.ImageTools.ByStream(selectedFile);
                   }
               }, SubWindows.CustomSkinWindow.prevSelectedPath);
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex);
            }
        }
    }
}

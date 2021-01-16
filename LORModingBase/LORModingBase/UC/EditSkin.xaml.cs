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
using LORModingBase.CustomExtensions;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditSkin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditSkin : UserControl
    {
        string skinRootPath = "";
        Action initStack = null;

        public EditSkin(string skinRootPath, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO);
            this.skinRootPath = skinRootPath;
            this.initStack = initStack;

            TbxSkinName.Text = skinRootPath.Split('\\').Last();
            TbxSkinName.ToolTip = skinRootPath.Split('\\').Last();
            InitSqlSkinImages();
        }

        private void InitSqlSkinImages()
        {
            SqlSkinImages.Children.Clear();
            DS.SkinRelativePaths.GetAllSkinImagePaths(skinRootPath).ForEachKeyValuePairSafe((string imageDes, string imagePath) =>
            {
                SqlSkinImages.Children.Add(new UC.EditSkinImage(imageDes, imagePath));
            });
        }

        private void TbxSkinName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string SKIN_DIRECTORY = $"{DM.Config.CurrentWorkingDirectory}\\Char";
            new SubWindows.Global_InputOneColumnData(null, afterClose: (string inputedName) =>
            {
                if (!string.IsNullOrEmpty(inputedName) && !Directory.Exists($"{SKIN_DIRECTORY}\\{inputedName}"))
                {
                    Directory.Move(skinRootPath, $"{SKIN_DIRECTORY}\\{inputedName}");
                    initStack();
                }

            }, prevData: TbxSkinName.Text, windowTitle: "변경시킬 스킨명 입력",
                    tbxToolTip: "변경시킬 스킨명 입력").ShowDialog();
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnDelete":
                    if(Directory.Exists(skinRootPath))
                        Directory.Delete(skinRootPath, true);
                    initStack();
                    break;
            }
        }
    }
}

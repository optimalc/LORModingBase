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

namespace LORModingBase.UC
{
    /// <summary>
    /// EditStage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditStage : UserControl
    {
        DM.XmlDataNode innerStageNode = null;
        Action stackInitFunc = null;

        public EditStage(DM.XmlDataNode stageNode, Action stackInitFunc)
        {
            InitializeComponent();
            this.innerStageNode = stageNode;
            this.stackInitFunc = stackInitFunc;
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerStageNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxStageName":
                    break;
                case "TbxStageUniqueID":
                    break;
            }
        }

        /// <summary>
        /// Button events that need search window
        /// </summary>
        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnStage":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnFloor":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnInvitation":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnAddWave":
                    break;
            }
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnCondition":
                    break;
                case "BtnMapInfo":
                    break;
                case "BtnCopyStage":
                    break;
                case "BtnDelete":
                    break;
            }
        }
    }
}

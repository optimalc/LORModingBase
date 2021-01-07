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
    /// EditWave.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditWave : UserControl
    {
        DM.XmlDataNode waveNode;
        Action stackInitFunc = null;

        public EditWave(DM.XmlDataNode waveNode, Action stackInitFunc)
        {
            InitializeComponent();
            this.waveNode = waveNode;
            this.stackInitFunc = stackInitFunc;
        }

        /// <summary>
        /// Button events that need search window
        /// </summary>
        private void SelectItemButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnFormation":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnUnit1":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnUnit3":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnUnit4":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnUnit5":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {

                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
            }
        }
    }
}

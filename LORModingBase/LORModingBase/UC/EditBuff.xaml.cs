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
    /// EditBuff.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditBuff : UserControl
    {
        DM.XmlDataNode innerBuffNode = null;
        Action initStack = null;

        public EditBuff(DM.XmlDataNode innerBuffNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.BUFF_INFO);
            this.innerBuffNode = innerBuffNode;
            this.initStack = initStack;

            TbxBuffID.Text = innerBuffNode.GetAttributesSafe("ID");
            TbxBuffName.Text = innerBuffNode.GetInnerTextByPath("Name");
            TbxBuffDes.Text = innerBuffNode.GetInnerTextByPath("Desc");
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerBuffNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxBuffID":
                    innerBuffNode.attribute["ID"] = tbx.Text;
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                    break;
                case "TbxBuffName":
                    innerBuffNode.SetXmlInfoByPath("Name", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                    break;
                case "TbxBuffDes":
                    innerBuffNode.SetXmlInfoByPath("Desc", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }

        /// <summary>
        /// Right menu button events
        /// </summary>
        private void RightMenuButtonEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnMakeDLL":
                    if (string.IsNullOrEmpty(DM.Config.config.DLLCompilerPath))
                    {
                        Tools.MessageBoxTools.ShowErrorMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"DLLCompilerPathError2"));
                        return;
                    }
                    //new DLLEditor.DLLEditorMainWindow($"customCardAbility_{TbxAbilityID.Text}",
                    //    new List<string>() { $"BASE_CARD_ABILITY_CODES/BASE_CARD_ABILITY,{TbxAbilityID.Text}" }).ShowDialog();
                    break;
                case "BtnCopyBuff":
                    DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList", (DM.XmlDataNode effectTextList) => {
                        effectTextList.subNodes.Add(
                            innerBuffNode.Copy());
                    });
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                    break;
                case "BtnDelete":
                    DM.EditGameData_Buff.LocalizedBuff.rootDataNode.ActionXmlDataNodesByPath("effectTextList", (DM.XmlDataNode effectTextList) => {
                        effectTextList.subNodes.Remove(innerBuffNode);
                    });
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_BUFF_DESC);
                    break;
            }
        }
    }
}

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
        Action initStack = null;

        public EditStage(DM.XmlDataNode stageNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.STAGE_INFO);
            this.innerStageNode = stageNode;
            this.initStack = initStack;

            TbxStageName.Text = innerStageNode.GetInnerTextByPath("Name");
            TbxStageUniqueID.Text = innerStageNode.GetAttributesSafe("id");

            innerStageNode.ActionIfInnertTextIsNotNullOrEmpty("StoryType", (string innerText) =>
            {
                BtnStage.ToolTip = innerText;

                LblStage.Content = innerText;
                BtnStage.Content = "          ";
            });
            innerStageNode.ActionIfInnertTextIsNotNullOrEmpty("FloorNum", (string innerText) =>
            {
                BtnFloor.ToolTip = innerText;

                LblFloor.Content = innerText;
                BtnFloor.Content = "          ";
            });

            if (innerStageNode.GetXmlDataNodesByPath("Invitation/Book").Count > 0)
            {
                string extraInfo = "";
                innerStageNode.ActionXmlDataNodesByPath("Invitation/Book", (DM.XmlDataNode xmlDataNode) =>
                {
                    extraInfo += $"{xmlDataNode.GetInnerTextSafe()}/";
                });
                extraInfo = extraInfo.TrimEnd('/');

                LblInvitation.Content = extraInfo;
                BtnInvitation.ToolTip = extraInfo;
                BtnInvitation.Content = "          ";
            }

            if (innerStageNode.GetXmlDataNodesByPath("Condition/Stage").Count > 0)
            {
                string extraInfo = "";
                innerStageNode.ActionXmlDataNodesByPath("Condition/Stage", (DM.XmlDataNode xmlDataNode) =>
                {
                    extraInfo += $"{xmlDataNode.GetInnerTextSafe()}\n";
                });
                extraInfo = extraInfo.TrimEnd('\n');

                BtnCondition.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesCondition.png");
                BtnCondition.ToolTip = $"선택된 컨디션\n{extraInfo}";
            }

            string MAP_INFO = innerStageNode.GetInnerTextByPath("MapInfo");
            if(!string.IsNullOrEmpty(MAP_INFO))
            {
                BtnMapInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesMapInfo.png");
                BtnMapInfo.ToolTip = innerStageNode.GetInnerTextByPath("MapInfo");
            }
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
                    innerStageNode.SetXmlInfoByPath("Name", tbx.Text);
                    if (DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                    attributeToCheck: new Dictionary<string, string>() { { "ID", innerStageNode.GetAttributesSafe("id") } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", innerStageNode.GetAttributesSafe("id") } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].innerText = tbx.Text;
                        }
                    }
                    else
                    {
                        DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.subNodes.Add(DM.EditGameData_StageInfo.MakeNewStageNameBase(
                                innerStageNode.GetAttributesSafe("id"),
                                innerStageNode.GetInnerTextByPath("Name")));
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_STAGE_NAME);
                    break;
                case "TbxStageUniqueID":
                    string PREV_STAGE_ID = innerStageNode.attribute["id"];
                    #region Stage info localizing ID refrect
                    if (DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_STAGE_ID } }))
                    {
                        List<DM.XmlDataNode> foundXmlDataNode = DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Name",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_STAGE_ID } });

                        if (foundXmlDataNode.Count > 0)
                        {
                            foundXmlDataNode[0].attribute["ID"] = tbx.Text;
                        }
                    }
                    #endregion
                    innerStageNode.attribute["id"] = tbx.Text;
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    MainWindow.mainWindow.UpdateDebugInfo();
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
                    DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.subNodes.Add(innerStageNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
                case "BtnDelete":
                    if (DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.CheckIfGivenPathWithXmlInfoExists("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerStageNode.GetAttributesSafe("id") } }))
                        DM.EditGameData_StageInfo.LocalizedStageName.rootDataNode.RemoveXmlInfosByPath("Name",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerStageNode.GetAttributesSafe("id") } });
                    DM.EditGameData_StageInfo.StaticStageInfo.rootDataNode.subNodes.Remove(innerStageNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
            }
        }
    }
}

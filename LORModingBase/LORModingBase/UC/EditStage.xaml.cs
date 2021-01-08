using LORModingBase.CustomExtensions;
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

        #region Init controls
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
                string EPISODE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"EPISODE");
                BtnStage.ToolTip = $"{EPISODE_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStoryType(innerText)}";

                LblStage.Content = $"{EPISODE_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStoryType(innerText)}"; ;
                BtnStage.Content = "          ";
            });
            innerStageNode.ActionIfInnertTextIsNotNullOrEmpty("FloorNum", (string innerText) =>
            {
                string FLOOR_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"AVALIABLE_FLOOR");
                BtnFloor.ToolTip = $"{FLOOR_WORD} : {innerText}";

                LblFloor.Content = $"{FLOOR_WORD} : {innerText}";
                BtnFloor.Content = "          ";
            });

            if (innerStageNode.GetXmlDataNodesByPath("Invitation/Book").Count > 0)
            {
                string extraInfo = "";
                innerStageNode.ActionXmlDataNodesByPath("Invitation/Book", (DM.XmlDataNode xmlDataNode) =>
                {
                    extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(xmlDataNode.GetInnerTextSafe())}/";
                });
                extraInfo = extraInfo.TrimEnd('/');

                string INVITATION_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"INVITATION_BOOK");
                LblInvitation.Content = $"{INVITATION_WORD} : {extraInfo}";
                BtnInvitation.ToolTip = $"{INVITATION_WORD} : {extraInfo}";
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
                BtnCondition.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"STAGE_CONDITION")}\n{extraInfo}";
            }

            string MAP_INFO = innerStageNode.GetInnerTextByPath("MapInfo");
            if (!string.IsNullOrEmpty(MAP_INFO))
            {
                BtnMapInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesMapInfo.png");
                BtnMapInfo.ToolTip = innerStageNode.GetInnerTextByPath("MapInfo");
            }

            InitSqlWaves();
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
        }

        /// <summary>
        /// Init eave stack panel
        /// </summary>
        private void InitSqlWaves()
        {
            SqlWaves.Children.Clear();
            innerStageNode.GetXmlDataNodesByPath("Wave").ForEachSafe((DM.XmlDataNode waveNode) =>
            {
                SqlWaves.Children.Add(new EditWave(waveNode, innerStageNode, InitSqlWaves));
            });
        } 
        #endregion


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
                    MainWindow.mainWindow.UpdateDebugInfo();
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
                        DM.GameInfos.staticInfos["StageInfo"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Stage",
                            attributeToCheck: new Dictionary<string, string>() { { "id", selectedItem } }).ActionOneItemSafe((DM.XmlDataNode stageNode) =>
                            {
                                innerStageNode.SetXmlInfoByPath("Chapter", stageNode.GetInnerTextByPath("Chapter"));
                                innerStageNode.SetXmlInfoByPath("StoryType", stageNode.GetInnerTextByPath("StoryType"));

                                innerStageNode.RemoveXmlInfosByPath("Story");
                                DM.XmlDataNode copyedStageNode = stageNode.Copy();
                                innerStageNode.subNodes.AddRange(copyedStageNode.GetXmlDataNodesByPath("Story"));

                                string EPISODE_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"EPISODE");
                                BtnStage.ToolTip = $"{EPISODE_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(selectedItem)}";

                                LblStage.Content = $"{EPISODE_WORD} : {DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForStage(selectedItem)}"; ;
                                BtnStage.Content = "          ";
                            });
                        MainWindow.mainWindow.UpdateDebugInfo();
                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.EPISODE).ShowDialog();
                    break;
                case "BtnFloor":
                    new SubWindows.Global_ListSeleteWindow((string floorNumStr) => {
                        innerStageNode.SetXmlInfoByPath("FloorNum", floorNumStr);
                        string FLOOR_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"AVALIABLE_FLOOR");
                        BtnFloor.ToolTip = $"{FLOOR_WORD} : {floorNumStr}";

                        LblFloor.Content = $"{FLOOR_WORD} : {floorNumStr}";
                        BtnFloor.Content = "          ";

                        MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    }, SubWindows.Global_ListSeleteWindow_PRESET.FLOORS).ShowDialog();
                    break;
                case "BtnInvitation":
                    List<string> selectedDropBooks = new List<string>();
                    innerStageNode.ActionXmlDataNodesByPath("Invitation/Book", (DM.XmlDataNode xmlDataNode) =>
                    {
                        selectedDropBooks.Add(DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(xmlDataNode.innerText));
                    });

                   new SubWindows.Global_AddItemToListWindow((string addedDropBookItemID) =>
                    {
                        innerStageNode.ActionXmlDataNodesByPath("Invitation", (DM.XmlDataNode invitationNode) =>
                        {
                            invitationNode.AddXmlInfoByPath("Book", addedDropBookItemID);
                        });
                    }, (string deletedDropBookItemID) => {
                        innerStageNode.ActionXmlDataNodesByPath("Invitation", (DM.XmlDataNode invitationNode) =>
                        {
                            invitationNode.RemoveXmlInfosByPath("Book", deletedDropBookItemID);
                        });
                    }, selectedDropBooks, SubWindows.AddItemToListWindow_PRESET.DROP_BOOK).ShowDialog();

                    if (innerStageNode.GetXmlDataNodesByPath("Invitation/Book").Count > 0)
                    {
                        string extraInfo = "";
                        innerStageNode.ActionXmlDataNodesByPath("Invitation/Book", (DM.XmlDataNode xmlDataNode) =>
                        {
                            extraInfo += $"{DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForDropBook(xmlDataNode.GetInnerTextSafe())}/";
                        });
                        extraInfo = extraInfo.TrimEnd('/');

                        string INVITATION_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"INVITATION_BOOK");
                        LblInvitation.Content = $"{INVITATION_WORD} : {extraInfo}";
                        BtnInvitation.ToolTip = $"{INVITATION_WORD} : {extraInfo}";
                        BtnInvitation.Content = "          ";
                    }
                    else
                    {
                        LblInvitation.Content = "";
                        BtnInvitation.ToolTip = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"%BtnInvitation_ToolTip%").Split('$')[0];
                        BtnInvitation.Content = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"%BtnInvitation%").Split('$')[0];
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
                case "BtnAddWave":
                    innerStageNode.subNodes.Add(DM.EditGameData_StageInfo.MakeNewWaveInfoBase());
                    InitSqlWaves();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
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
                    List<string> selectedConditions = new List<string>();
                    innerStageNode.ActionXmlDataNodesByPath("Condition/Stage", (DM.XmlDataNode xmlDataNode) =>
                    {
                        selectedConditions.Add(xmlDataNode.innerText);
                    });

                    new SubWindows.Global_AddItemToListWindow((string addedStageID) =>
                    {
                        innerStageNode.AddXmlInfoByPath("Condition/Stage", addedStageID);
                    }, (string deletedStageID) => {
                        innerStageNode.RemoveXmlInfosByPath("Condition/Stage", deletedStageID);
                        List<DM.XmlDataNode> conditonNode = innerStageNode.GetXmlDataNodesByPath("Condition");
                        if(conditonNode.Count > 0)
                        {
                            DM.XmlDataNode CONDITION_NODE = conditonNode[0];
                            if (CONDITION_NODE.GetXmlDataNodesByPath("Stage").Count <= 0)
                                innerStageNode.RemoveXmlInfosByPath("Condition");
                        }
                    }, selectedConditions, SubWindows.AddItemToListWindow_PRESET.STAGES).ShowDialog();

                    if (innerStageNode.GetXmlDataNodesByPath("Condition/Stage").Count > 0)
                    {
                        string extraInfo = "";
                        innerStageNode.ActionXmlDataNodesByPath("Condition/Stage", (DM.XmlDataNode xmlDataNode) =>
                        {
                            extraInfo += $"{xmlDataNode.GetInnerTextSafe()}\n";
                        });
                        extraInfo = extraInfo.TrimEnd('\n');

                        BtnCondition.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesCondition.png");
                        BtnCondition.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"STAGE_CONDITION")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{extraInfo}";
                    }
                    else
                    {
                        BtnCondition.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoCondition.png");
                        BtnCondition.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"%BtnCondition_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
                case "BtnMapInfo":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerStageNode.SetXmlInfoByPathAndEmptyWillRemove("MapInfo", selectedItem);   
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.MAP_INFO).ShowDialog();

                    string MAP_INFO = innerStageNode.GetInnerTextByPath("MapInfo");
                    if (!string.IsNullOrEmpty(MAP_INFO))
                    {
                        BtnMapInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconYesMapInfo.png");
                        BtnMapInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"%BtnMapInfo_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"Inputted")})\n{innerStageNode.GetInnerTextByPath("MapInfo")}";
                    }
                    else
                    {
                        BtnMapInfo.Background = Tools.ColorTools.GetImageBrushFromPath(this, "../Resources/IconNoMapInfo.png");
                        BtnMapInfo.ToolTip = $"{DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"%BtnMapInfo_ToolTip%")} ({DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.BOOK_INFO, $"NotInputted")})";

                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
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

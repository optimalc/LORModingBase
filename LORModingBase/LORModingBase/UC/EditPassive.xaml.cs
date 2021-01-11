using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditPassive.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditPassive : UserControl
    {
        DM.XmlDataNode innerPassiveNode = null;
        Action initStack = null;

        #region Init controls
        public EditPassive(DM.XmlDataNode innerPassiveNode, Action initStack)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.PASSIVE_INFO);

            this.initStack = initStack;
            this.innerPassiveNode = innerPassiveNode;

            switch (innerPassiveNode.GetInnerTextByPath("Rarity"))
            {
                case "Common":
                    ChangeRarityButtonEvents(BtnRarity_Common, null);
                    break;
                case "Uncommon":
                    ChangeRarityButtonEvents(BtnRarity_Uncommon, null);
                    break;
                case "Rare":
                    ChangeRarityButtonEvents(BtnRarity_Rare, null);
                    break;
                case "Unique":
                    ChangeRarityButtonEvents(BtnRarity_Unique, null);
                    break;
            }

            TbxCost.Text = innerPassiveNode.GetInnerTextByPath("Cost");
            TbxPassiveID.Text = innerPassiveNode.GetAttributesSafe("ID");

            DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.ActionXmlDataNodesByAttributeWithPath("PassiveDesc", "ID", innerPassiveNode.GetAttributesSafe("ID"),
                (DM.XmlDataNode passiveDescNode) => {
                    TbxPassiveName.Text = passiveDescNode.GetInnerTextByPath("Name");
                    TbxPassiveDes.Text = passiveDescNode.GetInnerTextByPath("Desc");
                });
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
        }

        /// <summary>
        /// Change rarity button events
        /// </summary>
        private void ChangeRarityButtonEvents(object sender, RoutedEventArgs e)
        {
            Button rarityButton = sender as Button;

            BtnRarity_Common.Background = null;
            BtnRarity_Uncommon.Background = null;
            BtnRarity_Rare.Background = null;
            BtnRarity_Unique.Background = null;

            rarityButton.Background = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
            WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr(rarityButton.Tag.ToString());
            innerPassiveNode.SetXmlInfoByPath("Rarity", rarityButton.Name.Split('_').Last());
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
        } 
        #endregion

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerPassiveNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxCost":
                    innerPassiveNode.SetXmlInfoByPath("Cost", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
                    break;
                case "TbxPassiveID":
                    string PREV_PASSIVE_ID = innerPassiveNode.attribute["ID"];
                    #region Books info localizing ID refrect
                    if (DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.CheckIfGivenPathWithXmlInfoExists("PassiveDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_PASSIVE_ID } }))
                    {
                        List<DM.XmlDataNode> foundPassiveDescsForID = DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", PREV_PASSIVE_ID } });

                        if (foundPassiveDescsForID.Count > 0)
                        {
                            foundPassiveDescsForID[0].attribute["ID"] = tbx.Text;
                        }
                    }
                    #endregion
                    innerPassiveNode.attribute["ID"] = tbx.Text;
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
                    break;
                case "TbxPassiveName":
                    if (!DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.CheckIfGivenPathWithXmlInfoExists("PassiveDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } } )
                        && !string.IsNullOrEmpty(innerPassiveNode.GetAttributesSafe("ID")))
                    {
                        DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.subNodes.Add(
                            DM.EditGameData_PassiveInfo.MakeNewPassiveDescBase(TbxPassiveID.Text, TbxPassiveName.Text, TbxPassiveDes.Text));
                    }

                    List<DM.XmlDataNode> foundPassiveDescsForName = DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                          attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } });

                    if (foundPassiveDescsForName.Count > 0)
                    {
                        foundPassiveDescsForName[0].SetXmlInfoByPath("Name", tbx.Text);
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_PASSIVE_DESC);
                    break;
                case "TbxPassiveDes":
                    if (!DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.CheckIfGivenPathWithXmlInfoExists("PassiveDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } })
                        && !string.IsNullOrEmpty(innerPassiveNode.GetAttributesSafe("ID")))
                    {
                        DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.subNodes.Add(
                            DM.EditGameData_PassiveInfo.MakeNewPassiveDescBase(TbxPassiveID.Text, TbxPassiveName.Text, TbxPassiveDes.Text));
                    }

                    List<DM.XmlDataNode> foundPassiveDescsForDesc = DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                          attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } });

                    if (foundPassiveDescsForDesc.Count > 0)
                    {
                        foundPassiveDescsForDesc[0].SetXmlInfoByPath("Desc", tbx.Text);
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.LOCALIZED_PASSIVE_DESC);
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
                case "BtnMakeDLL":
                    if (string.IsNullOrEmpty(DM.Config.config.DLLCompilerPath))
                    {
                        Tools.MessageBoxTools.ShowErrorMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.OPTION, $"DLLCompilerPathError2"));
                        return;
                    }
                    new DLLEditor.DLLEditorMainWindow($"customPassive_{TbxPassiveID.Text}", 
                        new List<string>() { $"BASE_PASSIVE_CODES/BASE_KEY_CARD_PASSIVE,{TbxPassiveID.Text}" }).ShowDialog();
                    break;
                case "BtnCopyPassive":
                    DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.subNodes.Add(innerPassiveNode.Copy());
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
                    break;

                case "BtnDelete":
                    if (DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.CheckIfGivenPathWithXmlInfoExists("PassiveDesc",
                        attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } }))
                    {
                        if (DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Passive", 
                            attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } }).Count == 1)
                            DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode.RemoveXmlInfosByPath("PassiveDesc",
                               attributeToCheck: new Dictionary<string, string>() { { "ID", innerPassiveNode.GetAttributesSafe("ID") } });
                    }

                    DM.EditGameData_PassiveInfo.StaticPassiveList.rootDataNode.subNodes.Remove(innerPassiveNode);
                    initStack();
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_PASSIVE_INTO);
                    break;
            }
        }
    }
}

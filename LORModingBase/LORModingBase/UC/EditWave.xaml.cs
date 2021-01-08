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
        DM.XmlDataNode stageNode;
        Action stackInitFunc = null;

        public EditWave(DM.XmlDataNode waveNode, DM.XmlDataNode stageNode, Action stackInitFunc)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.STAGE_INFO);
            this.waveNode = waveNode;
            this.stageNode = stageNode;
            this.stackInitFunc = stackInitFunc;

            waveNode.ActionIfInnertTextIsNotNullOrEmpty("Formation", (string innerText) =>
            {
                string FORMATION_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"FORMATION");
                BtnFormation.ToolTip = $"{FORMATION_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForFormation(innerText)}";

                LblFormation.Content = $"{FORMATION_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForFormation(innerText)}";
                BtnFormation.Content = "          ";
            });

            List<DM.XmlDataNode> unitNodes = waveNode.GetXmlDataNodesByPathWithXmlInfo("Unit");
            string unitStr = "";
            if(unitNodes.Count > 0)
            {
                unitNodes.ForEach((DM.XmlDataNode unitNode) =>
                {
                    unitStr += $" {DM.LocalizedGameDescriptions.GetDescriptionForEnemy(unitNode.innerText)}:{unitNode.innerText} /";
                });
                unitStr = unitStr.Trim('/');
                if(!string.IsNullOrEmpty(unitStr.Trim()))
                {
                    BtnUnits.ToolTip = unitStr;

                    LblUnits.Content = unitStr;
                    BtnUnits.Content = "          ";
                }
            }
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
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
                        waveNode.SetXmlInfoByPath("Formation", selectedItem);

                        string FORMATION_WORD = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.STAGE_INFO, $"FORMATION");
                        BtnFormation.ToolTip = $"{FORMATION_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForFormation(selectedItem)}";

                        LblFormation.Content = $"{FORMATION_WORD} : {DM.LocalizedGameDescriptions.GetDescriptionForFormation(selectedItem)}";
                        BtnFormation.Content = "          ";
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.FORMATION).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
                case "BtnUnits":
                    List<string> selectedEnemies= new List<string>();
                    waveNode.ActionXmlDataNodesByPath("Unit", (DM.XmlDataNode unitNode) =>
                    {
                        selectedEnemies.Add($"{DM.LocalizedGameDescriptions.GetDescriptionForEnemy(unitNode.innerText)}:{unitNode.innerText}");
                    });

                    new SubWindows.Global_AddItemToListWindow((string addedEnemyID) =>
                    {
                        waveNode.AddXmlInfoByPath("Unit", addedEnemyID);
                    }, (string deletedEnemyID) => {
                        waveNode.RemoveXmlInfosByPath("Unit", deletedEnemyID, deleteOnce: true);
                    }, selectedEnemies, SubWindows.AddItemToListWindow_PRESET.ENEMIES).ShowDialog();

                    List<DM.XmlDataNode> unitNodes = waveNode.GetXmlDataNodesByPathWithXmlInfo("Unit");
                    string unitStr = "";
                    if (unitNodes.Count > 0)
                    {
                        unitNodes.ForEach((DM.XmlDataNode unitNode) =>
                        {
                            unitStr += $" {DM.LocalizedGameDescriptions.GetDescriptionForEnemy(unitNode.innerText)}:{unitNode.innerText} /";
                        });
                        unitStr = unitStr.Trim('/');
                        if (!string.IsNullOrEmpty(unitStr.Trim()))
                        {
                            BtnUnits.ToolTip = unitStr;

                            LblUnits.Content = unitStr;
                            BtnUnits.Content = "          ";
                        }
                    }
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
            }
        }

        private void NomalButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Name)
            {
                case "BtnDeleteWave":
                    stageNode.subNodes.Remove(waveNode);
                    MainWindow.mainWindow.UpdateDebugInfo();
                    stackInitFunc();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
            }
        }

        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (waveNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxAvailableUnit":
                    waveNode.SetXmlInfoByPathAndEmptyWillRemove("AvailableUnit", tbx.Text);
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_STAGE_INFO);
                    break;
            }
        }
    }
}

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
using LORModingBase.CustomExtensions;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditDice.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditDice : UserControl
    {
        DM.XmlDataNode innerCardCardNode = null;
        DM.XmlDataNode innerBehaviourNode = null;
        Action stackInitFunc = null;

        public EditDice(DM.XmlDataNode innerCardCardNode, DM.XmlDataNode innerBehaviourNode, Action stackInitFunc)
        {
            this.innerCardCardNode = innerCardCardNode;
            this.stackInitFunc = stackInitFunc;
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.CARD_INFO);

            TbxMinDice_Min.Text = innerBehaviourNode.GetAttributesSafe("Min");
            TbxMaxDice_Dice.Text = innerBehaviourNode.GetAttributesSafe("Dice");
            TbxMotion.Text = innerBehaviourNode.GetAttributesSafe("Motion");
            TbxEffectRes.Text = innerBehaviourNode.GetAttributesSafe("EffectRes");
            TbxActionScript.Text = innerBehaviourNode.GetAttributesSafe("ActionScript");
            this.innerBehaviourNode = innerBehaviourNode;

            BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/icon_{innerBehaviourNode.attribute["Type"] }_{innerBehaviourNode.attribute["Detail"]}.png");
            UpdateEffectGrid();

            GldInfo.Visibility = Visibility.Visible;
            GldChangeAttackType.Visibility = Visibility.Collapsed;
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }

        private void UpdateEffectGrid()
        {
            string DICE_SCRIPT = $"{DM.LocalizedGameDescriptions.GetDescriptionForCardPassive(innerBehaviourNode.attribute["Script"])}:{innerBehaviourNode.attribute["Script"]}";
            LblEffect.Content = DICE_SCRIPT;
            LblEffect.ToolTip = DICE_SCRIPT;
            RectAllInfo.Visibility = Visibility.Collapsed;
            RectDiceOnly.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(DICE_SCRIPT))
            {
                RectDiceOnly.Visibility = Visibility.Visible;
                GldEffect.Visibility = Visibility.Collapsed;
            }   
            else
            {
                RectAllInfo.Visibility = Visibility.Visible;
                GldEffect.Visibility = Visibility.Visible;
            }
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }

        private void NomalButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch(btn.Name)
            {
                case "BtnDiceType":
                    GldInfo.Visibility = Visibility.Collapsed;
                    GldChangeAttackType.Visibility = Visibility.Visible;
                    break;
                case "BtnEffect":
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedItem) =>
                    {
                        innerBehaviourNode.attribute["Script"] = selectedItem;
                        UpdateEffectGrid();
                        MainWindow.mainWindow.UpdateDebugInfo();
                    }, SubWindows.InputInfoWithSearchWindow_PRESET.DICE_ABILITES).ShowDialog();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
                case "BtnDeleteDice":
                    innerCardCardNode.ActionXmlDataNodesByPath("BehaviourList", (DM.XmlDataNode behaviourListNode) =>
                    {
                        behaviourListNode.subNodes.Remove(innerBehaviourNode);
                        MainWindow.mainWindow.UpdateDebugInfo();
                        stackInitFunc();
                    });
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
            }
        }

        private void DiceTypeButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            List<string> SPLIT_NAME = btn.Name.Split('_').ToList();
            if (SPLIT_NAME.Count >= 3)
            {
                innerBehaviourNode.attribute["Type"] = SPLIT_NAME[1];
                innerBehaviourNode.attribute["Detail"] = SPLIT_NAME[2];

                BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/icon_{innerBehaviourNode.attribute["Type"] }_{innerBehaviourNode.attribute["Detail"]}.png");

                #region Motion Update
                switch(innerBehaviourNode.GetAttributesSafe("Detail"))
                {
                    case "Guard": innerBehaviourNode.attribute["Motion"] = "G"; break;
                    case "Evasion": innerBehaviourNode.attribute["Motion"] = "E"; break;

                    case "Slash": innerBehaviourNode.attribute["Motion"] = "J"; break;
                    case "Penetrate": innerBehaviourNode.attribute["Motion"] = "Z"; break;
                    case "Hit": innerBehaviourNode.attribute["Motion"] = "H"; break;
                    default: innerBehaviourNode.attribute["Motion"] = "H"; break;
                }
                innerBehaviourNode.attribute["EffectRes"] = "";
                TbxMotion.Text = innerBehaviourNode.GetAttributesSafe("Motion");
                TbxEffectRes.Text = "";
                #endregion
            }

            GldInfo.Visibility = Visibility.Visible;
            GldChangeAttackType.Visibility = Visibility.Collapsed;
            MainWindow.mainWindow.UpdateDebugInfo();
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }

        /// <summary>
        /// Reflect text chagnes in TextBox
        /// </summary>
        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerBehaviourNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                default:
                    List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                    if (SPLIT_NAME.Count == 2)
                        innerBehaviourNode.attribute[SPLIT_NAME.Last()] = tbx.Text;
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }


        private void TbxMotion_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string DICE_TYPE = innerBehaviourNode.GetAttributesSafe("Type");
            string DICE_DETAIL = innerBehaviourNode.GetAttributesSafe("Detail");

            List<DM.XmlDataNode> behaviorNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card/BehaviourList/Behaviour",
                attributeToCheck: new Dictionary<string, string>() { { "Type", DICE_TYPE }, { "Detail", DICE_DETAIL } });
            List<string> motions = new List<string>();
            behaviorNodes.ForEach((DM.XmlDataNode motion) =>
            {
                string BEHAVIOR_MOTION = motion.GetAttributesSafe("Motion");
                if (!string.IsNullOrEmpty(BEHAVIOR_MOTION))
                    motions.Add(BEHAVIOR_MOTION);
            });
            new SubWindows.Global_ListSeleteWindow((string selectedMotion) =>
            {
                TbxMotion.Text = selectedMotion;
                TbxMotion.ToolTip = selectedMotion;
                innerBehaviourNode.attribute["Motion"] = selectedMotion;

                MainWindow.mainWindow.UpdateDebugInfo();
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
            }, motions.GetUniqueList()).ShowDialog();
        }

        private void TbxEffectRes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new SubWindows.Global_InputInfoWithSearchWindow((string selectedEffectRes) =>
            {
                TbxEffectRes.Text = selectedEffectRes;
                TbxEffectRes.ToolTip = selectedEffectRes;
                innerBehaviourNode.attribute["EffectRes"] = selectedEffectRes;
                MainWindow.mainWindow.UpdateDebugInfo();
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
            }, SubWindows.InputInfoWithSearchWindow_PRESET.DICE_EFFECT_RESES).ShowDialog();
        }

        private void TbxActionScript_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<DM.XmlDataNode> behaviorNodes = DM.GameInfos.staticInfos["Card"].rootDataNode.GetXmlDataNodesByPathWithXmlInfo("Card/BehaviourList/Behaviour");
            List<string> actionScripts = new List<string>();
            behaviorNodes.ForEach((DM.XmlDataNode effectRes) =>
            {
                string BEHAVIOR_ACTION_SCRIPT = effectRes.GetAttributesSafe("ActionScript");
                if (!string.IsNullOrEmpty(BEHAVIOR_ACTION_SCRIPT))
                    actionScripts.Add(BEHAVIOR_ACTION_SCRIPT);
            });
            new SubWindows.Global_ListSeleteWindow((string selectedActionScript) =>
            {
                TbxActionScript.Text = selectedActionScript;
                TbxActionScript.ToolTip = selectedActionScript;
                innerBehaviourNode.attribute["ActionScript"] = selectedActionScript;
                MainWindow.mainWindow.UpdateDebugInfo();
                MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
            }, actionScripts.GetUniqueList()).ShowDialog();
        }


        /// <summary>
        /// Mouse right button select -> Handly input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlyInputToTextBox(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (innerBehaviourNode == null)
                    return;
                TextBox tbx = sender as TextBox;

                new SubWindows.Global_InputOneColumnData((string inputedData) =>
                {
                    tbx.Text = inputedData;
                    tbx.ToolTip = inputedData;
                    innerBehaviourNode.attribute[tbx.Name.Replace("Tbx","")] = inputedData;
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
                }, tbx.Text).ShowDialog();
            }
        }
    }
}

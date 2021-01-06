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
            this.innerBehaviourNode = innerBehaviourNode;
            this.stackInitFunc = stackInitFunc;
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.CARD_INFO);

            TbxMinDice_Min.Text = innerBehaviourNode.GetAttributesSafe("Min");
            TbxMaxDice_Dice.Text = innerBehaviourNode.GetAttributesSafe("Dice");
            BtnDiceType.Background = Tools.ColorTools.GetImageBrushFromPath(this, $"../Resources/icon_{innerBehaviourNode.attribute["Type"] }_{innerBehaviourNode.attribute["Detail"]}.png");
            UpdateEffectGrid();

            GldInfo.Visibility = Visibility.Visible;
            GldChangeAttackType.Visibility = Visibility.Collapsed;
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
                    break;
                case "BtnDeleteDice":
                    innerCardCardNode.ActionXmlDataNodesByPath("BehaviourList", (DM.XmlDataNode behaviourListNode) =>
                    {
                        behaviourListNode.subNodes.Remove(innerBehaviourNode);
                        MainWindow.mainWindow.UpdateDebugInfo();
                        stackInitFunc();
                    });
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
            }

            GldInfo.Visibility = Visibility.Visible;
            GldChangeAttackType.Visibility = Visibility.Collapsed;
            MainWindow.mainWindow.UpdateDebugInfo();
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
                    break;
            }
            MainWindow.mainWindow.UpdateDebugInfo();
        }
    }
}

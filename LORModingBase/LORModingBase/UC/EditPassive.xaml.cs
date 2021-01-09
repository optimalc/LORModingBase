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
            this.innerPassiveNode = innerPassiveNode;
            this.initStack = initStack;

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
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
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
                    break;
                case "LblPassiveID":
                    break;
                case "TbxPassiveName":
                    break;
                case "TbxPassiveDes":
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
                case "BtnCopyPassive":
                    break;

                case "BtnDelete":
                    break;
            }

            initStack();
            MainWindow.mainWindow.UpdateDebugInfo();
            MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_CARD);
        }
    }
}

using System.Windows.Controls;

namespace LORModingBase.UC
{
    /// <summary>
    /// EditEmotionReward.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EditEmotionReward : UserControl
    {
        DM.XmlDataNode dropItemNode;

        public EditEmotionReward(DM.XmlDataNode dropItemNode, string emotionLevel)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.ENEMY_INFO);
            this.dropItemNode = dropItemNode;

            LblEmotionLevel.Content = emotionLevel;

            LblBookName.Content = dropItemNode.innerText;
            LblBookName.ToolTip = dropItemNode.innerText;

            TbxBookCount.Text = dropItemNode.GetAttributesSafe("Prob");
        }

        private void ReflectTextChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (dropItemNode == null)
                return;

            TextBox tbx = sender as TextBox;
            switch (tbx.Name)
            {
                case "TbxBookCount":
                    dropItemNode.attribute["Prob"] = tbx.Text;
                    MainWindow.mainWindow.UpdateDebugInfo();
                    MainWindow.mainWindow.ChangeDebugLocation(MainWindow.DEBUG_LOCATION.STATIC_ENEMY_INFO);
                    break;
            }
        }
    }
}

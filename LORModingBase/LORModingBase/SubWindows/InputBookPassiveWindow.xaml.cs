using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputBookPassiveWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputBookPassiveWindow : Window
    {
        Action<string> afterSelectPassive = null;

        #region Init controls
        public InputBookPassiveWindow(Action<string> afterSelectPassive)
        {
            InitializeComponent();
            InitLbxFile();
            this.afterSelectPassive = afterSelectPassive;
        }

        private void InitLbxFile()
        {
            LbxFile.Items.Clear();
            DM.StaticInfos.passiveInfos.Keys.ToList().ForEach((string fileName) =>
            {
                LbxFile.Items.Add(fileName);
            });

            if (LbxFile.Items.Count > 0)
            {
                LbxFile.SelectedIndex = 0;
                InitLbxBookPassive();
            }
        }

        private void InitLbxBookPassive()
        {
            if (LbxFile.SelectedItem != null)
            {
                LbxPassive.Items.Clear();
                DM.StaticInfos.passiveInfos[LbxFile.SelectedItem.ToString()].ForEach((DS.PassiveInfo passiveInfo) =>
                {
                    LbxPassive.Items.Add($"{passiveInfo.passiveName}:{passiveInfo.passiveDes}:{passiveInfo.passiveID}");
                });
            }
        }
        #endregion

        private void LbxFile_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxBookPassive();
        }

        private void LbxPassive_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxPassive.SelectedIndex != -1)
            {
                afterSelectPassive(LbxPassive.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}
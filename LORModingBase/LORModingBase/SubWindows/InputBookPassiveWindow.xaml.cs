using System;
using System.Collections.Generic;
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
        List<string> passiveSearchType = new List<string>()
        {
            "모든 패시브",
            "모든 환상체 패시브",
            "모든 일반 패시브",
            "속도",
            "참격", "관통", "타격", "방어", "회피", "반격",
            "화상", "마비", "출혈", "취약", "허약", "속박", "보호", "힘", "인내", "신속", "충전", "연기",
            "빛", "탄환", "회복", "책장을 손에 추가"
        };

        #region Init controls
        public InputBookPassiveWindow(Action<string> afterSelectPassive)
        {
            InitializeComponent();
            InitLbxSearchType();
            this.afterSelectPassive = afterSelectPassive;
        }

        private void InitLbxSearchType()
        {
            LbxSearchType.Items.Clear();
            passiveSearchType.ForEach((string searchType) =>
            {
                LbxSearchType.Items.Add(searchType);
            });

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitLbxBookPassive();
            }
        }

        private void InitLbxBookPassive()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxPassive.Items.Clear();

                foreach (KeyValuePair<string, List<DS.PassiveInfo>> passiveInfoPair in DM.StaticInfos.passiveInfos)
                {
                    foreach(DS.PassiveInfo passiveInfoValue in passiveInfoPair.Value)
                    {
                        string PASSIVE_DES = $"{passiveInfoValue.passiveName}:{passiveInfoValue.passiveDes}:{passiveInfoValue.passiveID}";
                        if (!string.IsNullOrEmpty(TbxSearch.Text) && !PASSIVE_DES.Contains(TbxSearch.Text)) continue;

                        switch (LbxSearchType.SelectedItem.ToString())
                        {
                            case "모든 패시브":
                                LbxPassive.Items.Add(PASSIVE_DES);
                                break;
                            case "모든 환상체 패시브":
                                if(passiveInfoPair.Key.Contains("Creature"))
                                    LbxPassive.Items.Add(PASSIVE_DES);
                                break;
                            case "모든 일반 패시브":
                                if (!passiveInfoPair.Key.Contains("Creature"))
                                    LbxPassive.Items.Add(PASSIVE_DES);
                                break;
                            case "속도":
                                if(passiveInfoValue.passiveName.Contains(LbxSearchType.SelectedItem.ToString()))
                                    LbxPassive.Items.Add(PASSIVE_DES);
                                break;
                            default:
                                if (passiveInfoValue.passiveName.Contains(LbxSearchType.SelectedItem.ToString()) ||
                                    passiveInfoValue.passiveDes.Contains(LbxSearchType.SelectedItem.ToString()))
                                    LbxPassive.Items.Add(PASSIVE_DES);
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitLbxBookPassive();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDirectInput_Click(object sender, RoutedEventArgs e)
        {
            new InputPassiveDirectlyWindow((string passiveCode, string passiveName, string passiveDes) =>
            {
                afterSelectPassive($"{passiveName}:{passiveDes}:{passiveCode}");
                Close();
            }).ShowDialog();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputEffectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputEffectWindow : Window
    {
        Action<string> afterSelectEffect = null;
        List<string> EffectSearchType_NoCardEffect = new List<string>()
        {
            "모든 효과",
            "[적중]",
            "[합 승리]",
            "[합 시작시]",
            "[합 패배]",
            "[전투 시작]",
            "속도",
            "참격", "관통", "타격", "방어", "반격",
            "화상", "마비", "출혈", "취약", "허약", "속박", "보호", "힘", "인내", "신속", "충전", "연기",
            "빛", "회복"
        };
        List<string> EffectSearchType_CardEffect = new List<string>()
        {
            "모든 효과",
            "속도",
            "반격",
            "마비", "허약", "속박", "보호", "힘", "인내", "신속", "충전", "연기",
            "빛", "회복"
        };

        bool isCardEffect = false;

        #region Init controls
        public InputEffectWindow(Action<string> afterSelectEffect, bool isCardEffect = false)
        {
            this.afterSelectEffect = afterSelectEffect;
            this.isCardEffect = isCardEffect;
            InitializeComponent();

            InitLbxSearchType();
        }

        private void InitLbxSearchType()
        {
            LbxSearchType.Items.Clear();
            if(isCardEffect)
            {
                EffectSearchType_CardEffect.ForEach((string searchType) =>
                {
                    LbxSearchType.Items.Add(searchType);
                });

            }
            else
            {
                EffectSearchType_NoCardEffect.ForEach((string searchType) =>
                {
                    LbxSearchType.Items.Add(searchType);
                });
            }

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitLbxEffects();
            }
        }

        private void InitLbxEffects()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxEffects.Items.Clear();

                foreach(KeyValuePair<string, List<string>> effectKeyPair in DM.StaticInfos.cardEffectDic)
                {
                    string EFFECT_DES = $"{String.Join(" ", effectKeyPair.Value.ToArray())}:{effectKeyPair.Key}";

                    if (isCardEffect && !EFFECT_DES.Contains("[사용시]"))
                        continue;
                    else if (!isCardEffect && EFFECT_DES.Contains("[사용시]"))
                        continue;

                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !EFFECT_DES.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;

                    switch (LbxSearchType.SelectedItem.ToString())
                    {
                        case "모든 효과":
                            LbxEffects.Items.Add(EFFECT_DES);
                            break;
                        default:
                            if (EFFECT_DES.Contains(LbxSearchType.SelectedItem.ToString()))
                                LbxEffects.Items.Add(EFFECT_DES);
                            break;
                    }
                }
            }
        }
        #endregion

        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxEffects();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitLbxEffects();
        }

        private void LbxEffects_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxEffects.SelectedIndex != -1)
            {
                afterSelectEffect(LbxEffects.SelectedItem.ToString());
                this.Close();
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDirectInput_Click(object sender, RoutedEventArgs e)
        {
            new InputEpisodeDirectlyWindow((string effectName, string effectDes) =>
            {
                afterSelectEffect($"{effectDes}:{effectName}");
                Close();
            }, upsideLabelInfo: "효과 이름 >").ShowDialog();
        }
    }
}
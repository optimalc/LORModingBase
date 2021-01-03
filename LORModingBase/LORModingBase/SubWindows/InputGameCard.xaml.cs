using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputGameCard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputGameCard : Window
    {
        Action<DS.CardInfo> afterSelectCard = null;
        List<string> CardSearchType = new List<string>()
        {
            "모든 전투책장",
            "[기본 책장]",
            "[일반 책장]",
            "[크리쳐 책장]",
            "[마지막 이야기 책장]",
            "[기타 책장]",
            "챕터 1 : 뜬소문",
            "챕터 2 : 도시 괴담",
            "챕터 3 : 도시 전설",
            "챕터 4 : 도시 질병",
            "챕터 5 : 도시 악몽",
            "챕터 6 : 도시의 별",
            "챕터 7",
            "COST-0",
            "COST-1",
            "COST-2",
            "COST-3",
            "COST-4",
            "COST-5",
            "COST-6",
            "COST-7",
            "보급",
            "고급",
            "한정",
            "예술"
        };

        #region Init controls
        public InputGameCard(Action<DS.CardInfo> afterSelectCard)
        {
            InitializeComponent();
            InitLbxSearchType();
            this.afterSelectCard = afterSelectCard;
        }

        private void InitLbxSearchType()
        {
            LbxSearchType.Items.Clear();
            CardSearchType.ForEach((string searchType) =>
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
                LbxCards.Items.Clear();

                foreach (DS.CardInfo cardInfo in DM.StaticInfos.gameCardInfos)
                {
                    string extraInfo = "";
                    int CARD_ID = Convert.ToInt32(cardInfo.cardID);
                    if (CARD_ID < DS.FilterDatas.CARD_DIV_BASIC)
                        extraInfo += "[기본 책장] ";
                    else if (CARD_ID < DS.FilterDatas.CARD_DIV_NOMAL)
                        extraInfo += "[일반 책장] ";
                    else if (CARD_ID < DS.FilterDatas.CARD_DIV_CREATURE)
                        extraInfo += "[크리쳐 책장] ";
                    else if (CARD_ID > DS.FilterDatas.CARD_DIV_FINAL_STORY)
                        extraInfo += "[마지막 이야기 책장] ";
                    else
                        extraInfo += "[기타 책장] ";

                    string extraChpater = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CARD_INFO, "noChapter");
                    if (DS.GameInfo.chapter_Dic.ContainsKey(cardInfo.chapter))
                        extraChpater = DS.GameInfo.chapter_Dic[cardInfo.chapter];


                    string PAGEINFO_DES = $"{extraInfo}{cardInfo.name}:{extraChpater}:COST-{cardInfo.cost}:{cardInfo.rarity}:{cardInfo.cardID}";
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !PAGEINFO_DES.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;

                    switch (LbxSearchType.SelectedItem.ToString())
                    {
                        case "모든 전투책장":
                            LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                        case "보급":
                            if (PAGEINFO_DES.Contains("Common"))
                                LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                        case "고급":
                            if (PAGEINFO_DES.Contains("Uncommon"))
                                LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                        case "한정":
                            if (PAGEINFO_DES.Contains("Rare"))
                                LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                        case "예술":
                            if (PAGEINFO_DES.Contains("Unique"))
                                LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                        default:
                            if (PAGEINFO_DES.Contains(LbxSearchType.SelectedItem.ToString().Split(':').Last().Trim()))
                                LbxCards.Items.Add(PAGEINFO_DES);
                            break;
                    }
                }
            }
        }
        #endregion

        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxBookPassive();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitLbxBookPassive();
        }

        private void LbxCards_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCards.SelectedIndex != -1)
            {
                DS.CardInfo CardInfo = DM.StaticInfos.gameCardInfos.Find((DS.CardInfo cardInfo) =>
                {
                    return cardInfo.cardID == LbxCards.SelectedItem.ToString().Split(':').Last();
                });
                if (CardInfo != null)
                    afterSelectCard(CardInfo);
                this.Close();
            }
        }
    }
}
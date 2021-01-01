using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputGameCriticalPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputGameCriticalPage : Window
    {
        Action<DS.CriticalPageInfo> afterSelectCriticalPage = null;
        List<string> CriticalPagesSearchType = new List<string>()
        {
            "모든 핵심책장",
            "[사서 전용 책장]",
            "[적 전용 책장]",
            "[환상체 책장]",
            "[유저 전용 책장]",
            "챕터 1 : 뜬소문",
            "챕터 2 : 도시 괴담",
            "챕터 3 : 도시 전설",
            "챕터 4 : 도시 질병",
            "챕터 5 : 도시 악몽",
            "챕터 6 : 도시의 별",
            "챕터 7"
        };

        #region Init controls
        public InputGameCriticalPage(Action<DS.CriticalPageInfo> afterSelectCriticalPage)
        {
            InitializeComponent();
            InitLbxSearchType();
            this.afterSelectCriticalPage = afterSelectCriticalPage;
        }

        private void InitLbxSearchType()
        {
            LbxSearchType.Items.Clear();
            CriticalPagesSearchType.ForEach((string searchType) =>
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
                LbxCriticalPages.Items.Clear();

                foreach (DS.CriticalPageInfo criticalPageInfo in DM.StaticInfos.gameCriticalPageInfos)
                {
                    string extraInfo = "";
                    string extraChpater = "챕터 없음";

                    int BOOK_ID = Convert.ToInt32(criticalPageInfo.bookID);
                    if (BOOK_ID < 1000)
                        extraInfo += "[사서 전용 책장] ";
                    else if (BOOK_ID < 200000)
                    {
                        extraInfo += "[적 전용 책장] ";
                        DS.CriticalPageInfo foundCriticalPageInfo = DM.StaticInfos.gameCriticalPageInfos.Find((DS.CriticalPageInfo pageInfo) =>
                        {
                            return pageInfo.bookID == (BOOK_ID + 100000).ToString();
                        });
                        if (foundCriticalPageInfo != null)
                            extraChpater = $"{foundCriticalPageInfo.episodeDes} / {foundCriticalPageInfo.chapter}:{foundCriticalPageInfo.episode}";
                    }
                    else if (BOOK_ID > 900000)
                        extraInfo += "[환상체 책장] ";
                    else
                    {
                        extraInfo += "[유저 전용 책장]";
                        extraChpater = criticalPageInfo.episodeDes;
                    }

                    string PAGEINFO_DES = $"{extraInfo}{criticalPageInfo.name}:{extraChpater}:{criticalPageInfo.skinName}:{criticalPageInfo.bookID}";
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !PAGEINFO_DES.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;

                    switch (LbxSearchType.SelectedItem.ToString())
                    {
                        case "모든 핵심책장":
                            LbxCriticalPages.Items.Add(PAGEINFO_DES);
                            break;
                        default:
                            if (PAGEINFO_DES.Contains(LbxSearchType.SelectedItem.ToString().Split(':').Last().Trim()))
                                LbxCriticalPages.Items.Add(PAGEINFO_DES);
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

        private void LbxCriticalPages_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCriticalPages.SelectedIndex != -1)
            {
                DS.CriticalPageInfo criticalPageInfo = DM.StaticInfos.gameCriticalPageInfos.Find((DS.CriticalPageInfo pageInfo) =>
                {
                    return pageInfo.bookID == LbxCriticalPages.SelectedItem.ToString().Split(':').Last();
                });
                if (criticalPageInfo != null)
                    afterSelectCriticalPage(criticalPageInfo);
                this.Close();
            }
        }
    }
}
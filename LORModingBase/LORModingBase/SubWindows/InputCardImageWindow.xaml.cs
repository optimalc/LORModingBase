﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputCardImageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputCardImageWindow : Window
    {
        Action<string> afterSelectImage = null;
        List<string> ImageSearchType = new List<string>()
        {
            "모든 이미지명",
            "챕터 없음",
            "챕터 1 : 뜬소문",
            "챕터 2 : 도시 괴담",
            "챕터 3 : 도시 전설",
            "챕터 4 : 도시 질병",
            "챕터 5 : 도시 악몽",
            "챕터 6 : 도시의 별",
            "챕터 7"
        };

        #region Init controls
        public InputCardImageWindow(Action<string> afterSelectImage)
        {
            InitializeComponent();
            InitLbxSearchType();
            this.afterSelectImage = afterSelectImage;
        }

        private void InitLbxSearchType()
        {
            LbxSearchType.Items.Clear();
            ImageSearchType.ForEach((string searchType) =>
            {
                LbxSearchType.Items.Add(searchType);
            });

            if (LbxSearchType.Items.Count > 0)
            {
                LbxSearchType.SelectedIndex = 0;
                InitLbxCardImages();
            }
        }

        private void InitLbxCardImages()
        {
            if (LbxSearchType.SelectedItem != null)
            {
                LbxCardImages.Items.Clear();

                foreach (DS.CardInfo cardInfo in DM.StaticInfos.gameCardInfos)
                {
                    string chapterDes = "챕터 없음";
                    if (DS.GameInfo.chapter_Dic.ContainsKey(cardInfo.chapter))
                        chapterDes = DS.GameInfo.chapter_Dic[cardInfo.chapter];

                    string IMAGE_DES = $"{cardInfo.name}:{chapterDes}:{cardInfo.cardImage}";
                    if (!string.IsNullOrEmpty(TbxSearch.Text) && !IMAGE_DES.ToLower().Replace(" ", "").Contains(TbxSearch.Text.ToLower().Replace(" ", ""))) continue;

                    switch (LbxSearchType.SelectedItem.ToString())
                    {
                        case "모든 이미지명":
                            LbxCardImages.Items.Add(IMAGE_DES);
                            break;
                        default:
                            if (IMAGE_DES.Contains(LbxSearchType.SelectedItem.ToString().Split(':').Last().Trim()))
                                LbxCardImages.Items.Add(IMAGE_DES);
                            break;
                    }
                }
            }
        }
        #endregion

        private void LbxSearchType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitLbxCardImages();
        }

        private void TbxSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            InitLbxCardImages();
        }

        private void LbxCardImages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LbxCardImages.SelectedIndex != -1)
            {
                afterSelectImage(LbxCardImages.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}
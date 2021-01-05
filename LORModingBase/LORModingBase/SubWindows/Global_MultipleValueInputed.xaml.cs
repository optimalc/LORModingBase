using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LORModingBase.CustomExtensions;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// Global_MultipleValueInputed.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Global_MultipleValueInputed : Window
    {
        List<Action<string>> actionLists = null;

        public Global_MultipleValueInputed(Dictionary<string, string> labelContentToolTipDic, List<string> defaultValues, List<Action<string>> actionLists)
        {
            InitializeComponent();
            Tools.WindowControls.FindLogicalChildren<TextBox>(this).ForEachSafe((TextBox tbxData) =>
            {
                if(tbxData.Name.Contains("TbxData_"))
                {
                    string TBX_INDEX = tbxData.Name.Split('_').Last();
                    int TBX_INDEX_NUM = Convert.ToInt32(TBX_INDEX);

                    if(defaultValues.Count > TBX_INDEX_NUM)
                    {
                        tbxData.Text = defaultValues[TBX_INDEX_NUM];
                        tbxData.ToolTip = labelContentToolTipDic.Values.ToList()[TBX_INDEX_NUM];
                    }
                    else
                        tbxData.Visibility = Visibility.Collapsed;
                }
            });
            Tools.WindowControls.FindLogicalChildren<Label>(this).ForEachSafe((Label lblData) =>
            {
                if (lblData.Name.Contains("LblData_"))
                {
                    string LBL_INDEX = lblData.Name.Split('_').Last();
                    int LBL_INDEX_NUM = Convert.ToInt32(LBL_INDEX);

                    if (defaultValues.Count > LBL_INDEX_NUM)
                        lblData.Content = labelContentToolTipDic.Keys.ToList()[LBL_INDEX_NUM];
                    else
                        lblData.Visibility = Visibility.Collapsed;
                }
            });
            this.actionLists = actionLists;
        }

        private void TextChangedEvents(object sender, TextChangedEventArgs e)
        {
            if(actionLists != null)
            {
                TextBox tbx = sender as TextBox;
                string TBX_INDEX = tbx.Name.Split('_').Last();
                int TBX_INDEX_NUM = Convert.ToInt32(TBX_INDEX);

                if (actionLists.Count > TBX_INDEX_NUM)
                    actionLists[TBX_INDEX_NUM](tbx.Text);
            }
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            TbxData_0.Text = "";
            TbxData_1.Text = "";
            TbxData_2.Text = "";
            TbxData_3.Text = "";
            TbxData_4.Text = "";
            TbxData_5.Text = "";
            TbxData_6.Text = "";
            TbxData_7.Text = "";
            actionLists.ForEachSafe((Action<string> chAc) =>
            {
                chAc("");
            });
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

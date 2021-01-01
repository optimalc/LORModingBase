using System.Windows;
using System.Windows.Controls;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputEnemyInfoWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputEnemyInfoWindow : Window
    {
        DS.CriticalPageInfo criticalPageInfoToUse = null;

        public InputEnemyInfoWindow(DS.CriticalPageInfo criticalPageInfoToUse)
        {
            InitializeComponent();
            this.criticalPageInfoToUse = criticalPageInfoToUse;

            TbxStartPlayPoint.Text = criticalPageInfoToUse.ENEMY_StartPlayPoint;
            TbxMaxPlayPoint.Text = criticalPageInfoToUse.ENEMY_MaxPlayPoint;
            TbxEmotionLevel.Text = criticalPageInfoToUse.ENEMY_EmotionLevel;
            TbxAddedStartDraw.Text = criticalPageInfoToUse.ENEMY_AddedStartDraw;
        }

        private void TbxStartPlayPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            criticalPageInfoToUse.ENEMY_StartPlayPoint = TbxStartPlayPoint.Text;
        }

        private void TbxMaxPlayPoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            criticalPageInfoToUse.ENEMY_MaxPlayPoint = TbxMaxPlayPoint.Text;
        }

        private void TbxEmotionLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            criticalPageInfoToUse.ENEMY_EmotionLevel = TbxEmotionLevel.Text;
        }

        private void TbxAddedStartDraw_TextChanged(object sender, TextChangedEventArgs e)
        {
            criticalPageInfoToUse.ENEMY_AddedStartDraw = TbxAddedStartDraw.Text;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            TbxStartPlayPoint.Text = "";
            TbxMaxPlayPoint.Text = "";
            TbxEmotionLevel.Text = "";
            TbxAddedStartDraw.Text = "";

            criticalPageInfoToUse.ENEMY_StartPlayPoint = "";
            criticalPageInfoToUse.ENEMY_MaxPlayPoint = "";
            criticalPageInfoToUse.ENEMY_EmotionLevel = "";
            criticalPageInfoToUse.ENEMY_AddedStartDraw = "";
        }
    }
}

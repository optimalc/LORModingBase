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
using System.Windows.Shapes;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// InputPassiveDirectlyWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputPassiveDirectlyWindow : Window
    {
        Action<string, string, string> afterInputed = null;
        public delegate string CheckLogic(string upsideMessage, string upsideMessage2, string downsideMessgae);
        CheckLogic checkLogic = null;

        public InputPassiveDirectlyWindow(Action<string, string, string> afterInputed, CheckLogic checkLogic = null, 
            string upsideLabelInfo = "패시브 코드 >", string upside2LabelInfo = "패시브 이름 >", string downsideLabelInfo = "관련 설명 >", string windowTitle = "수동으로 직접 입력")
        {
            InitializeComponent();
            this.afterInputed = afterInputed;
            this.checkLogic = checkLogic;

            LblInputUpside.Content = upsideLabelInfo;
            LblInputUpside2.Content = upside2LabelInfo;
            LblInputDownside.Content = downsideLabelInfo;
            this.Title = windowTitle;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TbxInputUpSide.Text) || string.IsNullOrEmpty(TbxInputUpSide2.Text) || string.IsNullOrEmpty(TbxInputDownSide.Text))
                    throw new Exception("빈 값이 있습니다.");
                if (checkLogic != null)
                {
                    string CHECK_RESULT = checkLogic(TbxInputUpSide.Text, TbxInputUpSide2.Text, TbxInputDownSide.Text);
                    if(!string.IsNullOrEmpty(CHECK_RESULT))
                        throw new Exception(CHECK_RESULT);
                }

                afterInputed(TbxInputUpSide.Text, TbxInputUpSide2.Text, TbxInputDownSide.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex, "입력 과정 중 오류");
            }
        }
    }
}

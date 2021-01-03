using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LORModingBase.SubWindows
{
    /// <summary>
    /// ToEXTUrl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToEXTUrl : Window
    {
        public ToEXTUrl(List<string> listItems = null, string windowTitle = "툴과 관련된 외부 URL 링크 (더블클릭으로 이동)")
        {
            InitializeComponent();

            if(listItems == null)
            {
                listItems = new List<string>();
                listItems.Add("메인 홈페이지>https://gall.dcinside.com/mgallery/board/view/?id=lobotomycorporation&no=464908&search_head=110&page=1");
                listItems.Add("공식 가이드 1 - 핵심책장 편집 메뉴 활용>https://gall.dcinside.com/mgallery/board/view/?id=lobotomycorporation&no=467819");
                listItems.Add("공식 가이드 2 - 전투책장 편집 메뉴 활용>https://gall.dcinside.com/mgallery/board/view/?id=lobotomycorporation&no=470793");
                listItems.Add("공식 가이드 3 - 추가로 제공되는 툴의 활용>https://gall.dcinside.com/mgallery/board/view/?id=lobotomycorporation&no=471213");
            }

            listItems.ForEach((string content) =>
            {
                LbxContent.Items.Add(content);
            });

            this.Title = windowTitle;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LbxContent_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LbxContent.SelectedItem != null)
            {
                System.Diagnostics.Process.Start(LbxContent.SelectedItem.ToString().Split('>')[1]);
            }
        }
    }
}

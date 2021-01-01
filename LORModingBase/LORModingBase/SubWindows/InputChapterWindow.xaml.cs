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
    /// InputChapterWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputChapterWindow : Window
    {
        Action<string> afterInput = null;

        public InputChapterWindow(Action<string> afterInput, List<string> listItems = null, string windowTitle = "원하는 챕터를 더블 클릭")
        {
            InitializeComponent();
            this.afterInput = afterInput;

            if(listItems == null)
            {
                listItems = new List<string>();
                listItems.Add("뜬소문:1");
                listItems.Add("도시 괴담:2");
                listItems.Add("도시 전설:3");
                listItems.Add("도시 질병:4");
                listItems.Add("도시 악몽:5");
                listItems.Add("도시의 별:6");
                listItems.Add("챕터 7:7");
            }

            listItems.ForEach((string content) =>
            {
                LbxContent.Items.Add(content);
            });

            this.Title = windowTitle;
        }

        private void LbxContent_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(LbxContent.SelectedItem != null)
            {
                afterInput(LbxContent.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}

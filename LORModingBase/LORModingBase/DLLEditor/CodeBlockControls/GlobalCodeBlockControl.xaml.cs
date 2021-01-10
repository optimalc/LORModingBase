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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LORModingBase.DLLEditor.CodeBlockControls
{
    /// <summary>
    /// GlobalCodeBlockControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GlobalCodeBlockControl : UserControl
    {
        CodeBlock innerCodeBlock = null;
        Action updateTextBox = null;
        Action updateStack = null;
        CodeBlock perentCodeBlock = null;

        public GlobalCodeBlockControl()
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);
        }

        public GlobalCodeBlockControl(CodeBlock innerCodeBlock, Action updateTextBox, Action updateStack, CodeBlock perentCodeBlock)
        {
            InitializeComponent();
            Tools.WindowControls.LocalizeWindowControls(this, DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO);
            this.updateTextBox = updateTextBox;
            this.updateStack = updateStack;
            this.perentCodeBlock = perentCodeBlock;

            switch (innerCodeBlock.type)
            {
                case CODE_BLOCK_TYPE.BASE_BLOCK:
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54FFFFFF");
                    break;
            }

            LblCodeBlockType.Content = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"Type_{innerCodeBlock.type}");
            LblCodeBlockTitle.Content = innerCodeBlock.title;
            LblCodeBlockTitle.ToolTip = innerCodeBlock.title;
            BtnInfo.ToolTip = innerCodeBlock.description;

            #region Init parameters
            for (int paraIndex = 0; paraIndex < innerCodeBlock.parameterList.Count; paraIndex++)
            {
                switch (paraIndex)
                {
                    case 0:
                        GrdPara_0.Visibility = Visibility.Visible;
                        LblPara_0.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_0.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                    case 1:
                        GrdPara_1.Visibility = Visibility.Visible;
                        LblPara_1.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_1.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                    case 2:
                        GrdPara_2.Visibility = Visibility.Visible;
                        LblPara_2.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_2.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                    case 3:
                        GrdPara_3.Visibility = Visibility.Visible;
                        LblPara_3.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_3.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                    case 4:
                        GrdPara_4.Visibility = Visibility.Visible;
                        LblPara_4.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_4.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                    case 5:
                        GrdPara_5.Visibility = Visibility.Visible;
                        LblPara_5.Content = innerCodeBlock.parameterList[paraIndex];
                        TbxPara_5.Text = innerCodeBlock.inputtedParameterList[paraIndex];
                        break;
                } 
            }
            #endregion
            this.innerCodeBlock = innerCodeBlock;
        }

        private void CondeBlockControlButtonClickEvents(object sender, RoutedEventArgs e)
        {
            Button clickButton = sender as Button;
            try
            {
                switch (clickButton.Name)
                {
                    case "BtnAddNode":
                        break;
                    case "BtnDelete":
                        if (perentCodeBlock == null)
                            DLLEditor.DLLEditorMainWindow.rootCodeBlocks.Remove(innerCodeBlock);
                        else
                            perentCodeBlock.subCodeBlocks.Remove(innerCodeBlock);
                        updateTextBox();
                        updateStack();
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex,
                     DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.MAIN_WINDOW, $"MainWindowButtonClickEvents_Error_6"));
            }
        }

        private void ReflectParameterChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerCodeBlock == null)
                return;
            TextBox tbx = sender as TextBox;

            int PARA_INDEX = Convert.ToInt32(tbx.Name.Split('_').Last());
            innerCodeBlock.inputtedParameterList[PARA_INDEX] = tbx.Text;
            updateTextBox();
        }
    }
}

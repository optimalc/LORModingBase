using System;
using System.Collections.Generic;
using System.IO;
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
                case CODE_BLOCK_TYPE.IF_BLOCK:
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#5454D47F");
                    break;
                case CODE_BLOCK_TYPE.WHILE_BLOCK:
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#545471D4");
                    break;
                case CODE_BLOCK_TYPE.ACTION_BLOCK:
                    WindowBg.Fill = Tools.ColorTools.GetSolidColorBrushByHexStr("#54D4A754");
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
                        ProcessParameters(LblPara_0, TbxPara_0, innerCodeBlock, paraIndex);
                        break;
                    case 1:
                        GrdPara_1.Visibility = Visibility.Visible;
                        ProcessParameters(LblPara_1, TbxPara_1, innerCodeBlock, paraIndex);
                        break;
                    case 2:
                        GrdPara_2.Visibility = Visibility.Visible;
                        ProcessParameters(LblPara_2, TbxPara_2, innerCodeBlock, paraIndex);
                        break;
                    case 3:
                        GrdPara_3.Visibility = Visibility.Visible;
                        ProcessParameters(LblPara_3, TbxPara_3, innerCodeBlock, paraIndex);
                        break;
                    case 4:
                        GrdPara_4.Visibility = Visibility.Visible;
                        ProcessParameters(LblPara_4, TbxPara_4, innerCodeBlock, paraIndex);
                        break;
                    case 5:
                        GrdPara_5.Visibility = Visibility.Visible;
                        ProcessParameters(LblPara_5, TbxPara_5, innerCodeBlock, paraIndex);
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
                        new DLLEditorGlobalSearchWindow((CodeBlock selectedCodeBlock) =>
                        {
                            innerCodeBlock.subCodeBlocks.Add(selectedCodeBlock);
                            updateTextBox();
                            updateStack();
                        }, innerCodeBlock.subBlockWhiteFilter).ShowDialog();
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

        #region Text box parameter events
        /// <summary>
        /// Text box change -> reflect change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReflectParameterChangeInTextBox(object sender, TextChangedEventArgs e)
        {
            if (innerCodeBlock == null)
                return;
            TextBox tbx = sender as TextBox;

            int PARA_INDEX = Convert.ToInt32(tbx.Name.Split('_').Last());
            innerCodeBlock.inputtedParameterList[PARA_INDEX] = tbx.Text;
            updateTextBox();
        }

        /// <summary>
        /// Mouse left button select -> Show selector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReflectSelections(object sender, MouseButtonEventArgs e)
        {
            if (innerCodeBlock == null)
                return;
            TextBox tbx = sender as TextBox;
            if (tbx.Tag.ToString().Contains('$'))
            {
                new SubWindows.Global_ListSeleteWindow((string selectedCode) =>
                {
                    GetLocalizedDescriptionForWord(tbx, selectedCode);
                    int PARA_INDEX = Convert.ToInt32(tbx.Name.Split('_').Last());
                    innerCodeBlock.inputtedParameterList[PARA_INDEX] = selectedCode.Split('-').Last();
                    updateTextBox();
                }, ParseParameterStr(tbx.Tag.ToString())).ShowDialog();
            }
            else if (tbx.Tag.ToString().Contains('#'))
            {
                if (tbx.Tag.ToString().Split('#').Count() > 1)
                {
                    new SubWindows.Global_InputInfoWithSearchWindow((string selectedID) =>
                    {
                        int PARA_INDEX = Convert.ToInt32(tbx.Name.Split('_').Last());
                        GetDescriptionForSearchTagStr(innerCodeBlock.parameterList[PARA_INDEX], selectedID, tbx);
                        innerCodeBlock.inputtedParameterList[PARA_INDEX] = selectedID;
                        updateTextBox();
                    }, tbx.Tag.ToString().Split('#')[1]).ShowDialog();
                }
            }
        }

        /// <summary>
        /// Mouse right button select -> Handly input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlyInputToTextBox(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                if (innerCodeBlock == null)
                    return;
                TextBox tbx = sender as TextBox;
                int PARA_INDEX = Convert.ToInt32(tbx.Name.Split('_').Last());

                new SubWindows.Global_InputOneColumnData((string inputedData) =>
                {
                    tbx.Text = inputedData;
                    innerCodeBlock.inputtedParameterList[PARA_INDEX] = inputedData;
                    updateTextBox();
                }, innerCodeBlock.inputtedParameterList[PARA_INDEX]).ShowDialog();
            }
        }
        #endregion

        #region Parse and Localize parameters
        private void ProcessParameters(Label lbl, TextBox tbx, CodeBlock codeBlock, int paraIndex)
        {
            lbl.Content = codeBlock.parameterList[paraIndex].Split('$')[0].Split('#')[0];
            lbl.ToolTip = codeBlock.parameterList[paraIndex].Split('$')[0].Split('#')[0];
            if (codeBlock.parameterList[paraIndex].Contains('$') && DM.Config.config.localizeOption == "kr")
            {
                foreach (string paraDes in ParseParameterStr(codeBlock.parameterList[paraIndex]))
                {
                    if (codeBlock.inputtedParameterList[paraIndex] == paraDes.Split('-').Last())
                        tbx.Text = paraDes.Split('-')[0];
                }
            }
            else if (codeBlock.parameterList[paraIndex].Contains('#'))
            {
                GetDescriptionForSearchTagStr(
                    codeBlock.parameterList[paraIndex],
                    codeBlock.inputtedParameterList[paraIndex],
                    tbx);
            }
            else
                tbx.Text = codeBlock.inputtedParameterList[paraIndex];
            tbx.Tag = codeBlock.parameterList[paraIndex];
        }

        private List<string> ParseParameterStr(string paraStr)
        {
            List<string> LIST_TO_SHOW = new List<string>();
            paraStr.ToString().Split('$').Skip(1).ToList().ForEach((string selectStr) =>
            {
                if (selectStr.Contains("&"))
                {
                    string GROUP_PATH = $"{DS.PROGRAM_PATHS.CODE_BLOCK_GROUP}\\{selectStr.Replace("&", "")}.json";
                    if (File.Exists(GROUP_PATH))
                        LIST_TO_SHOW.AddRange(Tools.JsonFile.LoadJsonFile<List<string>>(GROUP_PATH));
                }
                else
                    LIST_TO_SHOW.Add(selectStr);
            });
            return LIST_TO_SHOW;
        }

        private void GetLocalizedDescriptionForWord(TextBox tbx, string targetWord)
        {
            if (!targetWord.Contains("-"))
            {
                tbx.Text = targetWord;
                tbx.ToolTip = targetWord;
                return;
            }

            if (DM.Config.config.localizeOption != "kr")
            {
                tbx.Text = targetWord.Split('-').Last();
                tbx.ToolTip = targetWord.Split('-').Last();
            }
            else
            {
                string KOREAN_WORD = targetWord.Split('-')[0];
                if (string.IsNullOrEmpty(KOREAN_WORD))
                {
                    tbx.Text = targetWord.Split('-').Last();
                    tbx.ToolTip = targetWord.Split('-').Last();
                }
                else
                {
                    tbx.Text = KOREAN_WORD;
                    tbx.ToolTip = KOREAN_WORD;
                }
            }
        }

        private void GetDescriptionForSearchTagStr(string searchTagStr, string inputedValue, TextBox tbx)
        {
            if (searchTagStr.Split('#').Count() > 1)
            {
                List<DM.XmlDataNode> searchedNodes = new List<DM.XmlDataNode>();
                string searchPathToUse = "";
                switch (searchTagStr.Split('#')[1])
                {
                    case SubWindows.DLL_EDITOR_SELECT_PRESET.CUSTOM_PASSIVE:
                        searchedNodes = DM.EditGameData_PassiveInfo.LocalizedPassiveDesc.rootDataNode
                            .GetXmlDataNodesByPathWithXmlInfo("PassiveDesc",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", inputedValue } });
                        searchPathToUse = "Name";
                        break;
                    case SubWindows.DLL_EDITOR_SELECT_PRESET.CUSTOM_ABILITY:
                        searchedNodes = DM.EditGameData_CardAbilityInfo.LocalizedCardAbility.rootDataNode
                            .GetXmlDataNodesByPathWithXmlInfo("BattleCardAbility",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", inputedValue } });
                        searchPathToUse = "Desc";
                        break;
                    case SubWindows.DLL_EDITOR_SELECT_PRESET.PERSONAL_EGO_CARD:
                    case SubWindows.DLL_EDITOR_SELECT_PRESET.ALL_CARDS:
                        tbx.Text = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(inputedValue).Split('/').Last();
                        tbx.ToolTip = DM.FullyLoclalizedGameDescriptions.GetFullDescriptionForCard(inputedValue);
                        return;

                    case SubWindows.DLL_EDITOR_SELECT_PRESET.CUSTOM_BUFF:
                        searchedNodes = DM.EditGameData_Buff.LocalizedBuff.rootDataNode
                            .GetXmlDataNodesByPathWithXmlInfo("effectTextList/BattleEffectText",
                            attributeToCheck: new Dictionary<string, string>() { { "ID", inputedValue } });
                        searchPathToUse = "Name";
                        break;
                    case SubWindows.DLL_EDITOR_SELECT_PRESET.CUSTOM_BUFF_FOR_NAME:
                        tbx.Text = inputedValue;
                        tbx.ToolTip = inputedValue;
                        return;

                }

                if (searchedNodes.Count > 0 && !string.IsNullOrEmpty(searchPathToUse)
                    && !string.IsNullOrEmpty(searchedNodes[0].GetInnerTextByPath(searchPathToUse)))
                {
                    tbx.Text = searchedNodes[0].GetInnerTextByPath(searchPathToUse);
                    tbx.ToolTip = searchedNodes[0].GetInnerTextByPath(searchPathToUse);
                }
                else
                {
                    tbx.Text = inputedValue;
                    tbx.ToolTip = inputedValue;
                }
            }
        } 
        #endregion
    }
}

using LORModingBase.CustomExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LORModingBase.Tools
{
    class WindowControls
    {
        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        if (child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply extra UI option
        /// </summary>
        /// <param name="targetControl">Target control to change UI</param>
        /// <param name="optionStr">Option strings</param>
        public static void ApplyTranslateOption(Control targetControl, List<string> SPLITED_INFO)
        {
            if (SPLITED_INFO.Count > 0)
            {
                List<string> TRANSFORM_INFO = SPLITED_INFO[0].Split(',').ToList();
                TransformGroup myTransformGroup = new TransformGroup();
                myTransformGroup.Children.Add(
                    new ScaleTransform(Convert.ToDouble(TRANSFORM_INFO[0]), Convert.ToDouble(TRANSFORM_INFO[1]))
                );
                if (SPLITED_INFO.Count > 1)
                {
                    List<string> POSITION_INFO = SPLITED_INFO[1].Split(',').ToList();
                    myTransformGroup.Children.Add(
                          new TranslateTransform(Convert.ToDouble(POSITION_INFO[0]), Convert.ToDouble(POSITION_INFO[1]))
                    );
                }

                targetControl.RenderTransform = myTransformGroup;
            }
        }

        /// <summary>
        /// Localize all window uis
        /// </summary>
        /// <param name="window">Window to localize</param>
        /// <param name="languageDictionary">Language dictionary to this</param>
        public static void LocalizeWindowControls(Control rootControl, string languageDictionary)
        {
            if(rootControl is Window)
            {
                Window rootWindow = rootControl as Window;
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{rootControl.Name}%"))
                    rootWindow.Title = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{rootControl.Name}%");
            }

            FindLogicalChildren<Button>(rootControl).ForEachSafe((Button btn) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{btn.Name}%"))
                {
                    string LANGUAGE_CONTENT = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{btn.Name}%");
                    List<string> SPLITED_INFO = LANGUAGE_CONTENT.Split('$').ToList();
                    btn.Content = SPLITED_INFO[0];
                    ApplyTranslateOption(btn, SPLITED_INFO.Skip(1).ToList());
                }

                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{btn.Name}_ToolTip%"))
                {
                    string LANGUAGE_CONTENT = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{btn.Name}_ToolTip%");
                    List<string> SPLITED_INFO = LANGUAGE_CONTENT.Split('$').ToList();
                    btn.ToolTip = SPLITED_INFO[0];
                    ApplyTranslateOption(btn, SPLITED_INFO.Skip(1).ToList());
                }
            });
            FindLogicalChildren<Label>(rootControl).ForEachSafe((Label lbl) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbl.Name}%"))
                {
                    string LANGUAGE_CONTENT = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbl.Name}%");
                    List<string> SPLITED_INFO = LANGUAGE_CONTENT.Split('$').ToList();
                    lbl.Content = SPLITED_INFO[0];
                    ApplyTranslateOption(lbl, SPLITED_INFO.Skip(1).ToList());
                }

                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbl.Name}_ToolTip%"))
                {
                    string LANGUAGE_CONTENT = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbl.Name}_ToolTip%");
                    List<string> SPLITED_INFO = LANGUAGE_CONTENT.Split('$').ToList();
                    lbl.ToolTip = SPLITED_INFO[0];
                    ApplyTranslateOption(lbl, SPLITED_INFO.Skip(1).ToList());
                }
            });

            FindLogicalChildren<TextBox>(rootControl).ForEachSafe((TextBox tbx) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{tbx.Name}_ToolTip%"))
                    tbx.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{tbx.Name}_ToolTip%");
            });
            FindLogicalChildren<ListBox>(rootControl).ForEachSafe((ListBox lbx) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbx.Name}_ToolTip%"))
                    lbx.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbx.Name}_ToolTip%");
            });
            FindLogicalChildren<CheckBox>(rootControl).ForEachSafe((CheckBox cbk) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{cbk.Name}_ToolTip%"))
                    cbk.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{cbk.Name}_ToolTip%");
            });
            FindLogicalChildren<Image>(rootControl).ForEachSafe((Image img) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{img.Name}_ToolTip%"))
                    img.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{img.Name}_ToolTip%");
            });
        }

        /// <summary>
        /// Init content by using name
        /// </summary>
        /// <param name="window">Window to use</param>
        /// <param name="nodeToUse">Xml data node to get</param>
        /// <param name="ignoreNameList">Name list that will be ingnored</param>
        public static void InitTextBoxControlsByUsingName(Control rootControl, DM.XmlDataNode nodeToUse, List<string> ignoreNameList = null)
        {
            FindLogicalChildren<TextBox>(rootControl).ForEachSafe((TextBox tbx) =>
            {
                if (ignoreNameList != null && ignoreNameList.Contains(tbx.Text))
                    return;

                List<string> SPLIT_NAME = tbx.Name.Split('_').ToList();
                if (SPLIT_NAME.Count == 2)
                    tbx.Text = nodeToUse.GetInnerTextByPath(SPLIT_NAME.Last());
                else if (SPLIT_NAME.Count > 2)
                    tbx.Text = nodeToUse.GetInnerTextByPath(String.Join("/", SPLIT_NAME.Skip(1)));
            });
        }
    }
}

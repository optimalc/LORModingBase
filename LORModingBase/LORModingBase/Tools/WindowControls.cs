using LORModingBase.CustomExtensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        /// Localize all window uis
        /// </summary>
        /// <param name="window">Window to localize</param>
        /// <param name="languageDictionary">Language dictionary to this</param>
        public static void LocalizeWindowControls(Window window, string languageDictionary)
        {
            FindLogicalChildren<Button>(window).ForEachSafe((Button btn) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{btn.Name}%"))
                    btn.Content = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{btn.Name}%");
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{btn.Name}_ToolTip%"))
                    btn.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{btn.Name}_ToolTip%");
            });
            FindLogicalChildren<Label>(window).ForEachSafe((Label lbl) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbl.Name}%"))
                    lbl.Content = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbl.Name}%");
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbl.Name}_ToolTip%"))
                    lbl.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbl.Name}_ToolTip%");
            });
            FindLogicalChildren<TextBox>(window).ForEachSafe((TextBox tbx) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{tbx.Name}_ToolTip%"))
                    tbx.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{tbx.Name}_ToolTip%");
            });
            FindLogicalChildren<ListBox>(window).ForEachSafe((ListBox lbx) => {
                if (DM.LocalizeCore.IsLanguageKeyExist(languageDictionary, $"%{lbx.Name}_ToolTip%"))
                    lbx.ToolTip = DM.LocalizeCore.GetLanguageData(languageDictionary, $"%{lbx.Name}_ToolTip%");
            });
        }
    }
}

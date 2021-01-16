using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for dialog
    /// </summary>
    class Dialog
    {
        /// <summary>
        /// Select directory by using dialog
        /// <para> :Parameters: </para>
        /// <para> callbackSelectedDir - Callback after select directory path </para>
        /// <para> defualtDirectory - Default directory. If empty, use working directory </para>
        /// <para> :Example: </para>
        /// <para> SelectDirectory((string selectedDic) => {Console.WriteLine(selectedDic);}); </para>
        /// <para> // Show directory select browser and print selected dir </para>
        /// </summary>
        /// <param name="callbackSelectedDir">Callback after select directory path</param>
        /// <param name="defualtDirectory">Default directory. If empty, use working directory</param>
        public static void SelectDirectory(Action<string> callbackSelectedDir, string defualtDirectory = "")
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            #region Set defualtDirectory
            if (string.IsNullOrEmpty(defualtDirectory))
                folderBrowser.SelectedPath = Tools.ProcessTools.GetWorkingDirectory();
            else
                folderBrowser.SelectedPath = defualtDirectory;
            #endregion

            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                callbackSelectedDir(folderBrowser.SelectedPath);
        }

        /// <summary>
        /// Show open file dialog and give it to view
        /// <para> :Parameters: </para>
        /// <para> title - Title to use </para>
        /// <para> filter - Filter to use </para>
        /// <para> callbackSelectedFilePath - Callback after select file path </para>
        /// <para> InitialDirectory - Initial directory path </para>
        /// <para> :Example: </para>
        /// <para> ShowOpenFileDialog("Select exe file", "EXE files|*.exe", (string selectedFilePath) => {Console.WriteLine(selectedFilePath);}); </para>
        /// <para> // Show file select browser and print selected file path </para>
        /// </summary>
        /// <param name="title">Title to use</param>
        /// <param name="filter">Filter to use </param>
        /// <param name="callbackSelectedFilePath">Callback after select file path</param>
        /// <param name="InitialDirectory">Initial directory path</param>
        public static void ShowOpenFileDialog(string title, string filter, Action<string> callbackSelectedFilePath, string InitialDirectory = @"C:\")
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = title;
            theDialog.Filter = filter;
            theDialog.InitialDirectory = InitialDirectory;
            if (theDialog.ShowDialog() == DialogResult.OK)
                callbackSelectedFilePath(theDialog.FileName.ToString());
        }

        /// <summary>
        /// Show open file dialog and give it to view
        /// <para> :Parameters: </para>
        /// <para> title - Title to use </para>
        /// <para> filter - Filter to use </para>
        /// <para> callbackSelectedFilePath - Callback after select file path </para>
        /// <para> InitialDirectory - Initial directory path </para>
        /// <para> :Example: </para>
        /// <para> ShowOpenFileDialog("Select exe file", "EXE files|*.exe", (string selectedFilePath) => {Console.WriteLine(selectedFilePath);}); </para>
        /// <para> // Show file select browser and print selected file path </para>
        /// </summary>
        /// <param name="title">Title to use</param>
        /// <param name="filter">Filter to use </param>
        /// <param name="callbackSelectedFilePath">Callback after select file path</param>
        /// <param name="InitialDirectory">Initial directory path</param>
        public static void ShowOpenMultipleFileDialog(string title, string filter, Action<string> callbackSelectedFilesPath, string InitialDirectory = @"C:\")
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = title;
            theDialog.Filter = filter;
            theDialog.InitialDirectory = InitialDirectory;
            theDialog.Multiselect = true;
            if (theDialog.ShowDialog() == DialogResult.OK)
                theDialog.FileNames.ToList().ForEach(callbackSelectedFilesPath);
        }
    }
}

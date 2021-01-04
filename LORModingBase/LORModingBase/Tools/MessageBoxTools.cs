using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Integrated messgage box
    /// </summary>
    class MessageBoxTools
    {
        /// <summary>
        /// Error message box
        /// </summary>
        public static void ShowErrorMessageBox(string message, string title = "Error is occured")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Error message box with exception
        /// </summary>
        public static void ShowErrorMessageBox(string message, Exception ex, string title = "Error is occured")
        {
            MessageBox.Show($"{message} : {ex.Message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Error message box with exception
        /// </summary>
        public static void ShowErrorMessageBox(Exception ex, string title = "Error is occured")
        {
            MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Information message box
        /// </summary>
        public static void ShowInfoMessageBox(string message, string title = "Information")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

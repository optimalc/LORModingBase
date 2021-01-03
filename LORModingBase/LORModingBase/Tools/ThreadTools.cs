using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for threads
    /// </summary>
    public static class ThreadTools
    {
        /// <summary>
        /// Make STA Thread and execute given action
        /// </summary>
        /// <param name="actionToExecute">Action to start in STA Thread</param>
        /// <returns>Created STA Thread</returns>
        public static Thread MakeSTAThreadAndStart(Action actionToExecute)
        {
            Thread thread = new Thread(new ThreadStart(actionToExecute));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Execute UI thread safely
        /// </summary>
        /// <param name="uiUpdateFunc">Func to execute</param>
        public static void ExecuteSafeUIUpdate(Action uiUpdateFunc)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, uiUpdateFunc);
        }
    }
}

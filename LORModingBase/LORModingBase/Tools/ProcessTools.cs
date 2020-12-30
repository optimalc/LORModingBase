using System.Diagnostics;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for process
    /// </summary>
    class ProcessTools
    {
        /// <summary>
        /// Get current working directory
        /// </summary>
        /// <returns>Current working directory</returns>
        public static string GetWorkingDirectory()
        {
            string FULL_PATH = Process.GetCurrentProcess().MainModule.FileName;
            string WORKING_DIRECTORY = FULL_PATH.Substring(0, FULL_PATH.LastIndexOf("\\"));
            return WORKING_DIRECTORY;
        }

        /// <summary>
        /// Open explorer with given path
        /// <para> :Parameters: </para>
        /// <para> pathToLoc - Path to locate </para>
        /// <para> :Example: </para>
        /// <para> OpenExplorer("C:\\"); </para>
        /// <para> // Open "C:\\" in explorer </para>
        /// </summary>
        /// <param name="pathToLoc">Path to locate</param>
        public static void OpenExplorer(string pathToLoc)
        {
            Process.Start("explorer.exe", pathToLoc);
        }
    }
}

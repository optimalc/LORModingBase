using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DLLEditor
{
    class CompileDLL
    {
        /// <summary>
        /// Compile given DLL source Code
        /// </summary>
        /// <param name="sourceCode"></param>
        /// <param name="dllTitle"></param>
        /// <returns></returns>
        public static string CompileGivenCodeToDLL(string sourceCode, string dllTitle)
        {
            try
            {
                List<string> DEPENDENCY_DLL_NAME = Tools.JsonFile.LoadJsonFile<List<string>>(DS.PROGRAM_RESOURCE_PATHS.RESOURCE_COMPILE_DENPENDENCIES);
                string MakedParameterStr = "";

                string OUTPUT_DLL_PATH = $"{DM.Config.CurrentWorkingDirectory}\\{dllTitle}.dll";
                if (File.Exists(OUTPUT_DLL_PATH))
                    File.Delete(OUTPUT_DLL_PATH);

                MakedParameterStr += " /target:library";
                MakedParameterStr += $" /out:\"{OUTPUT_DLL_PATH}\"";
                DEPENDENCY_DLL_NAME.ForEach((string dllName) =>
                {
                    MakedParameterStr += $" /r:\"{DM.Config.config.LORFolderPath}\\LibraryOfRuina_Data\\Managed\\{dllName}\"";
                });
                MakedParameterStr += " ./output.cs";
                File.WriteAllText("./output.cs", sourceCode);

                // Start the child process.
                Process p = new Process();
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = DM.Config.config.DLLCompilerPath;
                p.StartInfo.Arguments = MakedParameterStr;
                p.Start();
                // Do not wait for the child process to exit before
                // reading to the end of its redirected stream.
                // p.WaitForExit();
                // Read the output stream first and then wait.
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                if(output.Contains("output.cs"))
                {
                    string ERROR_MESSAGE = output.Substring(output.IndexOf("output.cs"));
                    Tools.MessageBoxTools.ShowErrorMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"ERROR_COMPILE") + $"\n{ERROR_MESSAGE}");
                }
                else
                    Tools.MessageBoxTools.ShowInfoMessageBox(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"SUCCESS_COMPILE"));

                return output;
            }
            catch(Exception ex)
            {
                Tools.MessageBoxTools.ShowErrorMessageBox(ex);
                return "";
            }
        }
    }
}

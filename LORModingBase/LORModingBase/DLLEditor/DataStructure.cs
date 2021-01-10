using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// Global datas for DLL Editing
    /// </summary>
    class GlobalDatas
    {
        /// <summary>
        /// DLL source file directory name
        /// </summary>
        public static string DLL_SOURCE_FILE_DIR_NAME = "SourceFiles";
        /// <summary>
        /// Name space to use
        /// </summary>
        public static string NAME_SPACE_TO_USE = "LOR_Moding_Base";
    }

    /// <summary>
    /// Code block data structure
    /// </summary>
    public class CodeBlock
    {
        public string type = "";
        public string title = "";
        public string description = "";
        public string codes = "";
        public List<string> subBlockWhiteFilter = new List<string>();
        public List<CodeBlock> subCodeBlocks = new List<CodeBlock>();
        public List<string> parameterList = new List<string>();
        public List<string> inputtedParameterList = new List<string>();
        public List<string> usings = new List<string>();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DLLEditor
{
    /// <summary>
    /// Config data management
    /// </summary>
    class CodeBlockDataManagement
    {
        /// <summary>
        /// Loaded code block dictionary data
        /// </summary>
        public static Dictionary<string, List<CodeBlock>> loadedCodeBlocks = new Dictionary<string, List<CodeBlock>>();
        public static string CODE_BLOCK_DICTIONARY_DIC = "./codeBlockDictionary";

        #region Save / Load code block dictionary data
        /// <summary>
        /// Load code block dictionary data
        /// </summary>
        public static void LoadData()
        {
            if (!Directory.Exists(CODE_BLOCK_DICTIONARY_DIC))
                Directory.CreateDirectory(CODE_BLOCK_DICTIONARY_DIC);

            Directory.GetDirectories(CODE_BLOCK_DICTIONARY_DIC).ToList().ForEach((string codeBlockDir) =>
            {
                string KEY_TO_USE = codeBlockDir.Split('\\').Last();
                loadedCodeBlocks[KEY_TO_USE] = new List<CodeBlock>();
                Directory.GetFiles(codeBlockDir).ToList().ForEach((string codeBlockFile) =>
                {
                    if(codeBlockFile.Split('.').Last() == "json")
                    {
                        CodeBlock createdCodeBlock = new CodeBlock();

                        Dictionary<string, List<string>> loadedCodeBlockData = Tools.JsonFile.LoadJsonFile<Dictionary<string, List<string>>>(codeBlockFile);
                        if (loadedCodeBlockData.ContainsKey("subBlockWhiteFilter"))
                            createdCodeBlock.subBlockWhiteFilter = loadedCodeBlockData["subBlockWhiteFilter"];
                        if (loadedCodeBlockData.ContainsKey(DM.Config.config.localizeOption))
                        {
                            createdCodeBlock.title = loadedCodeBlockData[DM.Config.config.localizeOption][0];
                            createdCodeBlock.description = loadedCodeBlockData[DM.Config.config.localizeOption][1];
                            createdCodeBlock.parameterList = loadedCodeBlockData[DM.Config.config.localizeOption][2].Split(',').ToList();
                        }

                        string SOURCE_DATA = codeBlockFile.Replace(".json", ".txt");
                        if (File.Exists(SOURCE_DATA))
                            createdCodeBlock.codes = File.ReadAllText(SOURCE_DATA);

                        loadedCodeBlocks[KEY_TO_USE].Add(createdCodeBlock);
                    }
                });
            });
        }
        #endregion
    }
}

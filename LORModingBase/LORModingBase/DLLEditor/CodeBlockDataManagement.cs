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
        /// Loaded code block dictionary data (Directory, Title, CodeBlock)
        /// </summary>
        public static Dictionary<string, Dictionary<string, CodeBlock>> loadedCodeBlocks = new Dictionary<string, Dictionary<string, CodeBlock>>();
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
                loadedCodeBlocks[KEY_TO_USE] = new Dictionary<string, CodeBlock>();
                Directory.GetFiles(codeBlockDir).ToList().ForEach((string codeBlockFile) =>
                {
                    if(codeBlockFile.Split('.').Last() == "json")
                    {
                        string SUB_KEY_TO_USE = codeBlockFile.Split('\\').Last().Split('.')[0];
                        CodeBlock createdCodeBlock = new CodeBlock();

                        Dictionary<string, List<string>> loadedCodeBlockData = Tools.JsonFile.LoadJsonFile<Dictionary<string, List<string>>>(codeBlockFile);
                        if (loadedCodeBlockData.ContainsKey("subBlockWhiteFilter"))
                            createdCodeBlock.subBlockWhiteFilter = loadedCodeBlockData["subBlockWhiteFilter"];
                        if (loadedCodeBlockData.ContainsKey(DM.Config.config.localizeOption))
                        {
                            createdCodeBlock.title = $"{loadedCodeBlockData[DM.Config.config.localizeOption][0]}:{KEY_TO_USE}/{SUB_KEY_TO_USE}";
                            createdCodeBlock.description = loadedCodeBlockData[DM.Config.config.localizeOption][1];
                            createdCodeBlock.parameterList = loadedCodeBlockData[DM.Config.config.localizeOption][2].Split(',').ToList();
                        }

                        string SOURCE_DATA = codeBlockFile.Replace(".json", ".txt");
                        if (File.Exists(SOURCE_DATA))
                            createdCodeBlock.codes = File.ReadAllText(SOURCE_DATA);

                        loadedCodeBlocks[KEY_TO_USE][SUB_KEY_TO_USE] = createdCodeBlock;
                    }
                });
            });
        }
        #endregion
        
        public static CodeBlock GetBaseBlockFromWhiteListData(string targetPath)
        {
            List<string> WHITE_LIST_SPLIT_DATAS = targetPath.Split('/').ToList();
            if (WHITE_LIST_SPLIT_DATAS.Count > 2)
            {
                string ROOT_KEY = WHITE_LIST_SPLIT_DATAS[0];
                string SUB_KEY = WHITE_LIST_SPLIT_DATAS[1];

                if (loadedCodeBlocks.ContainsKey(ROOT_KEY) &&
                    loadedCodeBlocks[ROOT_KEY].ContainsKey(SUB_KEY))
                    return loadedCodeBlocks[ROOT_KEY][SUB_KEY];
                else
                    return null;
            }
            else
                return null;
        }
    }
}

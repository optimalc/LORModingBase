using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LORModingBase.CustomExtensions;

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
            loadedCodeBlocks.Clear();
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
                        if (loadedCodeBlockData.ContainsKey("usings"))
                            createdCodeBlock.usings = loadedCodeBlockData["usings"];

                        if (loadedCodeBlockData.ContainsKey("type"))
                            createdCodeBlock.type = loadedCodeBlockData["type"][0];
                        if (loadedCodeBlockData.ContainsKey("inputtedParameter"))
                            createdCodeBlock.inputtedParameterList = loadedCodeBlockData["inputtedParameter"];

                        if (loadedCodeBlockData.ContainsKey(DM.Config.config.localizeOption))
                        {
                            string TYPE_DESC = DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"Type_{createdCodeBlock.type}");
                            createdCodeBlock.title = $"{TYPE_DESC}[{KEY_TO_USE}] {loadedCodeBlockData[DM.Config.config.localizeOption][0]}:{KEY_TO_USE}/{SUB_KEY_TO_USE}";
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
        
        /// <summary>
        /// Get code block by using title or path
        /// </summary>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static CodeBlock GetBaseBlockFromTargetPathOrTitle(string targetPath)
        {
            targetPath = targetPath.Split(':').Last();
            List<string> WHITE_LIST_SPLIT_DATAS = targetPath.Split('/').ToList();
            if (WHITE_LIST_SPLIT_DATAS.Count == 2)
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
    
        /// <summary>
        /// Get all code block list from name
        /// </summary>
        public static List<CodeBlock> GetAllCodeBlockListFromType(string codeBlockType)
        {
            List<CodeBlock> foundCodeBlocks = new List<CodeBlock>();
            loadedCodeBlocks.ForEachKeyValuePairSafe((string dir, Dictionary<string, CodeBlock> codeBlockDir) =>
            {
                codeBlockDir.ForEachKeyValuePairSafe((string blockName, CodeBlock codeBlock) =>
                {
                    if (codeBlock.type == codeBlockType)
                        foundCodeBlocks.Add(codeBlock);
                });
            });
            return foundCodeBlocks;
        }
    
        /// <summary>
        /// Get all code block keys
        /// </summary>
        /// <returns></returns>
        public static List<string> GetALLCodeBlockKeys()
        {
            return loadedCodeBlocks.Keys.ToList();
        }
    
        public static List<string> GetALLLocalizedBlockType()
        {
            List<string> localizedBlockTypes = new List<string>();
            List<string> TYPE_NAME_LIST = new List<string>() { "BASE", "IF", "WHILE", "ACTION"};

            TYPE_NAME_LIST.ForEach((string TYPE_NAME) =>
            {
                localizedBlockTypes.Add(DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.DLL_EDITOR_INFO, $"Type_{TYPE_NAME}"));
            });

            return localizedBlockTypes;
        }
    }


    /// <summary>
    /// The type of code block
    /// </summary>
    class CODE_BLOCK_TYPE
    {
        public const string BASE_BLOCK = "BASE";
        public const string IF_BLOCK = "IF";
        public const string WHILE_BLOCK = "WHILE";
        public const string ACTION_BLOCK = "ACTION";
    }
}

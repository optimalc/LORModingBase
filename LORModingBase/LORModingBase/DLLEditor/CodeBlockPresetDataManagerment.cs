using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DLLEditor
{
    class CodeBlockPresetDataManagerment
    {
        /// <summary>
        /// Loaded code block preset data
        /// </summary>
        public static List<CodeBlockPresets> loadedCodeBlockPresets = new List<CodeBlockPresets>();

        #region Save / Load code block dictionary data
        /// <summary>
        /// Load code block dictionary data
        /// </summary>
        public static void LoadData()
        {
            loadedCodeBlockPresets.Clear();
            if (!Directory.Exists(DS.PROGRAM_PATHS.CODE_BLOCK_PRESET))
                Directory.CreateDirectory(DS.PROGRAM_PATHS.CODE_BLOCK_PRESET);

            Directory.GetFiles(DS.PROGRAM_PATHS.CODE_BLOCK_PRESET).ToList().ForEach((string codeBlockPresetFile) =>
            {
                if (codeBlockPresetFile.Contains("DESCRIPTION_"))
                {
                    Dictionary<string, List<string>> loadedCodeBlockDes = Tools.JsonFile.LoadJsonFile<Dictionary<string, List<string>>>(codeBlockPresetFile);
                    if (loadedCodeBlockDes.ContainsKey(DM.Config.config.localizeOption))
                    {
                        List<string> localizedCodeBlockDes = loadedCodeBlockDes[DM.Config.config.localizeOption];
                        CodeBlockPresets newCodeBlockPreset = new CodeBlockPresets();
                        newCodeBlockPreset.title = localizedCodeBlockDes[0];
                        newCodeBlockPreset.description = localizedCodeBlockDes[1];

                        string CODE_BLCOKS_FILE_NAME = codeBlockPresetFile.Replace("DESCRIPTION_", "CODE_BLOCKS_");
                        if(File.Exists(CODE_BLCOKS_FILE_NAME))
                        {
                            List<List<string>> loadedCodeBlockPresetSyntaxStrLists = Tools.JsonFile.LoadJsonFile<List<List<string>>>(CODE_BLCOKS_FILE_NAME);
                            List<CodeBlock> newCodeBlockLists = new List<CodeBlock>();

                            Dictionary<string, CodeBlock> codeBlockDics = new Dictionary<string, CodeBlock>();
                            foreach(List<string> loadedCodeBlockPresetSyntaxStrList in loadedCodeBlockPresetSyntaxStrLists)
                            {
                                string CODE_BLOCK_PATH_TO_USE = loadedCodeBlockPresetSyntaxStrList[0];
                                string CODE_BLOCK_UNIQUE_NAME = loadedCodeBlockPresetSyntaxStrList[1];
                                string CODE_BLOCK_PERENT_NAME = loadedCodeBlockPresetSyntaxStrList[2];

                                CodeBlock newCodeBlock = CodeBlockDataManagement.GetBaseBlockFromTargetPathOrTitle(CODE_BLOCK_PATH_TO_USE);
                                codeBlockDics[CODE_BLOCK_UNIQUE_NAME] = newCodeBlock;
                                if (string.IsNullOrEmpty(CODE_BLOCK_PERENT_NAME))
                                    newCodeBlockLists.Add(newCodeBlock);
                                else if (codeBlockDics.ContainsKey(CODE_BLOCK_PERENT_NAME))
                                    codeBlockDics[CODE_BLOCK_PERENT_NAME].subCodeBlocks.Add(newCodeBlock);
                            }

                            newCodeBlockPreset.rootCodeBlocks = newCodeBlockLists;
                            loadedCodeBlockPresets.Add(newCodeBlockPreset);
                        }
                    }
                }
            });
        }
        #endregion
    }
}

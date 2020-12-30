using Newtonsoft.Json;
using System.IO;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Management .json format files
    /// </summary>
    class JsonFile
    {
        /// <summary>
        /// Save data in json format
        /// <para> :Parameters: </para>
        /// <para> JSONFileName - File path (.json) </para>
        /// <para> classToSave - Data class to save </para>
        /// <para> :Example: </para>
        /// <para> SaveJsonFile&lt;DS.Config&gt;("./config.json", new Config() {dataPath="./testdata.txt"}); </para>
        /// <para> // Save data by using json format </para>
        /// </summary>
        /// <typeparam name="TargetClass">Data class type to save</typeparam>
        /// <param name="JSONFileName">File path (.json)</param>
        /// <param name="classToSave">Data class to save</param>
        /// <returns>True if successed, False if fail.</returns>
        public static bool SaveJsonFile<TargetClass>(string JSONFileName, TargetClass classToSave)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(classToSave);
                File.WriteAllText(JSONFileName, jsonData);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Load .json file
        /// <para> :Parameters: </para>
        /// <para> JSONFileName - File path (.json) </para>
        /// <para> :Example: </para>
        /// <para> DS.Config config = LoadJsonFile&lt;DS.Config&gt;("./config.json"); </para>
        /// <para> // Load json data and convert to DS.Config class </para>
        /// </summary>
        /// <typeparam name="TargetClass">Class type to load</typeparam>
        /// <param name="JSONFileName">File path (.json)</param>
        /// <returns>Loaded class</returns>
        public static TargetClass LoadJsonFile<TargetClass>(string JSONFileName)
        {
            try
            {
                string loadedString = File.ReadAllText(JSONFileName);
                return JsonConvert.DeserializeObject<TargetClass>(loadedString);
            }
            catch
            {
                return default(TargetClass);
            }
        }
    }
}

using System.Collections.Generic;

namespace LORModingBase.DS
{
    /// <summary>
    /// Filter datas structure
    /// </summary>
    class FilterDatas
    {
        /// <summary>
        /// Passives that must be excluded
        /// </summary>
        public static List<string> EXCLUDE_PASSIVE_CODE = new List<string>()
        {
            "150015", "150016", "150017", "150116", "150117",
            "200008", "200009", "200011", "200012",
            "211001", "211002", "211004",
            "221002", "221004", "221003",
            "230028",
            "240008", "240028", "241301",
            "250013", "250227",
            "305211", "305414", "305415", "305423", "305431", "305510",
            "404015", "404023", "405423",
            "505313",
            "604031", "605341", "605411", "605421", "605432"
        };
    }
}

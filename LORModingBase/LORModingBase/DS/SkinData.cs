using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DS
{
    /// <summary>
    /// Relative path
    /// </summary>
    class SkinRelativePaths
    {
        public const string ThumbPath = "Thumb.png";
        public const string ItemInfoPath = "ItemInfo.xml";
        public const string ModInfoPath = "ModInfo.Xml";

        const string ClothCustomFolder = "ClothCustom";
        public static string Cloth_Aim = $"{ClothCustomFolder}\\Aim.png";
        public static string Cloth_Damaged = $"{ClothCustomFolder}\\Damaged.png";
        public static string Cloth_Default = $"{ClothCustomFolder}\\Default.png";
        public static string Cloth_Evade = $"{ClothCustomFolder}\\Evade.png";
        public static string Cloth_Fire = $"{ClothCustomFolder}\\Fire.png";
        public static string Cloth_Guard = $"{ClothCustomFolder}\\Guard.png";
        public static string Cloth_Hit = $"{ClothCustomFolder}\\Hit.png";
        public static string Cloth_Move = $"{ClothCustomFolder}\\Move.png";
        public static string Cloth_Penetrate = $"{ClothCustomFolder}\\Penetrate.png";
        public static string Cloth_Slash = $"{ClothCustomFolder}\\Slash.png";
    }
}

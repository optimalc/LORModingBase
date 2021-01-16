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

        public const string ClothCustomFolder = "ClothCustom";
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

        public static List<string> GetAllSkinImagePaths(string rootSkinPath)
        {
            List<string> skinPaths = new List<string>();
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.ThumbPath}");

            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Aim}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Damaged}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Default}");

            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Evade}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Fire}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Guard}");

            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Hit}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Move}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Penetrate}");
            skinPaths.Add($"{rootSkinPath}\\{SkinRelativePaths.Cloth_Slash}");
            return skinPaths;
        }
    }
}

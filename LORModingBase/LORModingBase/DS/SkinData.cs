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

        public static Dictionary<string, string> GetAllSkinImagePaths(string rootSkinPath)
        {
            Dictionary<string, string> skinPaths = new Dictionary<string, string>();
            skinPaths["썸네일"] = $"{rootSkinPath}\\{SkinRelativePaths.ThumbPath}";

            skinPaths["적중 당함"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Aim}";
            skinPaths["데미지를 받음"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Damaged}";
            skinPaths["화염 데미지를 받음"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Fire}";

            skinPaths["기본 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Default}";
            skinPaths["회피 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Evade}";
            skinPaths["방어 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Guard}";
            skinPaths["이동 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Move}";

            skinPaths["타격 공격 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Hit}";
            skinPaths["관통 공격 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Penetrate}";
            skinPaths["참격 공격 자세"] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Slash}";
            return skinPaths;
        }
    }
}

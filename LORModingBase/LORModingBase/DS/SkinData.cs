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
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_THUMB_NAIL")] = $"{rootSkinPath}\\{SkinRelativePaths.ThumbPath}";

            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_HIT")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Aim}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_TAKE_DAMAGE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Damaged}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_TAKE_FIRE_DAMAGE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Fire}";

            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_DEFAULT_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Default}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_EVADE_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Evade}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_GUARD_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Guard}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_MOVE_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Move}";

            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_HIT_ATTACK_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Hit}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_PENETRATE_ATTACK_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Penetrate}";
            skinPaths[DM.LocalizeCore.GetLanguageData(DM.LANGUAGE_FILE_NAME.CUSTOM_SKIN_INFO, $"IMAGE_SLASH_ATTACK_POSTURE")] = $"{rootSkinPath}\\{SkinRelativePaths.Cloth_Slash}";
            return skinPaths;
        }
    }
}

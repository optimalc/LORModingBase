using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LORModingBase.Tools
{
    /// <summary>
    /// Tools for color, brushs
    /// </summary>
    class ColorTools
    {
        /// <summary>
        /// Get SolidColorBrush class by using color hex code
        /// <para> :Parameters: </para>
        /// <para> colorHexStr - Color hex code </para>
        /// <para> :Example: </para>
        /// <para> getSolidColorBrushByHexStr("#5430BF4B"); </para>
        /// <para> // Get SolidColorBrush by using hex color string </para>
        /// </summary>
        /// <param name="colorHexStr">Color hex code</param>
        /// <returns>SolidColorBrush from color hex string</returns>
        public static SolidColorBrush GetSolidColorBrushByHexStr(string colorHexStr)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHexStr));
        }

        /// <summary>
        /// Get ImageBrush object from image path
        /// <para> :Parameters: </para>
        /// <para> baseControl - Base control for navigation </para>
        /// <para> imagePath - Image path to use </para>
        /// <para> :Example: </para>
        /// <para> BtnCiricalBookInfo.Background = GetImageBrushFromPath(this, "../Resources/IconYesbookInfo.png"); </para>
        /// <para> // Get ImageBrush by using image path "../Resources/IconYesbookInfo.png" </para>
        /// </summary>
        /// <param name="baseControl">Base control for navigation</param>
        /// <param name="imagePath">Image path to use</param>
        /// <returns>ImageBrush from given image path</returns>
        public static ImageBrush GetImageBrushFromPath(Control baseControl, string imagePath)
        {
            return new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(baseControl), imagePath)));
        }
    }
}

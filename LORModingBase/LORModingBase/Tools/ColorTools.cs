using System.Windows.Media;

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
        /// <returns></returns>
        public static SolidColorBrush GetSolidColorBrushByHexStr(string colorHexStr)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHexStr));
        }
    }
}

using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace LORModingBase.Tools
{
    class ImageTools
    {
        public static BitmapImage ByStream(string filePath)
        { 
            try
            {
                if (File.Exists(filePath))
                {
                    // do this so that the image file can be moved in the file system
                    BitmapImage result = new BitmapImage();
                    // Create new BitmapImage   
                    Stream stream = new MemoryStream(); // Create new MemoryStream   
                    Bitmap bitmap = new Bitmap(filePath);
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    bitmap.Dispose(); // Dispose bitmap so it releases the source image file   
                    result.BeginInit(); // Begin the BitmapImage's initialisation   
                    result.StreamSource = stream;
                    // Set the BitmapImage's StreamSource to the MemoryStream containing the image   
                    result.EndInit(); // End the BitmapImage's initialisation   
                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}

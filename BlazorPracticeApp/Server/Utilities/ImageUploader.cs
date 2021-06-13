using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace BlazorPracticeApp.Server.Utilities
{
    public static class ImageUploader
    {
        public static void AddImageToServer(this Image image, string fileName, string originalPath, string deleteFileName = null)
        {

            if (image != null)
            {
                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);

                if (!string.IsNullOrEmpty(deleteFileName))
                {
                    if (File.Exists(originalPath + deleteFileName))
                        File.Delete(originalPath + deleteFileName);
                }

                string OriginPath = originalPath + fileName;

                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) image.Save(stream, ImageFormat.Jpeg);
                }
            }
        }
    }
}

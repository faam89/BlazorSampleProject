using System;
using System.Drawing;
using System.IO;



namespace BlazorPracticeApp.Server.Utilities
{
    public static class Convertor
    {
        public static byte[] DecodeUrlBase64(string s)
        {
            return Convert.FromBase64String(s.Substring(s.LastIndexOf(',') + 1));
        }

        //public static MediaTypeNames.Image Base64ToImage(string base64String)
        //{
        //    var res = DecodeUrlBase64(base64String);
        //    MemoryStream ms = new MemoryStream(res, 0, res.Length);
        //    ms.Write(res, 0, res.Length);
        //    MediaTypeNames.Image image = MediaTypeNames.Image.FromStream(ms, true);
        //    return image;
        //}

        public static Image Base64ToImage(string base64String)
        {
            var res = DecodeUrlBase64(base64String);
            MemoryStream ms = new MemoryStream(res, 0, res.Length);
            ms.Write(res, 0, res.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}


using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartBuilding.FaceApp.Helpers
{
    public class SKiaImageHelper
    {
        public static byte[] FixedSize(MemoryStream ms, int Width, int Height, bool needToFill)
        {
            try
            {
                var arr = ms.ToArray();
                var result = Resize(arr, Width, Height);
                return result.FileContents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }

        }
        public static (byte[] FileContents, int Height, int Width) Resize(byte[] fileContents, int maxWidth, int maxHeight, SKFilterQuality quality = SKFilterQuality.Medium)
        {
            using MemoryStream ms = new MemoryStream(fileContents);
            using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

            int height = Math.Min(maxHeight, sourceBitmap.Height);
            int width = Math.Min(maxWidth, sourceBitmap.Width);

            using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), quality);
            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
            using SKData data = scaledImage.Encode();

            return (data.ToArray(), height, width);
        }

    }
   
}

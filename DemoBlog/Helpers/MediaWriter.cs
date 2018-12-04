using System.Linq;
using System.Text;

namespace DemoBlog.Helpers
{
    public class MediaWriter
    {
        public enum MediaFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static MediaFormat GetMediaFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM"); // BMP
            var gif = Encoding.ASCII.GetBytes("GIF"); // GIF
            var png = new byte[] {137, 80, 78, 71}; // PNG
            var tiff = new byte[] {73, 73, 42}; // TIFF
            var tiff2 = new byte[] {77, 77, 42}; // TIFF
            var jpeg = new byte[] {255, 216, 255, 224}; // jpeg
            var jpeg2 = new byte[] {255, 216, 255, 225}; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return MediaFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return MediaFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return MediaFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return MediaFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return MediaFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return MediaFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return MediaFormat.jpeg;

            return MediaFormat.unknown;
        }
    }
}
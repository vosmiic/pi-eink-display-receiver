// See https://aka.ms/new-console-template for more information

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Point = SixLabors.ImageSharp.Point;

namespace pi_eink_display_receiver
{
    internal class Program
    {
        static void Main(string[] args) {
            var epd = new EPD2in7();
            epd.Initialize();
            epd.Clear();

            var image = Image.Load<Rgba32>(args[0]);
                
            image.Mutate(x => {
                x.Resize(Convert.ToInt32(epd.Height / 4), Convert.ToInt32(epd.Width / 4));
                x.Invert();
                x.Opacity(0);
            });

            var newImage = new Image<Rgba32>(epd.Height, epd.Width);
            newImage.Mutate(x => {
                x.DrawLine(Color.Black, 1, new PointF(165, 50), new PointF(165, 100));
                x.DrawLine(Color.Black, 1, new PointF(140, 75), new PointF(190, 75));
                x.DrawImage(image, new Point(20, 50), PixelColorBlendingMode.Normal, PixelAlphaCompositionMode.Src, 0);
            });
                
            epd.Display(newImage);
        }
    }
}
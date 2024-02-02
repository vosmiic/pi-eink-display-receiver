using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace pi_eink_display_receiver; 

public class VideoConverter {
    public static async Task PlayVideo(string pathToVideo, EPD2in7 epd) {
        var directoryOfPbmImages = await FFmpegConversions.ConvertVideo(pathToVideo);
        if (directoryOfPbmImages == null) return;
        List<Image<Rgba32>> frames = ConvertImages(epd, GetFolderImages(directoryOfPbmImages));
        
        while (true) {
            foreach (Image<Rgba32> frame in frames) {
                epd.Clear();
                
                epd.Display(frame);
                Thread.Sleep(2000);
            }
            
            epd.Clear();
        }
    }

    private static List<Image<Rgba32>> GetFolderImages(DirectoryInfo directoryInfo) =>
        directoryInfo.GetFiles().Select(image => Image.Load<Rgba32>(image.FullName)).ToList();

    private static List<Image<Rgba32>> ConvertImages(EPD2in7 epd, List<Image<Rgba32>> stockImages) {
        foreach (Image<Rgba32> stockImage in stockImages) {
            stockImage.Mutate(x => {
                x.Resize(epd.Height, epd.Width);
                x.Invert();
                x.Opacity(0);
            });
        }

        return stockImages;
    }
}
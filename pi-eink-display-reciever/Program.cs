using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace pi_eink_display_receiver
{
    internal class Program
    {
        static async Task Main(string[] args) {
            ServiceProvider serviceProvider = RegisterServices();
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
        
        private static ServiceProvider RegisterServices() {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json")
                .Build();
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);

            return serviceCollection.BuildServiceProvider();
        }
    }
}
using System.Security.Cryptography;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace pi_eink_display_receiver; 

public class FFmpegConversions {
    public static async Task<DirectoryInfo?> ConvertVideo(string videoPath) {
        if (!File.Exists(videoPath)) {
            Console.WriteLine("File could not be found/incorrect permissions");
            return null;
        }

        string fileHash = await GetFileMd5(videoPath);
        return new DirectoryInfo(videoPath);
    }

    private static async Task<string> GetFileMd5(string file) {
        using var md5 = MD5.Create();
        await using var stream = File.OpenRead(file);
        return BitConverter.ToString(await md5.ComputeHashAsync(stream)).Replace("-", "").ToLowerInvariant();
    }
}
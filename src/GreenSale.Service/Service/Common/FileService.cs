using GreenSale.Persistence.Helpers;
using GreenSale.Service.Interfaces.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GreenSale.Service.Service.Common;

public class FileService : IFileService
{
    private readonly string MEDIA = "Media";
    private readonly string IMAGES = "Images";
    private readonly string ROOTPATH;

    public FileService(IWebHostEnvironment env)
    {
        ROOTPATH = env.WebRootPath;
    }

    public async Task<bool> DeleteImageAsync(string subpath)
    {
        string path = Path.Combine(ROOTPATH, subpath);

        if (File.Exists(path))
        {
            await Task.Run(() =>
            {
                File.Delete(path);
            });

            return true;
        }

        return false;
    }

    public async Task<string> UploadImageAsync(IFormFile image, string rootpath)
    {
        string newImageName = MediaHelper.MakeImageName(image.FileName);
        string subPath = Path.Combine(MEDIA, IMAGES, rootpath, newImageName);
        string path = Path.Combine(ROOTPATH, subPath);
        var stream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(stream);
        stream.Close();

        return subPath;
    }
}

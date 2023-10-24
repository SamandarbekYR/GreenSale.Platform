using Microsoft.AspNetCore.Http;

namespace GreenSale.Service.Interfaces.Common;

public interface IFileService
{
    public Task<string> UploadImageAsync(IFormFile image, string rootpath);
    public Task<bool> DeleteImageAsync(string subpath);
}

using Microsoft.AspNetCore.Http;

namespace GreenSale.Persistence.Dtos.StoragDtos;

public class StorageImageUpdateDto
{
    public IFormFile StorageImage { get; set; } = default!;
}

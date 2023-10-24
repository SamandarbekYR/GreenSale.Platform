using Microsoft.AspNetCore.Http;

namespace GreenSale.Persistence.Dtos.SellerPostImageUpdateDtos;

public class SellerPostImageUpdateDto
{
    public IFormFile ImagePath { get; set; } = default!;
}

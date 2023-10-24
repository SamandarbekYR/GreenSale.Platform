using Microsoft.AspNetCore.Http;

namespace GreenSale.Persistence.Dtos.BuyerPostImageUpdateDtos;

public class BuyerPostImageDto
{
    public IFormFile ImagePath { get; set; } = default!;
}

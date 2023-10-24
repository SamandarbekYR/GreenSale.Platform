using Microsoft.AspNetCore.Http;

namespace GreenSale.Persistence.Dtos.StoragDtos;

public class StoragCreateDto
{
    public long CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double AddressLatitude { get; set; }
    public double AddressLongitude { get; set; }
    public string Info { get; set; } = string.Empty;
    public IFormFile ImagePath { get; set; } = default!;
}

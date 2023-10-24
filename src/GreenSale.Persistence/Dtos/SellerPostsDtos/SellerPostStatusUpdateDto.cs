using GreenSale.Domain.Enums.SellerPosts;

namespace GreenSale.Persistence.Dtos.SellerPostsDtos;

public class SellerPostStatusUpdateDto
{
    public SellerPostEnum PostStatus { get; set; }
}

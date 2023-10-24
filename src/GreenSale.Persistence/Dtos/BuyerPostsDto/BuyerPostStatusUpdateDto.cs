using GreenSale.Domain.Enums.BuyerPosts;

namespace GreenSale.Persistence.Dtos.BuyerPostsDto;

public class BuyerPostStatusUpdateDto
{
    public BuyerPostEnum PostStatus { get; set; }
}

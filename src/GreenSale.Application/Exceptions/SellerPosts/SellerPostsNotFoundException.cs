namespace GreenSale.Application.Exceptions.SellerPosts;

public class SellerPostsNotFoundException : NotFoundException
{
    public SellerPostsNotFoundException()
    {
        this.TitleMessage = "Seller's announcement not found!";
    }
}
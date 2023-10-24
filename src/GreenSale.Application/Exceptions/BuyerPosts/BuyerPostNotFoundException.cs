namespace GreenSale.Application.Exceptions.BuyerPosts;

public class BuyerPostNotFoundException : NotFoundException
{
    public BuyerPostNotFoundException()
    {
        this.TitleMessage = "Buyer's announcement not found!";
    }
}
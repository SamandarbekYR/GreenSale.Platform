namespace GreenSale.Application.Exceptions.PostStars;

public class PostStarAmmountManyException :BadRequestException
{
	public PostStarAmmountManyException()
	{
		this.TitleMessage = "The number of stars was entered incorrectly!";
    }
}

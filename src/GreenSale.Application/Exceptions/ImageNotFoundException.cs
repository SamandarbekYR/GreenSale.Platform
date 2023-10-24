namespace GreenSale.Application.Exceptions
{
    public class ImageNotFoundException : NotFoundException
    {
        public ImageNotFoundException()
        {
            TitleMessage = "Image not found!";
        }
    }
}

namespace GreenSale.Application.Exceptions.Categories;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException()
    {
        TitleMessage = "Category not found!";
    }
}
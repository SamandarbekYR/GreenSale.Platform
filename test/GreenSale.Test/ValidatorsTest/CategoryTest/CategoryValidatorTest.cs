using GreenSale.Persistence.Dtos.CategoryDtos;
using GreenSale.Persistence.Validators.Categories;

namespace GreenSale.Test.ValidatorsTest.CategoryTestl;

public class CategoryValidatorTest
{
    [Theory]
    [InlineData("Uzum")]
    [InlineData("Nok")]
    [InlineData("Olma")]
    [InlineData("Gilos")]
    [InlineData("poliz ekinlari")]
    [InlineData("Tarvuz")]
    [InlineData("Qovuz")]
    [InlineData("Anor")]
    [InlineData("Shaftoli")]
    [InlineData("Anjir")]

    public void CheckRightTest(string value)
    {
        CategoryCreateValidator validator = new CategoryCreateValidator();

        CategoryCreateDto categoryCreateDto = new CategoryCreateDto()
        {
            Name = value,
        };

        var result = validator.Validate(categoryCreateDto);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("No")]
    [InlineData("                           ")]
    [InlineData("Gilszdfzddvc jzxdfdfvkjzbxfv,kzjbxfnv,zkjxfbnvz,kjxdbvnzlkjxdfbvn")]
    [InlineData("  ")]

    public void CheckFalseTest(string value)
    {
        CategoryCreateValidator validator = new CategoryCreateValidator();

        CategoryCreateDto categoryCreateDto = new CategoryCreateDto()
        {
            Name = value,
        };

        var result = validator.Validate(categoryCreateDto);
        Assert.False(result.IsValid);
    }
}

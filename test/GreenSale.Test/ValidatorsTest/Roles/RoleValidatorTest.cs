using GreenSale.Persistence.Dtos.RoleDtos;
using GreenSale.Persistence.Validators.Roles;

namespace GreenSale.Test.ValidatorsTest.Roles;

public class RoleValidatorTest
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
        RoleValidator validator = new RoleValidator();

        RoleCreatDto categoryCreateDto = new RoleCreatDto()
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
        RoleValidator validator = new RoleValidator();

        RoleCreatDto categoryCreateDto = new RoleCreatDto()
        {
            Name = value,
        };

        var result = validator.Validate(categoryCreateDto);
        Assert.False(result.IsValid);
    }
}

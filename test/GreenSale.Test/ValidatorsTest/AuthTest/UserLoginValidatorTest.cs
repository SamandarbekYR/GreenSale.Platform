using GreenSale.Persistence.Dtos;
using GreenSale.Persistence.Validators.Dtos.AuthUserValidators;

namespace GreenSale.Test.ValidatorsTest.AuthTest;

public class UserLoginValidatorTest
{
    [Theory]
    [InlineData("+998951092161", "asAS@#%123")]
    [InlineData("+998971234567", "Abcd123!@#")]
    [InlineData("+998998877665", "P@$$w0rd123")]
    [InlineData("+998935555555", "Qwerty!123")]
    [InlineData("+998940404040", "Hello123!")]
    [InlineData("+998950505050", "MyP@ssw0rd")]
    [InlineData("+998960606060", "GreenSale!")]
    [InlineData("+998970707070", "Testing@123")]
    [InlineData("+998980808080", "Welcome123")]
    [InlineData("+998990909090", "Security#1")]

    public void CheckRightValid(string phone, string password)
    {
        var dto = new UserLoginDto()
        {
            PhoneNumber = phone,
            password = password
        };

        UserLoginValidator userLoginDto = new UserLoginValidator();
        var result = userLoginDto.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("-998951092161", "invalidpassword")]
    [InlineData("123456789", "asAS@#%123")]
    [InlineData("+998951092161", "")]
    [InlineData("invalidphone", "asAS@#%123")]
    [InlineData("+998951092161", "      ")]
    [InlineData("+99851092161", "123456789")]
    [InlineData("1092161", "UPPERCASE")]
    [InlineData("+", "noSpecialCharacters")]
    [InlineData("+99895jbdsdb1092161", "longpasswordlongpasswordlongpasswordlongpasswordlongpasswordlongpassword")]
    [InlineData("+99895145092161", "noDigits#")]

    public void checkWrongTest(string phone, string password)
    {
        var dto = new UserLoginDto()
        {
            PhoneNumber = phone,
            password = password
        };

        var validator = new UserLoginValidator();
        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }
}

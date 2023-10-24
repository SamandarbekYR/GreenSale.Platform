using GreenSale.Persistence.Validators;

namespace GreenSale.Test.ValidatorsTest;

public class PhoneNumberTest
{
    [Theory]
    [InlineData("+998331211314")]
    [InlineData("+998331711314")]
    [InlineData("+998334211314")]
    [InlineData("+998501211314")]
    [InlineData("+998931211314")]
    [InlineData("+998901211314")]
    [InlineData("+998901011314")]
    [InlineData("+998941211314")]
    [InlineData("+998911211314")]
    [InlineData("+998501911314")]

    public void ShoulReturnCorrect(string phoneNumber)
    {
        var result = PhoneNumberValidator.IsValid(phoneNumber);
        Assert.True(result);
    }

    [Theory]
    [InlineData("+99831211314")]
    [InlineData("+99331711314")]
    [InlineData("+911314")]
    [InlineData("+9985012efd11314")]
    [InlineData("+99892!##1211314")]
    [InlineData("998901211314")]
    [InlineData("-998901011314")]
    [InlineData("+998-94-121-13-14")]
    [InlineData("+998911211314ASDASD")]
    [InlineData("+998 50 191 13 14")]

    public void ShouldReturnWrong(string phone)
    {
        var result = PhoneNumberValidator.IsValid(phone);
        Assert.False(result);
    }
}

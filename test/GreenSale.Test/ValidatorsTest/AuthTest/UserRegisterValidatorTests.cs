using GreenSale.Persistence.Dtos;
using GreenSaleuz.Persistence.Validators.Dtos.AuthUserValidators;

namespace GreenSale.Test.ValidatorsTest.AuthTest;

public class UserRegisterValidatorTests
{
    [Theory]
    [InlineData("Uligaliyev", "Hasa", "+998901234567", "NAdijan", "Asska", "Address123", "StrongP@ss123")]
    [InlineData("Samit", "tooShort", "+998901234567", "Region", "District", "Address123", "StrongP@ss123")]
    [InlineData("Sobir", "Doe", "+998952341232", "Region", "District", "Address123", "StrongP@ss123")]
    [InlineData("Akbar", "Doe", "+998901234567", "Toshkent", "District", "Address123", "StrongP@ss123")]
    [InlineData("Soli", "Doe", "+998901234567", "Region", "Namanjan", "Address123", "StrongP@ss123")]
    [InlineData("MirShakar", "Doe", "+998901234567", "Region", "District", "Jozax", "StrongP@ss123")]
    [InlineData("Ali", "Doe", "+998901234567", "Region", "District", "Address123", "32HGyt^&^")]
    [InlineData("Sobir", "Doe", "+998901234567", "Region", "District", "Address123", "noSym^&*BK12bols")]
    [InlineData("Javlonhn", "Doe", "+998904563412", "Region", "District", "Address123", "StrongP@ss123")]
    [InlineData("Aohn", "Doe", "+998901234567", "Region", "District", "Address123", "dfkbhjGJV78875^&*^&*")]

    public void ValidUserRegisterDto_ReturnsNoValidationErrors(
        string firstName, string lastName, string phoneNumber, string region, string district,
        string address, string password)
    {
        var dto = new UserRegisterDto
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Region = region,
            District = district,
            Address = address,
            Password = password
        };

        UserRegisterValidator validationRules = new UserRegisterValidator();
        var result = validationRules.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "Doe", "+998901234567", "Region", "District", "Address123", "weak")]
    [InlineData("John", "", "+998901234567", "Region", "District", "Address123", "short")]
    [InlineData("Alice", "Johnson", "+998910101010", "Region", "District", "Address789", "noSymbols")]
    [InlineData("Jane", "Smith", "+998912345678", "", "District", "Address123", "noUppercase")]
    [InlineData("Bob", "Johnson", "+998910101010", "Region", "", "Address789", "noDigit")]
    [InlineData("Chris", "Brown", "", "Region", "District", "Address123", "noLowercase")]
    [InlineData("d", "Taylor", "+998912345678", "Region", "District", "Address456", "short")]
    [InlineData("Elas,kejbala", "Adams", "+998901234567", "", "District", "Address123", "noSpecialCharacter")]
    [InlineData("Frank", "White", "+998912345678", "Region", "District", "", "longpasswordlongpasswordlongpasswordlongpasswordlongpasswordlongpassword")]
    [InlineData("Grdkhjsvflskdjbasleidfjbkalewsk,dace", "Moore", "+998910101010", "Region", "District", "Address789", "onlyDigits12345")]

    public void InvalidUserRegisterDto_ReturnsValidationErrors(
        string firstName, string lastName, string phoneNumber, string region, string district, string address, string password)
    {
        // Arrange
        var dto = new UserRegisterDto
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Region = region,
            District = district,
            Address = address,
            Password = password
        };

        UserRegisterValidator validationRules = new UserRegisterValidator();
        var result = validationRules.Validate(dto);

        Assert.False(result.IsValid);
    }

}

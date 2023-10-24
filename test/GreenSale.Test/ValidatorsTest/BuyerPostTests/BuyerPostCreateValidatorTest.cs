using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Persistence.Validators.BuyerPosts;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace GreenSale.Test.ValidatorsTest.BuyerPostTests;

public class BuyerPostCreateValidatorTest
{
    [Theory]
    [InlineData("+998941092151", "Title1", "Descriptisdasdvasdvsdon1", 100, 5, "kg", "Type1", "Region1", "District1", "Address1", 1, 4.5)]
    [InlineData("+998331092151", "Title2", "Description2", 200.0, 10, "lbs", "Type2", "Region2", "District2", "Address2", 2, 3.4)]
    [InlineData("+998912345678", "Title3", "Description3", 300.0, 15, "g", "Type3", "Region3", "District3", "Address3", 3, 2.5)]
    [InlineData("+998933344455", "Title4", "Description4", 400.0, 20, "kg", "Type4", "Region4", "District4", "Address4", 4, 1.4)]
    [InlineData("+998990011122", "Title5", "Description5", 500.0, 25, "lbs", "Type5", "Region5", "District5", "Address5", 5, 5.0)]
    [InlineData("+998977788899", "Title6", "Description6", 600.0, 30, "kg", "Type6", "Region6", "District6", "Address6", 6, 1.7)]
    [InlineData("+998955566677", "Title7", "Description7", 700.0, 35, "g", "Type7", "Region7", "District7", "Address7", 7, 3.1)]
    [InlineData("+998944433322", "Title8", "Description8", 800.0, 40, "lbs", "Type8", "Region8", "District8", "Address8", 8, 4.8)]
    [InlineData("+998933311100", "Title9", "Description9", 900.0, 45, "kg", "Type9", "Region9", "District9", "Address9", 9, 1.1)]
    [InlineData("+998922299988", "Title10", "Description10", 1000.0, 50, "g", "Type10", "Region10", "District10", "Address10", 10, 1.9)]

    public void CheckTrueTest(
        string phoneNumber, string title, string description,
        double price, int capacity, string capacityMeasure,
        string type, string region, string district, string address,
        long categoryID, long imagesiza)
    {
        byte[] byteImage = Encoding.UTF8.GetBytes("we sell an elejhsvfksjasdvasdvasdvasdvasdfvsadfvasdvdfa skasejfsevfbjk, " +
            "aewrvfctronic productslkzjbfvlzsd jvhk;oaspdvojisd pasdivj as;dovih aspdov haspdvhiasdpvoihas to our clients");

        long imageSizeInBytes = (long)(imagesiza * 1024 * 1024);
        IFormFile imageFile = new FormFile(new MemoryStream(byteImage), 0, imageSizeInBytes, "data", "file.png");
        List<IFormFile> ImgaAll = new List<IFormFile>();
        for (int i = 0; i < 5; i++)
        {
            ImgaAll.Add(imageFile);
        }
        var buyerPostCreateDto = new BuyerPostCreateDto
        {
            PhoneNumber = phoneNumber,
            Title = title,
            Description = description,
            Price = price,
            Capacity = capacity,
            CapacityMeasure = capacityMeasure,
            Type = type,
            Region = region,
            District = district,
            Address = address,
            CategoryID = categoryID,
            ImagePath = ImgaAll,
        };

        BuyerPostCreateValidator validations = new BuyerPostCreateValidator();
        var result = validations.Validate(buyerPostCreateDto);

        Assert.True(result.IsValid);
    }


    [Theory]
    [InlineData("+998 94 109 21 51", "Title1", "Descriptisdasdvasdvsdon1", 100, 5, "kg", "Type1", "Region1", "District1", "Address1", 1, 4.5)]
    [InlineData("998331092151", "Title2", "Description2", 200.0, 10, "lbs", "Type2", "Region2", "District2", "Address2", 2, 3.4)]
    [InlineData("912345678", "Title3", "Description3", 300.0, 15, "g", "Type3", "Region3", "District3", "Address3", 3, 2.5)]
    [InlineData("+998-93-334-44-55", "Title4", "Description4", 400.0, 20, "kg", "Type4", "Region4", "District4", "Address4", 4, 1.4)]
    [InlineData("+998990011122", "Ti", "Description5", 0, 25, "lbs", "Type5", "Region5", "District5", "Address5", 5, 5.0)]
    [InlineData("+998977788899", "", "Description6", 600.0, 30, "kg", "Type6", "Region6", "District6", "Address6", 6, 1.7)]
    [InlineData("+998955566677", "Title7", "", 700.0, 35, "g", "Type7", "", "District7", "Address7", 3, 3.1)]
    [InlineData("+998944433322", "Title8", "Description8", 800.0, null, "lbs", "Type8", "", "District8", "Address8", 8, 8)]
    [InlineData("+998933311100", "Title9", "Description9", 900.0, 45, "kg", "Type9", "Region9", "District9", "lbvsdfjbsldfkjvbsdlfvjkbsndfkvishbjdfnvs,dfhbvnjskudfjvnaskdrjfhvbnaskdjfvhbnaskjdfvhbanskvjdhbansrkvuhjbasn drkfvbjhmasbs drdvkjhabs rvvjhamsb dvjasdbfv smjdhfvbnkdufhjvn ", 9, 1.1)]
    [InlineData("+998922299988", "lbvsdfjbsldfkjvbsdlfvjkbsndfkvishbjdfnvs", "Description10", 1000.0, 50, "g", "Type10", "", "District10", "Address10", 10, 1.9)]
    // Yana inline data ma'lumotlarini qo'shing...
    public void CheckFalseTest(
    string phoneNumber, string title, string description,
    double price, int capacity, string capacityMeasure,
    string type, string region, string district, string address,
    long categoryID, long imagesiza)
    {
        byte[] byteImage = Encoding.UTF8.GetBytes("we sell an elejhsvfksjasdvasdvasdvasdvasdfvsadfvasdvdfa skasejfsevfbjk, " +
            "aewrvfctronic productslkzjbfvlzsd jvhk;oaspdvojisd pasdivj as;dovih aspdov haspdvhiasdpvoihas to our clients");

        long imageSizeInBytes = (long)(imagesiza * 1024 * 1024);
        IFormFile imageFile = new FormFile(new MemoryStream(byteImage), 0, imageSizeInBytes, "data", "file.png");
        List<IFormFile> ImgaAll = new List<IFormFile>();
        for (int i = 0; i < 5; i++)
        {
            ImgaAll.Add(imageFile);
        }
        var buyerPostCreateDto = new BuyerPostCreateDto
        {
            PhoneNumber = phoneNumber,
            Title = title,
            Description = description,
            Price = price,
            Capacity = capacity,
            CapacityMeasure = capacityMeasure,
            Type = type,
            Region = region,
            District = district,
            Address = address,
            CategoryID = categoryID,
            ImagePath = ImgaAll,
        };

        BuyerPostCreateValidator validations = new BuyerPostCreateValidator();
        var result = validations.Validate(buyerPostCreateDto);

        Assert.False(result.IsValid);
    }

}

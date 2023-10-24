using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Persistence.Validators.Storages;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace GreenSale.Test.ValidatorsTest.Storages;

public class StorageCreatedValidatorTest
{
    [Theory]
    [InlineData("Storage Name 1", "Valid description here 1", "Region 1", "District 1", "Address 1", "Info 1",
        41.12345, -71.98765, 3)]
    [InlineData("Storage Name 2", "Valid description here 2", "Region 2", "District 2", "Address 2", "Info 2",
        38.12345, -75.98765, 4)]
    [InlineData("Storage Name 3", "Valid description here 3", "Region 3", "District 3", "Address 3", "Info 3",
        42.12345, -70.98765, 2)]
    [InlineData("Storage Name 4", "Valid description here 4", "Region 4", "District 4", "Address 4", "Info 4",
        40.12345, -72.98765, 4.4)]
    [InlineData("Storage Name 5", "Valid description here 5", "Region 5", "District 5", "Address 5", "Info 5",
        39.12345, -74.98765, 2.4)]
    [InlineData("Storage Name 6", "Valid description here 6", "Region 6", "District 6", "Address 6", "Info 6",
        41.52345, -71.48765, 4.3)]
    [InlineData("Storage Name 7", "Valid description here 7", "Region 7", "District 7", "Address 7", "Info 7",
        38.82345, -75.28765, 5.0)]
    [InlineData("Storage Name 8", "Valid description here 8", "Region 8", "District 8", "Address 8", "Info 8",
        42.22345, -70.78765, 3)]
    [InlineData("Storage Name 9", "Valid description here 9", "Region 9", "District 9", "Address 9", "Info 9",
        40.32345, -72.18765, 2)]
    [InlineData("Storage Name 10", "Valid descriptionh10", "Region10", "District 10", "Address 10", "Info 10",
        39.42345, -74.38765, 3.8)]

    public void CheckTrueTest(
        string name, string description, string region, string district, string address, string info, double latitude,
            double longitude, double imagesiza)
    {
        byte[] byteImage = Encoding.UTF8.GetBytes("we sell an elejhsvfksjdfa skasejfsevfbjk, " +
            "aewrvfctronic products to our clients");

        long imageSizeInBytes = (long)(imagesiza * 1024 * 1024);
        IFormFile imageFile = new FormFile(new MemoryStream(byteImage), 0, imageSizeInBytes, "data", "file.png");


        var dto = new StoragCreateDto
        {
            Name = name,
            Description = description,
            Region = region,
            District = district,
            Address = address,
            Info = info,
            AddressLatitude = latitude,
            AddressLongitude = longitude,
            ImagePath = imageFile
        };

        StorageCreateValidator storageCreateValidator = new StorageCreateValidator();
        var result = storageCreateValidator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("djkbsljkb c", "Valid description here", "Region", "District", "Address", "Info", 41.12345, -71.98765, 7)]
    [InlineData("Storage Name", "", "Region", "District", "Address", "Info", 41.12345, -71.98765, 4.9)]
    [InlineData("Storage Name", "Valid description here", "", "District", "Address", "Info", 41.12345, -71.98765, 32.4)]
    [InlineData("Storage Name", "Valid description here", "Region", "", "Address", "Info", 41.12345, -71.98765, 90)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "", "Info", 41.12345, -71.98765, 3.7)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "Address", "", 41.12345, -71.98765, 5.1)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "Address", "Info", 0, -71.98765, 67)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "Address", "Info", 41.12345, 0, 6.0)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "Address", "Info", 41.12345, -71.98765, 7)]
    [InlineData("Storage Name", "Valid description here", "Region", "District", "Address", "Info", 41.12345, -71.98765, 6.5)]

    public void CheckFalsTest(
        string name, string description, string region, string district, string address, string info, double latitude,
        double longitude, double imgasize)
    {
        byte[] byteImage = Encoding.UTF8.GetBytes("we sell an electronic psdvsdts to our clients");
        long imageSizeInBytes = (long)(imgasize * 1024 * 1024);
        IFormFile imageFile = new FormFile(new MemoryStream(byteImage), 0, imageSizeInBytes, "data", "file.png");

        var dto = new StoragCreateDto
        {
            Name = name,
            Description = description,
            Region = region,
            District = district,
            Address = address,
            Info = info,
            AddressLatitude = latitude,
            AddressLongitude = longitude,
            ImagePath = imageFile
        };

        StorageCreateValidator storageCreateValidator = new StorageCreateValidator();
        var result = storageCreateValidator.Validate(dto);

        Assert.False(result.IsValid);
    }
}

using FluentValidation;
using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Persistence.Helpers;

namespace GreenSale.Persistence.Validators.Storages;

public class StorageUpdateValidator : AbstractValidator<StoragUpdateDto>
{
    public StorageUpdateValidator()
    {

        RuleFor(dto => dto.Name).NotNull().NotEmpty().WithMessage("Name field is required!")
            .MinimumLength(3).WithMessage("Name must be more than 3 characters")
                .MaximumLength(50).WithMessage("Name be less than 50 characters");

        RuleFor(dto => dto.Description).NotNull().NotEmpty().WithMessage("Description filed is required")
                .MinimumLength(15).WithMessage("Description must be more than 15 characters");

        RuleFor(dto => dto.Region).NotNull().NotEmpty().WithMessage("Region filed is required");
        RuleFor(dto => dto.District).NotNull().NotEmpty().WithMessage("District filed is required");
        RuleFor(dto => dto.Address).NotNull().NotEmpty().WithMessage("Address filed is required");

        RuleFor(dto => dto.Info).NotNull().NotEmpty().WithMessage("Info filed is required")
            .MinimumLength(3).WithMessage("Info must be more than 3 characters");

        RuleFor(dto => dto.AddressLatitude).NotNull().NotEmpty().WithMessage("Latitude filed is required");
        RuleFor(dto => dto.AddressLongitude).NotNull().NotEmpty().WithMessage("Longitude filed is required");

        //When(dto => dto.ImagePath is not null, () =>
        //{
        //    int maxImageSize = 5;
        //    RuleFor(dto => dto.ImagePath!.Length).LessThan(maxImageSize * 1024 * 1024 + 1).WithMessage($"Image size be less than {maxImageSize} MB");
        //    RuleFor(dto => dto.ImagePath!.FileName).Must(predicate =>
        //    {
        //        FileInfo fileInfo = new FileInfo(predicate);

        //        return MediaHelper.GetImageExtensions().Contains(fileInfo.Extension);
        //    }).WithMessage("This file type is not image file");
        //});
    }
}

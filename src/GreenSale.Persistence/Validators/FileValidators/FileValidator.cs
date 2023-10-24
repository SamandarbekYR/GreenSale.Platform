using FluentValidation;
using GreenSale.Persistence.Helpers;
using Microsoft.AspNetCore.Http;

namespace GreenSale.Persistence.Validators.FileValidators;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        int maxImageSize = 5;
        RuleFor(dto => dto.Length).NotEmpty().NotNull().WithMessage("Image filed is required!");

        RuleFor(dto => dto.Length).LessThan(maxImageSize * 1024 * 1024 + 1)
            .WithMessage($"Image size must be less than {maxImageSize} * MB");

        RuleFor(dto => dto.FileName).Must(predicate =>
        {
            FileInfo fileInfo = new FileInfo(predicate);

            return MediaHelper.GetImageExtensions().Contains(fileInfo.Extension);
        }).WithMessage("This file type is not image file");
    }
}

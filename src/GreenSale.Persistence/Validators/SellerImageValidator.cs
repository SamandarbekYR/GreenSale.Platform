using FluentValidation;
using GreenSale.Persistence.Dtos.SellerPostImageUpdateDtos;
using GreenSale.Persistence.Validators.FileValidators;

namespace GreenSale.Persistence.Validators;

public class SellerImageValidator : AbstractValidator<SellerPostImageUpdateDto>
{
    public SellerImageValidator()
    {
        RuleFor(x => x.ImagePath).SetValidator(new FileValidator());
    }
}

using FluentValidation;
using GreenSale.Persistence.Dtos.BuyerPostImageUpdateDtos;
using GreenSale.Persistence.Validators.FileValidators;

namespace GreenSale.Persistence.Validators;

public class BuyerImageValidator : AbstractValidator<BuyerPostImageDto>
{
    public BuyerImageValidator()
    {
        RuleFor(x => x.ImagePath).SetValidator(new FileValidator());
    }
}

using FluentValidation;
using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Persistence.Validators.FileValidators;

namespace GreenSale.Persistence.Validators;

public class StorageValidatorDto : AbstractValidator<StorageImageUpdateDto>
{
    public StorageValidatorDto()
    {
        RuleFor(x => x.StorageImage).SetValidator(new FileValidator());
    }
}

using FluentValidation;
using GreenSale.Persistence.Dtos.SellerPostsDtos;
using GreenSale.Persistence.Validators.FileValidators;

namespace GreenSale.Persistence.Validators.SellerPostValidators;

public class SellerPostCreatedValidators : AbstractValidator<SellerPostCreateDto>
{
    public SellerPostCreatedValidators()
    {
        RuleFor(dto => dto.PhoneNumber).Must(phone => PhoneNumberValidator.IsValid(phone))
        .WithMessage("Phone number is invalid! ex: +998xxYYYAABB");

        RuleFor(dto => dto.Title).NotEmpty().NotNull().WithMessage("Title field is required")
            .MinimumLength(3).WithMessage("Title must be more than 3 characters")
                .MaximumLength(50).WithMessage("Title must ber less than 50 characters");

        RuleFor(dto => dto.Description).NotNull().NotEmpty().WithMessage("Description field is required")
            .MinimumLength(10).WithMessage("Description must be more than 10 characters")
                .MaximumLength(50).WithMessage("Description must be less than 50 characters");

        RuleFor(dto => dto.Capacity).NotNull().NotEmpty().WithMessage("Capacity field is required");
        RuleFor(dto => dto.Price).NotNull().NotEmpty().WithMessage("Price field is required");

        RuleFor(dto => dto.CapacityMeasure).NotNull().NotEmpty().WithMessage("CapacityMeasure field is required")
            .MaximumLength(3).WithMessage("CapacityMeasure must be less than 3 characters");

        RuleFor(dto => dto.Type).NotEmpty().NotNull().WithMessage("Type filed is requires")
            .MinimumLength(3).WithMessage("Type must be more than 3 characters")
                .MaximumLength(50).WithMessage("Type must be less than 50 characters");

        RuleFor(dto => dto.Region).NotNull().NotEmpty().WithMessage("Region filed is required")
            .MinimumLength(3).WithMessage("Region must be more than 3 characters")
                .MaximumLength(50).WithMessage("Region must be less than characters");

        RuleFor(dto => dto.District).NotNull().NotEmpty().WithMessage("District field is required")
            .MinimumLength(3).WithMessage("District must be more than 3 characters")
                .MaximumLength(50).WithMessage("District must be more than 50 charaters");

        RuleFor(dto => dto.CategoryId).NotNull().NotEmpty().WithMessage("CategoryId filed is required");
        RuleFor(dto => dto.ImagePath).NotEmpty().NotNull().WithMessage("ImagePath filed is required");

        RuleFor(dto => dto.ImagePath).Must((dto, IFormFile) => IFormFile.Count <= 5)
            .WithMessage("There should be at least 5 images");
        RuleForEach(x => x.ImagePath).SetValidator(new FileValidator());
    }
}

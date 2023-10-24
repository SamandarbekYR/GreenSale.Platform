using FluentValidation;
using GreenSale.Persistence.Dtos.BuyerPostsDto;

namespace GreenSale.Persistence.Validators.BuyerPosts;

public class BuyerPostUpdateValidator : AbstractValidator<BuyerPostUpdateDto>
{
    public BuyerPostUpdateValidator()
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

        RuleFor(dto => dto.Address).NotEmpty().NotNull().WithMessage("Address field is requires")
            .MinimumLength(3).WithMessage("Addres must be more than 3 characters")
                .MaximumLength(50).WithMessage("Address must be less than 3 characters");

        ///RuleFor(dto => dto.CategoryID).NotNull().NotEmpty().WithMessage("CategoryId filed is required");
    }
}

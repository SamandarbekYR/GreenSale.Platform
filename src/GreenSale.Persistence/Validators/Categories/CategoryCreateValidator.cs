using FluentValidation;
using GreenSale.Persistence.Dtos.CategoryDtos;

namespace GreenSale.Persistence.Validators.Categories;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateValidator()
    {
        RuleFor(dto => dto.Name).NotNull().NotEmpty().WithMessage("Name field is required!")
           .MinimumLength(3).WithMessage("Name must be more than 3 characters")
                .MaximumLength(50).WithMessage("Name must be less than 50 characters");
    }
}

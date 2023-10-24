using FluentValidation;
using GreenSale.Persistence.Dtos.RoleDtos;

namespace GreenSale.Persistence.Validators.Roles
{
    public class RoleValidator : AbstractValidator<RoleCreatDto>
    {
        public RoleValidator()
        {
            RuleFor(dto => dto.Name).NotNull().NotEmpty().WithMessage("Name field is required!")
                .MinimumLength(3).WithMessage("Name must be more than 3 characters")
                    .MaximumLength(50).WithMessage("Name must be less than 50 characters");
        }
    }
}

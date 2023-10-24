using FluentValidation;
using GreenSale.Persistence.Dtos;

namespace GreenSale.Persistence.Validators.Dtos.AuthUserValidators;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(dto => dto.PhoneNumber).Must(phone => PhoneNumberValidator.IsValid(phone))
            .WithMessage("Phone number is invalid! ex: +998xxYYYAABB");
        RuleFor(dto => dto.password).NotNull().NotEmpty().WithMessage("FirstName is required!");
    }
}

using FluentValidation;
using GreenSale.Persistence.Dtos;
using GreenSale.Persistence.Validators;

namespace GreenSaleuz.Persistence.Validators.Dtos.AuthUserValidators;

public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterValidator()
    {
        RuleFor(dto => dto.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required!")
            .MinimumLength(3).WithMessage("FirstName must be less than 3 characters")
                .MaximumLength(30).WithMessage("FirstName must be less than 30 characters");

        RuleFor(dto => dto.LastName).NotNull().NotEmpty().WithMessage("LastName is required!")
            .MinimumLength(3).WithMessage("LastName must be less than 3 characters")
                .MaximumLength(30).WithMessage("LastName must be less than 30 characters");

        RuleFor(dto => dto.PhoneNumber).Must(phone => PhoneNumberValidator.IsValid(phone))
           .WithMessage("Phone number is invalid! ex: +998xxYYYAABB");

        RuleFor(dto => dto.Region).NotNull().NotEmpty().WithMessage("Region is required!")
            .MinimumLength(3).WithMessage("Region must be less than 3 characters")
                .MaximumLength(30).WithMessage("Region must be less than 30 characters");

        RuleFor(dto => dto.District).NotNull().NotEmpty().WithMessage("District is required!")
            .MinimumLength(3).WithMessage("District must be less than 3 characters")
                .MaximumLength(30).WithMessage("District must be less than 30 characters");

        RuleFor(dto => dto.Address).NotNull().NotEmpty().WithMessage("Address is required!")
            .MinimumLength(3).WithMessage("Address must be less than 3 characters")
                .MaximumLength(30).WithMessage("Address must be less than 30 characters");

        RuleFor(dto => dto.Password).Must(passsword => PasswordValidator.IsStrongPassword(passsword).IsValid);
    }
}
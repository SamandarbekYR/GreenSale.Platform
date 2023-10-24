using FluentValidation;
using GreenSale.Persistence.Dtos.UserDtos;

namespace GreenSale.Persistence.Validators.Users;

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        RuleFor(dto => dto.FirstName).MinimumLength(3).WithMessage("FirstName must be less than 3 characters")
                .MaximumLength(30).WithMessage("FirstName must be less than 30 characters");

        RuleFor(dto => dto.LastName).MinimumLength(3).WithMessage("LastName must be less than 3 characters")
                .MaximumLength(30).WithMessage("LastName must be less than 30 characters");

        RuleFor(dto => dto.PhoneNumber).Must(phone => PhoneNumberValidator.IsValid(phone))
           .WithMessage("Phone number is invalid! ex: +998xxYYYAABB");

        RuleFor(dto => dto.Region).MinimumLength(3).WithMessage("Region must be less than 3 characters")
                .MaximumLength(30).WithMessage("Region must be less than 30 characters");

        RuleFor(dto => dto.District)
            .MinimumLength(3).WithMessage("District must be less than 3 characters")
                .MaximumLength(30).WithMessage("District must be less than 30 characters");

        RuleFor(dto => dto.Address)
            .MinimumLength(3).WithMessage("Address must be less than 3 characters")
                .MaximumLength(30).WithMessage("Address must be less than 30 characters");
    }
}

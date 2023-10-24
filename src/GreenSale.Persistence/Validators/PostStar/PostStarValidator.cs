using FluentValidation;

namespace GreenSale.Persistence.Validators.PostStar;

public class PostStarValidator:AbstractValidator<int>
{
    public PostStarValidator()
    {
        RuleFor(star => star)
               .InclusiveBetween(0, 5)
               .WithMessage("Star must be an integer between 0 and 5");
    }
}

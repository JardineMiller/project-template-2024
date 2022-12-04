using FluentValidation;

namespace PlanningPoker.Application.Authentication.Queries.Login;

public class LoginQueryValidation : AbstractValidator<LoginQuery>
{
    public LoginQueryValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

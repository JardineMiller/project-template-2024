using FluentValidation;

namespace PlanningPoker.Application.Game.Queries;

public class GetGameQueryValidation : AbstractValidator<GetGameQuery>
{
    public GetGameQueryValidation()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

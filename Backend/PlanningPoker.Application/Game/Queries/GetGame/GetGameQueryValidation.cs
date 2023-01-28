using FluentValidation;

namespace PlanningPoker.Application.Game.Queries.GetGame;

public class GetGameQueryValidation : AbstractValidator<GetGameQuery>
{
    public GetGameQueryValidation()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

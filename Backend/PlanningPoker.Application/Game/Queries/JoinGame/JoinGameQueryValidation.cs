using FluentValidation;

namespace PlanningPoker.Application.Game.Queries.JoinGame;

public class JoinGameQueryValidation
    : AbstractValidator<JoinGameQuery>
{
    public JoinGameQueryValidation()
    {
        RuleFor(x => x.GameCode).NotEmpty();
        RuleFor(x => x.PlayerId).NotEmpty();
    }
}

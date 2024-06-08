using FluentValidation;

namespace ProjectTemplate2024.Application.Game.Queries.GetUserGames;

public class GetUserGamesQueryValidation
    : AbstractValidator<GetUserGamesQuery>
{
    public GetUserGamesQueryValidation()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

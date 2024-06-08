using FluentValidation;

namespace ProjectTemplate2024.Application.Game.Queries.GetGame;

public class GetGameQueryValidation : AbstractValidator<GetGameQuery>
{
    public GetGameQueryValidation()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}

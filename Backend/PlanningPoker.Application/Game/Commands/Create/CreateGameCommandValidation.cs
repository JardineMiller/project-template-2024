using FluentValidation;
using PlanningPoker.Application.Common.Validators;
using PlanningPoker.Domain.Common.Validation;

namespace PlanningPoker.Application.Game.Commands.Create;

public class CreateGameCommandValidation
    : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Validation.Game.Name.maxLength)
            .MinimumLength(Validation.Game.Name.minLength);

        RuleFor(x => x.Description)
            .MaximumLength(
                Validation.Game.Description.maxLength
            )
            .MinimumLength(
                Validation.Game.Description.minLength
            );

        RuleFor(x => x.OwnerId).IsGuid();
    }
}

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
            .MaximumLength(ValidationConstants.Game.Name.MaxLength)
            .MinimumLength(ValidationConstants.Game.Name.MinLength);

        RuleFor(x => x.Description)
            .MaximumLength(
                ValidationConstants.Game.Description.MaxLength
            )
            .MinimumLength(
                ValidationConstants.Game.Description.MinLength
            );

        RuleFor(x => x.OwnerId).IsGuid();
    }
}

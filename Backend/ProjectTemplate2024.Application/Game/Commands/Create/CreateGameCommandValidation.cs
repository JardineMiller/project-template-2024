using FluentValidation;
using ProjectTemplate2024.Application.Common.Validators;
using ProjectTemplate2024.Domain.Common.Validation;

namespace ProjectTemplate2024.Application.Game.Commands.Create;

public class CreateGameCommandValidation
    : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Validation.Game.Name.MaxLength)
            .MinimumLength(Validation.Game.Name.MinLength);

        RuleFor(x => x.Description)
            .MaximumLength(
                Validation.Game.Description.MaxLength
            )
            .MinimumLength(
                Validation.Game.Description.MinLength
            );

        RuleFor(x => x.OwnerId).IsGuid();
    }
}

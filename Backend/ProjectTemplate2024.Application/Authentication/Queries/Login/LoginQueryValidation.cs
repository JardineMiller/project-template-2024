﻿using FluentValidation;

namespace ProjectTemplate2024.Application.Authentication.Queries.Login;

public class LoginQueryValidation : AbstractValidator<LoginQuery>
{
    public LoginQueryValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

﻿namespace PlanningPoker.Contracts.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Token = null,
    string? PlayerId = null
);

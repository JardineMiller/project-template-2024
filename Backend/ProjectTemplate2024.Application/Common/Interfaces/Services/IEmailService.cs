﻿namespace PlanningPoker.Application.Common.Interfaces.Services;

public interface IEmailService
{
    void SendConfirmationEmail(
        string toEmail,
        string firstName,
        string token
    );

    void SendPasswordResetEmail(string toEmail, string token);
}

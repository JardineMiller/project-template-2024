using System;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Entities;
using ProjectTemplate2024.Infrastructure.Authentication;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Infrastructure.Tests.Authentication;

public class JwtGeneratorTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly JwtSettings _jwtSettings = new()
    {
        Secret = "extremely-very-incredibly-important-secret",
        Issuer = "issuer",
        Audience = "audience",
        ExpiryMinutes = 60,
    };

    private readonly User _user1 = new()
    {
        Id = Guid.NewGuid().ToString(),
        DisplayName = "test",
        Email = "test@user1.com",
    };

    public JwtGeneratorTests()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(new DateTime(2023, 1, 1));
    }

    [Fact]
    public void GenerateToken_GivenValidInput_ProvidesAToken()
    {
        // Arrange
        var generator = new TokenGenerator(
            _dateTimeProviderMock.Object,
            Options.Create(_jwtSettings)
        );

        // Act
        var token1 = generator.GenerateJwt(_user1);

        // Assert
        token1.ShouldNotBeNull();
    }
}

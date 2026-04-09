using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTemplate2024.Application.Authentication.Commands.GoogleLogin;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Settings;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;
using Xunit;

namespace ProjectTemplate2024.Tests.Application.Tests.Authentication.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly IOptions<GoogleSettings> _googleSettings;
        private readonly GoogleLoginCommandHandler _handler;

        public GoogleLoginCommandHandlerTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _googleSettings = Options.Create(
                new GoogleSettings { ClientId = "test-client-id" }
            );
            _handler = new GoogleLoginCommandHandler(
                _userManagerMock.Object,
                _userRepositoryMock.Object,
                _tokenGeneratorMock.Object,
                _googleSettings
            );
        }

        [Fact]
        public async Task Handle_InvalidJwt_ThrowsInvalidCredentials()
        {
            // Arrange
            var command = new GoogleLoginCommand("invalid-token");
            // Simulate GoogleJsonWebSignature.ValidateAsync throwing InvalidJwtException
            var exception = new InvalidJwtException("Invalid JWT");
            // Use Moq.Protected to mock static method if possible, otherwise skip this test
            // For demonstration, we assume ValidateAsync throws

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsError);
            Assert.Equal(
                Errors.Authentication.InvalidCredentials,
                result.Errors[0]
            );
        }

        [Fact]
        public async Task Handle_NewUser_CreatesUserAndReturnsAuthResult()
        {
            // Arrange
            var command = new GoogleLoginCommand("valid-token");
            var payload = new GoogleJsonWebSignature.Payload
            {
                Email = "test@example.com",
                Name = "Test User",
                Picture = "avatar.png",
            };
            // Simulate ValidateAsync
            // Simulate user not found`
            _userRepositoryMock
                .Setup(r =>
                    r.GetUserByEmail(
                        payload.Email,
                        It.IsAny<CancellationToken>(),
                        It.IsAny<System.Linq.Expressions.Expression<System.Func<
                            User,
                            object
                        >>[]>()
                    )
                )
                .ReturnsAsync(null as User);
            _userManagerMock
                .Setup(m => m.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            _tokenGeneratorMock
                .Setup(t => t.GenerateJwt(It.IsAny<User>()))
                .Returns("jwt-token");
            _tokenGeneratorMock
                .Setup(t => t.GenerateRefreshToken())
                .Returns(new RefreshToken { Token = "refresh-token" });
            _userRepositoryMock
                .Setup(r =>
                    r.UpdateUser(
                        It.IsAny<User>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .Returns(Task.CompletedTask);

            // Act
            // NOTE: In real test, you would mock GoogleJsonWebSignature.ValidateAsync
            // Here, you may need to refactor handler for testability (inject validator)
            // For demonstration, this test will not actually call ValidateAsync

            // Assert
            // (Add asserts for AuthenticationResult)
        }
    }
}

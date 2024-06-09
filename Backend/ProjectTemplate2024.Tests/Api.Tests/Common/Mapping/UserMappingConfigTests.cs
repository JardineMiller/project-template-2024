using Mapster;
using ProjectTemplate2024.Api.Common.Mapping;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Domain.Entities;
using Shouldly;
using Xunit;

namespace ProjectTemplate2024.Application.Tests.Api.Tests.Common.Mapping;

public class UserMappingConfigTests
{
    public UserMappingConfigTests()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        UserMappingConfig.AddConfig(config);
    }

    [Fact]
    public void GetUserDetailsResult_ShouldMapTo_GetUserDetailsResponse_WithNullToken()
    {
        var user = new User
        {
            Id = "Id",
            DisplayName = "DisplayName",
            Email = "Email",
            Bio = "Bio",
            AvatarFileName = "filename.jpg"
        };

        var src = new GetUserDetailsResult(user, user.AvatarFileName);

        var result = src.Adapt<GetUserDetailsResponse>();

        result.Id.ShouldBe(user.Id);
        result.DisplayName.ShouldBe(user.DisplayName);
        result.Email.ShouldBe(user.Email);
        result.Bio.ShouldBe(user.Bio);
        result.AvatarUrl.ShouldBe(user.AvatarFileName);
    }

    [Fact]
    public void GetUserDetailsResult_ShouldMapTo_GetUserDetailsResponse_WithNull()
    {
        var user = new User
        {
            Id = "Id",
            DisplayName = "DisplayName",
            Email = "Email",
        };

        var src = new GetUserDetailsResult(user, user.AvatarFileName);

        var result = src.Adapt<GetUserDetailsResponse>();

        result.Id.ShouldBe(user.Id);
        result.DisplayName.ShouldBe(user.DisplayName);
        result.Email.ShouldBe(user.Email);
        result.Bio.ShouldBe(null);
        result.AvatarUrl.ShouldBe(null);
    }
}

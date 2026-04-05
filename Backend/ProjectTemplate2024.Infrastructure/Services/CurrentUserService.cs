using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ProjectTemplate2024.Application.Common.Interfaces.Services;

namespace ProjectTemplate2024.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _user;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User!;
    }

    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;

    public string? UserName
    {
        get
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            return _user
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)
                ?.Value;
        }
    }

    public string? UserId
    {
        get
        {
            if (!IsAuthenticated)
            {
                return null;
            }

            return _user
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}

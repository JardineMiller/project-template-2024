using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PlanningPoker.Application.Common.Interfaces.Services;

namespace PlanningPoker.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _user;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor
    )
    {
        this._user = httpContextAccessor.HttpContext?.User!;
    }

    public bool IsAuthenticated =>
        this._user.Identity?.IsAuthenticated ?? false;

    public string? UserName
    {
        get
        {
            if (!this.IsAuthenticated)
            {
                return null;
            }

            return this._user.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.GivenName)
                ?.Value;
        }
    }

    public string? UserId
    {
        get
        {
            if (!this.IsAuthenticated)
            {
                return null;
            }

            return this._user.Claims
                .FirstOrDefault(
                    x => x.Type == ClaimTypes.NameIdentifier
                )
                ?.Value;
        }
    }
}

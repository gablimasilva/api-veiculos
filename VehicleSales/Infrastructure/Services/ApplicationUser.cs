using Application.Exceptions;
using Application.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public sealed class ApplicationUser : IApplicationUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                       .User?
                       .FindFirst("sub")?
                       .Value
                   ?? throw new UnauthorizedException(
                       "User is not authenticated.");
        }
    }
}
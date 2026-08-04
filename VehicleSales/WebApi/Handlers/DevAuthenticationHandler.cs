using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Handlers
{
    public sealed class DevAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DevAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "LOCAL-TEST"),
            new Claim("sub", "LOCAL-TEST"),
            new Claim(ClaimTypes.Name, "Gabriel")
        };

            var identity = new ClaimsIdentity(
                claims,
                Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(
                principal,
                Scheme.Name);

            return Task.FromResult(
                AuthenticateResult.Success(ticket));
        }
    }
}

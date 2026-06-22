using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BenchPulse.Api.Filters;

public class CompanyDomainFilter : IAuthorizationFilter
{
    private readonly string _allowedDomain;

    public CompanyDomainFilter(IConfiguration config)
    {
        _allowedDomain = config["Auth:AllowedEmailDomain"] ?? "@symphony.is";
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var email = context.HttpContext.User.FindFirstValue(ClaimTypes.Email)
                    ?? context.HttpContext.User.FindFirstValue("email");

        if (email == null || !email.EndsWith(_allowedDomain, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
        }
    }
}
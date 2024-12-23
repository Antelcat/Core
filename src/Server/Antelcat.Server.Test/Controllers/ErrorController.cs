using System.Net;
using System.Security.Claims;
using Antelcat.Server.Controllers;
using Antelcat.Server.Exceptions;
using Antelcat.Server.Test.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ErrorController : IdentityController<User,ErrorController>
{
    [HttpGet("{statusCode:int}")]
    public IActionResult Error([FromRoute] int? statusCode = 0)
    {
        throw (RejectException)(statusCode ?? 500,
            new
            {
                code    = 0,
                message = "Error occurred"
            }
            , "This is an error defined");
    }


    [HttpPost]
    public async Task<IActionResult> LoginMultiClaim([FromBody] List<Tuple<string, string>> args)
    {
        await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(
                args.Select(x => new Claim(x.Item1, x.Item2)),
                "Identity.Application")),
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.MaxValue
            });
        return Ok();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public List<(string Key, string Value)> GetClaim()
    {
        return HttpContext.User.Claims.Select(x => (x.Type, x.Value)).ToList();
    }
}
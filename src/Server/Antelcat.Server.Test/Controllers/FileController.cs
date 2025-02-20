using System.Net.Mime;
using System.Web;
using Antelcat.Server.Controllers;
using Antelcat.Server.Test.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FileController : IdentityController<User>
{
    [HttpGet("{fileName}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Assets([FromRoute] string fileName = "Logo.png")
    {
        var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", HttpUtility.UrlDecode(fileName));
        return System.IO.File.Exists(path)
            ? new FileContentResult(await System.IO.File.ReadAllBytesAsync(path), MediaTypeNames.Image.Jpeg)
            : new NotFoundResult();
    }
}
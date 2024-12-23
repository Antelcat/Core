using Antelcat.DependencyInjectionEx.Autowired;
using Antelcat.Extensions;
using Antelcat.Server.Controllers;
using Antelcat.Server.Test.Models;
using Antelcat.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers
{
    [ApiController]
    [Route($"{nameof(Antelcat)}/[controller]")]
    public class AuthorizeController : IdentityController<User,AuthorizeController>
    {
        [Autowired] private readonly JwtConfigure<User> configure = null!;


        [HttpPost(nameof(JwtLogin))]
        [AllowAnonymous]
        public HttpPayload<string> JwtLogin([FromBody] User user)
        {
            return new HttpPayload<string>($"Bearer {configure.CreateToken(user)}");
        }

        [HttpPost(nameof(CookieLogin))]
        [AllowAnonymous]
        public async Task<HttpPayload> CookieLogin([FromBody] User user)
        {
            await SignInAsync(user, "User",
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.MaxValue
                });
            return (1, "登录成功");
        }

        [HttpGet(nameof(CookieLogout))]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<HttpPayload> CookieLogout()
        {
            await SignOutAsync();
            return (1, "登出成功");
        }

        [HttpGet(nameof(WhoAmICookie))]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public HttpPayload<User> WhoAmICookie()
        {
            var user = Identity!;
            Logger.LogTrace("{Serialize}", user.Serialize());
            return user;
        }

        [HttpGet(nameof(DoctorAllowed))]
        [Authorize(Roles = "Doctor", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public HttpPayload<User> DoctorAllowed()
        {
            var user = Identity!;
            Logger.LogTrace("{Serialize}", user.Serialize());
            return user;
        }

        [HttpGet(nameof(WhoAmIJwt))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public HttpPayload<User> WhoAmIJwt()
        {
            var user = Identity!;
            Logger.LogTrace("{Serialize}", user.Serialize());
            return user;
        }
    }
}
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Models;
using Antelcat.Foundation.Server.Controllers;
using Antelcat.Foundation.Server.Utils;
using Feast.Foundation.Server.Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Test.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]

    public class HomeController : FeastController<User>
    {
        private JwtConfigure<User> configure;

        public HomeController(JwtConfigure<User> configure)
        {
            this.configure = configure;
        }

        [HttpPost]
        [AllowAnonymous]
        public Response<string> Login([FromBody]User user)
        {
            return new Response<string>(configure.CreateToken(user)!);
        }
        
        [HttpPost]
        public Response<User> WhoAmI()
        {
            Logger.LogTrace($"{Identity}");
            return Identity;
        }
    }
}

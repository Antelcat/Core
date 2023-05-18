using Feast.Foundation.Core.Extensions;
using Feast.Foundation.Core.Models;
using Feast.Foundation.Server.Controllers;
using Feast.Foundation.Server.Test.Models;
using Feast.Foundation.Server.Utils;
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

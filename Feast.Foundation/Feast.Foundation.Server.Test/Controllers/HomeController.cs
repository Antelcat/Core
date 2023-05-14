using Feast.Foundation.Core.Models;
using Feast.Foundation.Server.Test.Models;
using Feast.Foundation.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Test.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private JwtConfigure<User> configure;

        public HomeController(JwtConfigure<User> configure)
        {
            this.configure = configure;
        }

        [HttpPost]
        public Response<string> Login([FromBody]User user)
        {
            return new Response<string>(configure.CreateToken(user));
        }
    }
}

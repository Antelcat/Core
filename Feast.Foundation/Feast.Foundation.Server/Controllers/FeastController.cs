using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Feast.Foundation.Server.Controllers
{
    public abstract class FeastController : Controller
    {
        protected FeastController() { }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Debugger.Break();
            return base.OnActionExecutionAsync(context, next);
        }
    }
}

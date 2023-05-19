using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace Antelcat.Foundation.Server.Filters
{
    public sealed class AuthorizationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Clear();
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor descriptor) return;
            if (!WhetherActionNeedAuth(descriptor)) return;
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = nameof(Authorization),
                AllowEmptyValue = true,
                In = ParameterLocation.Header,
                Required = true,
                Description = "This action need auth"
            });
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            };
            operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
        }
        private static bool WhetherActionNeedAuth(ControllerActionDescriptor descriptor)
        {
            var tmp = AnalyzeAuthRequired(descriptor.MethodInfo.GetCustomAttributes(true));
            return  tmp == -1 
                ? AnalyzeAuthRequired(descriptor.ControllerTypeInfo.GetCustomAttributes(true)) == 1 
                : tmp == 1;
        }
        private static int AnalyzeAuthRequired(IEnumerable<object> attrs)
        {
            foreach (var attr in attrs)
            {
                switch (attr)
                {
                    case AuthorizeAttribute:
                        return 1;
                    case AllowAnonymousAttribute:
                        return 0;
                }
            }
            return -1;
        }
    }
}
using System.Net;
using Antelcat.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Antelcat.Server.Filters;

[Serializable]
public sealed class AuthorizationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Clear();
        string? scheme;
        switch (context.ApiDescription.ActionDescriptor)
        {
            case ControllerActionDescriptor descriptor:
                if ((scheme = WhetherActionNeedAuth(descriptor)).IsNullOrEmpty())
                    return;
                break;
            default:
                if ((scheme = AnalyzeAuthRequired(context.ApiDescription.ActionDescriptor.EndpointMetadata)).IsNullOrEmpty())
                    return;
                break;
        }

        switch (scheme)
        {
            case JwtBearerDefaults.AuthenticationScheme:
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name            = nameof(Authorization),
                    AllowEmptyValue = true,
                    In              = ParameterLocation.Header,
                    Required        = true,
                    Description     = "This action need auth",
                });
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            },
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                };
                operation.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                break;
            case CookieAuthenticationDefaults.AuthenticationScheme:
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name            = nameof(Cookie),
                    AllowEmptyValue = true,
                    In              = ParameterLocation.Cookie,
                    Required        = true,
                    Description     = "This action need cookie"
                });
                break;
        }

    }

    private static string? WhetherActionNeedAuth(ControllerActionDescriptor descriptor)
    {
        var tmp = AnalyzeAuthRequired(descriptor.MethodInfo.GetCustomAttributes(true));
        return tmp.IsNullOrEmpty()
            ? AnalyzeAuthRequired(descriptor.ControllerTypeInfo.GetCustomAttributes(true))
            : tmp;
    }
    private static string? AnalyzeAuthRequired(IEnumerable<object> attrs)
    {
        foreach (var attr in attrs)
        {
            switch (attr)
            {
                case AuthorizeAttribute attribute:
                    return attribute.AuthenticationSchemes;
                case AllowAnonymousAttribute:
                    return string.Empty;
            }
        }
        return null;
    }
}
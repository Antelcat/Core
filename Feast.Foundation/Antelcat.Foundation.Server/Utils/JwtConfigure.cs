﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Antelcat.Foundation.Server.Extensions;

namespace Antelcat.Foundation.Server.Utils;

public class JwtConfigure<TIdentity>
{
    public JwtConfigure(Action<TokenValidationParameters>? paramConfig = null)
    {
        Secret = Guid.NewGuid().ToString();
        paramConfig?.Invoke(Parameters);
    }
  
    public string Secret { init => SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value)); }

    private SecurityKey? SecurityKey
    {
        get => securityKey;
        init
        {
            securityKey = value;
            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
    }
    private readonly SecurityKey? securityKey;
    private readonly SigningCredentials? credentials;

    private JwtSecurityToken GetToken(IEnumerable<Claim?> claims) => new(
        Parameters.ValidIssuer,
        Parameters.ValidAudience,
        claims,
        signingCredentials: credentials,
        notBefore: DateTime.Now);

    public TokenValidationParameters Parameters
    {
        get => parameters ??= new()
        {
            ValidIssuer = Assembly.GetExecutingAssembly().GetName().Name,
            ValidAudience = Assembly.GetExecutingAssembly().GetName().Name,
            ValidateIssuer = true,
            IssuerSigningKey = SecurityKey,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
        init => parameters = value;
    }

    private readonly JwtSecurityTokenHandler handler = new();
    private TokenValidationParameters? parameters;
    public string? CreateToken(TIdentity source)
    {
        try
        {
            return handler.WriteToken(GetToken(JwtExtension<TIdentity>.GetClaims(source)));
        }
        catch
        {
            return null;
        }
    }
}
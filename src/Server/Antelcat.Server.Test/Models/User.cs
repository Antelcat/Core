using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Antelcat.ClaimSerialization.ComponentModel;

namespace Antelcat.Server.Test.Models;

public partial class User
{
    public required                              int    Id   { get; init; } = 10000;
    public required                              string Name { get; init; } = "admin";
    [ClaimType(ClaimTypes.Role)] public required string Role { get; init; } = "Doctor";
}
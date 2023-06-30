using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;

namespace EFCore.Entities;

public class User: IdentityUser
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}
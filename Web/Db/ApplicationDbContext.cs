using Duende.IdentityServer.EntityFramework.Options;
using EFCore.Entities;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Web.Db;

public class ApplicationDbContext : ApiAuthorizationDbContext<User>
{
    public override DbSet<User> Users { get; set; }
    
    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) :
        base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<IdentityRole>().ToTable("Role");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
    }
}
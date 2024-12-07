using AradaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Data
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "f59b1d3c-8245-4250-920c-f94e70cbaefa";

            // Create Admin and Cashier Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Create Admin and Cashier Users
            var adminUserId1 = "b98bbc8-e2c0-4ecd-9f33-dd46ccb4fde4";
            var adminUserId2 = "55a1bccc-0507-4c89-a818-0b4a0bce99eb";

            var admin1 = new User
            {
                Id = adminUserId1,
                UserName = "AradaAlaa",
                NormalizedUserName = "ARADAALAA",
            };
            var admin2 = new User
            {
                Id = adminUserId2,
                UserName = "AradaAhmed",
                NormalizedUserName = "ARADAAHMED",
            };


            // Hash passwords for seeded users
            var passwordHasher = new PasswordHasher<User>();
            admin1.PasswordHash = passwordHasher.HashPassword(admin1, "151020001987");
            admin2.PasswordHash = passwordHasher.HashPassword(admin2, "131119872000");


            builder.Entity<User>().HasData(admin1, admin2);

            // Assign Roles to Users
            var adminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string> { UserId = adminUserId1, RoleId = adminRoleId },
                new IdentityUserRole<string> { UserId = adminUserId2, RoleId = adminRoleId },
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}

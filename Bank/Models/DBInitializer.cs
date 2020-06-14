using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Models
{
    public class DbInitializer
    {
        private BankAppDataContext _context;
        
        public DbInitializer(BankAppDataContext context)
        {
            _context = context;
        }

        public async Task Initialize()
        {
            var admin = new IdentityUser
            {
                UserName = "stefan.holmberg@systementor.se",
                NormalizedUserName = "stefan.holmberg@systementor.se",
                Email = "stefan.holmberg@systementor.se",
                NormalizedEmail = "stefan.holmberg@systementor.se",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStoreAdmin = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStoreAdmin.CreateAsync(new IdentityRole {Name = "Admin", NormalizedName = "ADMIN"});
            }

            if (!_context.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(admin, "Hejsan123#");
                admin.PasswordHash = hashed;
                var userStoreAdmin = new UserStore<IdentityUser>(_context);
                await userStoreAdmin.CreateAsync(admin);
                await userStoreAdmin.AddToRoleAsync(admin, "ADMIN");
            }

            var cashier = new IdentityUser
            {
                UserName = "stefan.holmberg@nackademin.se",
                NormalizedUserName = "stefan.holmberg@nackademin.se",
                Email = "stefan.holmberg@nackademin.se",
                NormalizedEmail = "stefan.holmberg@nackademin.se",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStoreCashier = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "Cashier"))
            {
                await roleStoreCashier.CreateAsync(new IdentityRole { Name = "Cashier", NormalizedName = "CASHIER" });
            }

            if (!_context.Users.Any(u => u.UserName == cashier.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(cashier, "Hejsan123#");
                cashier.PasswordHash = hashed;
                var userStoreCashier = new UserStore<IdentityUser>(_context);
                await userStoreCashier.CreateAsync(cashier);
                await userStoreCashier.AddToRoleAsync(cashier, "CASHIER");
            }

            await _context.SaveChangesAsync();
        }
    }
}
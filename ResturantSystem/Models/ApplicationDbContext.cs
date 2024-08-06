using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResturantSystem.Models;
using System;
using System.Reflection.Emit;

namespace Auth.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users","Security");
            builder.Entity<IdentityRole>().ToTable("Roles" , "Security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");

            builder.Entity<Table>().HasKey(t => new { t.TableNo, t.BranchId });

            //IsExist relationship
            builder.Entity<Branch>()
                .HasMany(t => t.Tables)
                .WithOne(t => t.Branch)
                .HasForeignKey(t => t.BranchId);

            //WorkIn relationship
            builder.Entity<Branch>()
                .HasMany(t => t.Employees)
                .WithOne(t => t.Branch)
                .HasForeignKey(t => t.BranchId);

            //Manage relationship
            builder.Entity<Branch>()
                .HasOne(t => t.Manager)
                .WithOne()
                .HasForeignKey<Branch>(t => t.ManagerId);

            //Offer relationship
            builder.Entity<Branch>()
                .HasMany(t => t.Products)
                .WithMany(t => t.Branches)
                .UsingEntity(t => t.ToTable("BranchesProducts"));

            //Contain relationship
            builder.Entity<Order>()
                .HasMany(t => t.Products)
                .WithMany(t => t.Orders)
                .UsingEntity(t => t.ToTable("OrdersProducts"));

            //Contain relationship
            builder.Entity<Product>()
                .HasMany(p => p.Ingredients)
                .WithMany(p => p.Products)
                .UsingEntity<Dictionary<string, object>>(
                "ProductsIngredients",
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductsId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("IngredientsId")
                    .OnDelete(DeleteBehavior.Restrict))
                    .HasKey("ProductsId", "IngredientsId");
        }



    }
    
}

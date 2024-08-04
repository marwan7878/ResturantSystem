using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Security;

#nullable disable

namespace Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO[Security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) " +
                "VALUES(N'2b723733-db64-4941-ad51-b9a6cd6a7ed4', N'admin', N'ADMIN', N'admin@test.com', N'ADMIN@TEST.COM', 1, N'AQAAAAIAAYagAAAAEM5wZC3n6IDjSsIyq6NaetoV28v1finfWp7gKb996gfLnXUVIRRKe2klGwZZnTq6BA == ', N'J4DKXSYWRN56PRA7HCKSWVCZFXK3J3AZ', N'61745ffe-2ea9-4112-af2b-45e7c4d89a84', NULL, 0, 0, NULL, 1, 0, N'Marwan ', N'Mohamed', NULL)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Security].[Users] WHERE Id = '2b723733-db64-4941-ad51-b9a6cd6a7ed4'");
        }
    }
}

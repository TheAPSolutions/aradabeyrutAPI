using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MenuAppAPI.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class initialAuthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bf50746d-89a1-4260-912c-beb0ac4562b5", "8552276d-5fed-4ab9-8513-e2e5b868cf45" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bf50746d-89a1-4260-912c-beb0ac4562b5", "a63bbcf0-da35-4d1b-8109-47ab589a083c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bf50746d-89a1-4260-912c-beb0ac4562b5", "abea63e9-3bfb-4564-9a61-5723920767c8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf50746d-89a1-4260-912c-beb0ac4562b5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8552276d-5fed-4ab9-8513-e2e5b868cf45");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a63bbcf0-da35-4d1b-8109-47ab589a083c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "abea63e9-3bfb-4564-9a61-5723920767c8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "55a1bccc-0507-4c89-a818-0b4a0bce99eb",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "2a9ca489-ddca-42a4-90a0-7364f5f1eb45", "ARADAAHMED", "AQAAAAIAAYagAAAAEDFUj+zo+Z9s20cJp20OJwgdJmOSuGrbknuTbj87uo+tA7aZDGz66DIfysItrIthKQ==", "8e8c4267-f13e-4483-9a75-a91010e449c4", "AradaAhmed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b98bbc8-e2c0-4ecd-9f33-dd46ccb4fde4",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "3dc5cab5-7ea7-4d08-a7d2-ba06419e05a7", "ARADAALAA", "AQAAAAIAAYagAAAAEJJPWl1IxE6c7X+tHnEcWp87hahfImzTFYfXV6ruP/Oyxd9GeAB3c6QynWDUwwVQ8Q==", "82df87c1-b4d3-4449-a4b1-34789f1b216f", "AradaAlaa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bf50746d-89a1-4260-912c-beb0ac4562b5", "bf50746d-89a1-4260-912c-beb0ac4562b5", "Cashier", "CASHIER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "55a1bccc-0507-4c89-a818-0b4a0bce99eb",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "c2034c23-0b59-464a-8c55-01c798770169", "HUNGRYAHMED", "AQAAAAIAAYagAAAAENGQXElQV1khqm23XMI+52GhiM4fZ5mnzkSOM2Yag0zinwY9cUcJ854PjDime8X+ew==", "37e61c04-50ea-44d3-9385-d12c14aba039", "HungryAhmed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b98bbc8-e2c0-4ecd-9f33-dd46ccb4fde4",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "abadef32-7f10-4234-9595-dd6a6a788290", "HUNGRYALAA", "AQAAAAIAAYagAAAAEPtLvedFMKEoPGjkndVRpqvtEDM1yz+kyIhTfKYlXgZH3ZPFqq0NqEGv+wsnTYWXvQ==", "41af9f6e-089a-4d40-9410-60ecd236d2d5", "HungryAlaa" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8552276d-5fed-4ab9-8513-e2e5b868cf45", 0, 3, "9ff1bc64-e669-40b5-9c0a-2fa65ab53144", null, false, false, null, null, "BATISEHIRUSER", "AQAAAAIAAYagAAAAEPn/xmdZ9DxhQ1G52x7+awcHiT0PFQQOXYWvbs2kD6vvCD2IzPL18oaey8u5/RprvA==", null, false, "2ef97330-c50b-4bc9-95d9-130ca61681eb", false, "Batisehiruser" },
                    { "a63bbcf0-da35-4d1b-8109-47ab589a083c", 0, 2, "519e3551-c36b-4199-915e-f890f443a1a8", null, false, false, null, null, "MASLAKUSER", "AQAAAAIAAYagAAAAEMCULVDuUB7OWTWgNOtpTVjylLczV3zBH5QsgAEnwoiLA9FefDY9TXeJcXWdjRqIxg==", null, false, "ec73ed4b-a2a5-40fd-9dd6-5afdea07efda", false, "Maslakuser" },
                    { "abea63e9-3bfb-4564-9a61-5723920767c8", 0, 1, "a9f44ec5-55ce-4730-93e9-47c0ca5c6d00", null, false, false, null, null, "BESIKTASUSER", "AQAAAAIAAYagAAAAECDP4FzEXfA2MAyr/tyIne2JOC/XqwkR+9aopeGIl1KihemPOP7eMuFSlzgzkHvo5A==", null, false, "1dc390b8-b108-4e8b-9a6c-651761ee83d7", false, "Besiktasuser" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bf50746d-89a1-4260-912c-beb0ac4562b5", "8552276d-5fed-4ab9-8513-e2e5b868cf45" },
                    { "bf50746d-89a1-4260-912c-beb0ac4562b5", "a63bbcf0-da35-4d1b-8109-47ab589a083c" },
                    { "bf50746d-89a1-4260-912c-beb0ac4562b5", "abea63e9-3bfb-4564-9a61-5723920767c8" }
                });
        }
    }
}

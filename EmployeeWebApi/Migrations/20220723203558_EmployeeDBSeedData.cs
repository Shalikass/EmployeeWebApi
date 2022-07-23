using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeWebApi.Migrations
{
    public partial class EmployeeDBSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("80573d10-ee54-45f9-b93c-dad35adf5990"), "Big booss", "CEO" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("b4058a62-21e7-4e5e-8ad1-f745e484f733"), "Manages stuff", "Manager" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("bc89ba1f-8873-413d-94a1-316e0b79624d"), null, "Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "BirthDate", "BossId", "CurrentSalary", "EmploymentDate", "FirstName", "HomeAddress", "LastName", "RoleId" },
                values: new object[] { new Guid("0ac8137c-30f4-4441-a653-a79f59f6c96c"), new DateTime(1986, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3000m, new DateTime(2022, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Egle", "Latvia\r\n Riga, Mieto pr. 18-4", "Megle", new Guid("80573d10-ee54-45f9-b93c-dad35adf5990") });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "BirthDate", "BossId", "CurrentSalary", "EmploymentDate", "FirstName", "HomeAddress", "LastName", "RoleId" },
                values: new object[] { new Guid("430b0fd2-4470-469e-9a89-0d19c0e639a2"), new DateTime(1993, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0ac8137c-30f4-4441-a653-a79f59f6c96c"), 2300m, new DateTime(1994, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jonas", "Lithuania\r\n Klaipeda, Kranto g. 16A", "Babonas", new Guid("b4058a62-21e7-4e5e-8ad1-f745e484f733") });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "BirthDate", "BossId", "CurrentSalary", "EmploymentDate", "FirstName", "HomeAddress", "LastName", "RoleId" },
                values: new object[] { new Guid("bb8f7d67-9e90-426c-a776-bc1ff87d2225"), new DateTime(1995, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("430b0fd2-4470-469e-9a89-0d19c0e639a2"), 2450m, new DateTime(2000, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vytas", "Lithuania\r\n Vilnius, Gelezinio vilko g. 24", "Schmytas", new Guid("bc89ba1f-8873-413d-94a1-316e0b79624d") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("0ac8137c-30f4-4441-a653-a79f59f6c96c"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("430b0fd2-4470-469e-9a89-0d19c0e639a2"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("bb8f7d67-9e90-426c-a776-bc1ff87d2225"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("80573d10-ee54-45f9-b93c-dad35adf5990"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b4058a62-21e7-4e5e-8ad1-f745e484f733"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bc89ba1f-8873-413d-94a1-316e0b79624d"));
        }
    }
}

using EmployeeWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebApi.DbContexts
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public EmployeeContext(DbContextOptions<EmployeeContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                 new
                 {
                     Id = Guid.Parse("bb8f7d67-9e90-426c-a776-bc1ff87d2225"),
                     FirstName = "Vytas",
                     LastName = "Schmytas",
                     BirthDate = new DateTime(1995, 9, 3),
                     BossId = Guid.Parse("430b0fd2-4470-469e-9a89-0d19c0e639a2"),
                     CurrentSalary = 2450m,
                     EmploymentDate = new DateTime(2000, 8, 29),
                     RoleId = Guid.Parse("bc89ba1f-8873-413d-94a1-316e0b79624d"),
                     HomeAddress = "Lithuania\r\n Vilnius, Gelezinio vilko g. 24"
                 },
                new
                {
                    Id = Guid.Parse("430b0fd2-4470-469e-9a89-0d19c0e639a2"),
                    FirstName = "Jonas",
                    LastName = "Babonas",
                    BirthDate = new DateTime(1993, 9, 11),
                    BossId = Guid.Parse("0ac8137c-30f4-4441-a653-a79f59f6c96c"),
                    CurrentSalary = 2300m,
                    EmploymentDate = new DateTime(1994, 6, 6),
                    RoleId = Guid.Parse("b4058a62-21e7-4e5e-8ad1-f745e484f733"),
                    HomeAddress = "Lithuania\r\n Klaipeda, Kranto g. 16A"
                },
                new
                {
                    Id = Guid.Parse("0ac8137c-30f4-4441-a653-a79f59f6c96c"),
                    FirstName = "Egle",
                    LastName = "Megle",
                    BirthDate = new DateTime(1986, 2, 13),
                    CurrentSalary = 3000m,
                    EmploymentDate = new DateTime(2022, 01, 9),
                    RoleId = Guid.Parse("80573d10-ee54-45f9-b93c-dad35adf5990"),
                    HomeAddress = "Latvia\r\n Riga, Mieto pr. 18-4"
                }
                );

            modelBuilder.Entity<Role>().HasData(
                new
                {
                    Id = Guid.Parse("80573d10-ee54-45f9-b93c-dad35adf5990"),
                    Name = "CEO",
                    Description = "Big booss"
                },
                new
                {
                    Id = Guid.Parse("bc89ba1f-8873-413d-94a1-316e0b79624d"),
                    Name = "Developer"
                },
                new
                {
                    Id = Guid.Parse("b4058a62-21e7-4e5e-8ad1-f745e484f733"),
                    Name = "Manager",
                    Description = "Manages stuff"
                }
                );


            base.OnModelCreating(modelBuilder);
        }


    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverSQL.Core.Models;

namespace UniverSQL.Core.Data
{
    public class SqlServerDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Alice", DepartmentId = 1 },
                new Employee { Id = 2, Name = "Bob", DepartmentId = 2 }
            );
        }
    }

    // Simulating DB2 (Departments)
    public class Db2DbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public Db2DbContext(DbContextOptions<Db2DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            );
        }
    }
}

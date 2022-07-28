using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmployeesController.Models
{
    public class EmployeesDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        public string DbPath { get; }
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "employees.db");
            Database.EnsureCreated();
        }
        public EmployeesDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "employees.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(GetUsers());
            modelBuilder.Entity<Employee>().HasData(GetEmployees());
            base.OnModelCreating(modelBuilder);
        }
        private List<User> GetUsers()
        {
            return new List<User>
            {
                new User { UserId = 1, Login = "user", Password = "user" },
                new User { UserId = 2, Login = "admin", Password = "admin", IsAdmin = true }
            };
        }
        private List<Employee> GetEmployees()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SimpleEmployeesController.Resources.TestFio.csv";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var lines = reader.ReadToEnd().Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                    var result = lines.Skip(1).Select((line, i) => {
                        var items = line.Split(';');
                        return new Employee
                        {
                            EmployeeId = i+1,
                            FirstName = items[1],
                            LastName = items[2]
                        };
                    }).ToList();
                    return result;
                }
            }
        }
    }
}

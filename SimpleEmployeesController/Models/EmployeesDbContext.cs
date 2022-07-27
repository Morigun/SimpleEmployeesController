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
        public DbSet<EmployeeAdditionalFields> AdditionalFields { get; set; }
        public DbSet<AdditionalFieldDescription> FieldDescriptions { get; set; }

        public string DbPath { get; }
        public EmployeesDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "employees.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalFieldDescription>().HasData(GetAdditionalFieldDescription());
            modelBuilder.Entity<Employee>().HasData(GetEmployees());
            base.OnModelCreating(modelBuilder);
        }

        AdditionalFieldDescription middleNameDescription = new AdditionalFieldDescription
        {
            AdditionalFieldDescriptionId = 1,
            DisplayName = "MiddleName"
        };
        private List<AdditionalFieldDescription> GetAdditionalFieldDescription()
        {
            return new List<AdditionalFieldDescription>
            {
                middleNameDescription
            };
        }
        private List<Employee> GetEmployees()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "EmployeesContext.TestFio.csv";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var lines = reader.ReadToEnd().Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                    return lines.Select((line,i) => {
                        var items = line.Split(';');
                        return new Employee
                        {
                            EmployeeId = i,
                            FirstName = items[0],
                            LastName = items[1],
                            AdditionalFields = new System.Collections.ObjectModel.ObservableCollection<EmployeeAdditionalFields>
                            {
                                new EmployeeAdditionalFields 
                                { 
                                    EmployeeAdditionalFieldsId = 1, 
                                    EmployeeID = i, 
                                    AdditionalFieldDescription = middleNameDescription, 
                                    Value = items[2] 
                                }
                            }
                        };
                    }).ToList();
                }
            }
        }
    }
}
